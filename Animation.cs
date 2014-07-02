using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Timers;
using Lines.Properties;
using System.Threading;
using Timer = System.Windows.Forms.Timer;
namespace Lines
{
	enum AnimationType { Appearance, ChangeSize, Disapearance, Placement}
	public class Animation
	{
		///<summary>Количество кадров в секунду</summary>
		const double TimerInterval = 1000 / 24;
		///<summary>Таймер анимации</summary>
		Timer animationTimer;
		///<summary>Обьект графики</summary>
		SafetyDrawing safeDraw;
		Queue<AnimationType> animQueue = new Queue<AnimationType>();
		public bool Locked { 
			get 
			{
				if (animQueue.Count>0 || nextBallsApeared || nextBallsDissapeared)
					return true;
				else
					return false;
			} 
		}
		#region Переменные для анимации выделенного шарика
				int selectedBallAnimationNumber;
				bool selectedBallImageDecreasing;
				bool SelectedBallAnimated { get; set; }
				Rectangle selectedBallRectangle;
				Image SelectedBallImage { get; set; }
		#endregion
		#region Переменные для анимации следующих шариков
				int[] nextBallsAnimationNumbers;
				int[] nextBallsApearedAnimationNumbers;
				int[] nextBallsDisapearedAnimationNumbers;
				bool[] nextBallsImageDecreasing;
				public bool nextBallsAnimated { get; private set; }
				public bool nextBallsDissapeared { get; private set; }
				public bool nextBallsApeared { get; private set; }
				List<Rectangle> nextBallsRectangles;
				List<Image> nextBallsCurImages;
				List<Image> nextBallsNextImages;
		#endregion
		#region Переменные для анимации появлений шариков
				List<Rectangle> AppearBallsRectangles = new List<Rectangle>();
				List<Image> AppearBallsImages = new List<Image>();
				int AppearBallAnimationNumber { get; set; }
		#endregion
		#region Переменные для анимации исчезаний шариков
				List<Rectangle> DisappearBallsRectangles = new List<Rectangle>();
				List<Image> DisappearBallsImages = new List<Image>();
				int DisappearBallAnimationNumber { get; set; }
		#endregion
		#region Переменные для анимации передвижения шарика
				List<Rectangle> ballPathRec = new List<Rectangle>();
				Rectangle placeBallRect;
				Image placeBallImg;
				bool placeBallDecreasing = true;
				int placeRecIndex = int.MaxValue;
				int placeBallAnimationNumber;
		#endregion

		Image emptyCellImg = Resources.Cell;

		public Animation(SafetyDrawing safeDraw)
		{
			this.safeDraw = safeDraw;
			selectedBallImageDecreasing = true;
			selectedBallAnimationNumber = 1;
			AppearBallAnimationNumber = 0;
			DisappearBallAnimationNumber = 0;
			placeBallAnimationNumber = 1;
			animationTimer = new Timer();
			animationTimer.Interval = (int)TimerInterval;
			//animationTimer.AutoReset = true;
			animationTimer.Tick += new EventHandler(animationTimer_Elapsed);
		}

		


		/// <summary>
		/// Начать анимировать выделенный шарик
		/// </summary>
		/// <param name="rectangle">Область шарика</param>
		/// <param name="image">Картинка шарика</param>
		public void StartAnimateSelection(Rectangle rectangle, Image image)
		{
			//Проверка входных параметров
			if (rectangle == Rectangle.Empty || image == null)
				return;
			//Инициализация
			this.selectedBallRectangle = rectangle;
			this.SelectedBallImage = image;
			selectedBallImageDecreasing = true;
			selectedBallAnimationNumber = 1;
			//Начало анимации
			SelectedBallAnimated = true;
			animationTimer.Start();
		}
		/// <summary>
		/// Остановить анимацию
		/// </summary>
		public void DisableAnimateSelection()
		{
			//Конец анимации
			SelectedBallAnimated = false;
			selectedBallImageDecreasing = true;
			selectedBallAnimationNumber = 1;
			Thread.Sleep(100);
		}

		public void StartAnimateNextBalls(List<Rectangle> nextBallsRect, List<Image> nextBallsImg)
		{
			nextBallsAnimated = true;
			nextBallsRectangles = nextBallsRect;
			nextBallsCurImages = nextBallsImg;
			nextBallsAnimationNumbers = new int[nextBallsRect.Count];
			nextBallsImageDecreasing = new bool[nextBallsRect.Count];
			for (int x=0; x< nextBallsRect.Count;x++)
			{
				nextBallsAnimationNumbers[x] = x*Settings.AnimAmplitude / Settings.CountNextBalls;
			}
		}

		public void StartAnimateAppearedNextBalls(List<Rectangle> nextBallsRect, List<Image> nextBallsImg)
		{
			nextBallsApeared = true;
			nextBallsRectangles = nextBallsRect;
			nextBallsNextImages = nextBallsImg;
			nextBallsApearedAnimationNumbers = new int[nextBallsRect.Count];
			for (int x = 0; x < nextBallsRect.Count; x++)
			{
				nextBallsApearedAnimationNumbers[x] =0;
			}
			animationTimer.Start();
		}

		public void StartAnimateDisappearedNextBalls(List<Rectangle> nextBallsRect, List<Image> nextBallsImg)
		{
			nextBallsDissapeared = true;
			nextBallsAnimated = false;
			nextBallsCurImages = nextBallsImg;
			nextBallsDisapearedAnimationNumbers = new int[nextBallsRect.Count];
			for (int x = 0; x < nextBallsRect.Count; x++)
			{
				nextBallsDisapearedAnimationNumbers[x] = 0;
			}
		}

		public void StartAnimateAppearance(List<Rectangle> appearedBallsRect, List<Image> ballsImages)
		{
			if (appearedBallsRect == null && appearedBallsRect.Count <= 0 && ballsImages == null && ballsImages.Count <= 0)
				return;
			this.AppearBallsImages = ballsImages;
			this.AppearBallsRectangles = appearedBallsRect;
			//this.AppearedBallAnimated = true;
			animQueue.Enqueue(AnimationType.Appearance);
			this.AppearBallAnimationNumber = 0;
			animationTimer.Start();
		}

		
		public void StartAnimateDisappearance(List<Rectangle> disappearedBallsRect, List<Image> ballsImages)
		{
			if (disappearedBallsRect == null && disappearedBallsRect.Count <= 0 && ballsImages == null && ballsImages.Count <= 0)
				return;
			this.DisappearBallsImages = ballsImages;
			this.DisappearBallsRectangles = disappearedBallsRect;
			//this.DisappearedBallAnimated = true;
			animQueue.Enqueue(AnimationType.Disapearance);
		}

		public void StartAnimatePlacement(List<Rectangle> ballPath, Image ballImg)
		{
			this.placeBallImg = ballImg;
			this.placeBallDecreasing = true;
			this.ballPathRec = ballPath;
			this.placeBallRect = ballPath[0];
			this.placeRecIndex = 0;
			//this.placeBallAnimated = true;
			placeBallAnimationNumber = 1;
			animQueue.Enqueue(AnimationType.Placement);
			animationTimer.Start();
		}

		public void StopAnimations()
		{
			animationTimer.Stop();
			animQueue.Clear();
		}
		/// <summary>
		/// Применить анимацию
		/// </summary>
		void AnimateSelection()
		{
			if (SelectedBallAnimated)
			{
				ClearCell(selectedBallRectangle);
				BallChangingSize(selectedBallRectangle,ref selectedBallAnimationNumber,SelectedBallImage,ref selectedBallImageDecreasing,Settings.SpeedBallAnimation);
			}
		}

		void AnimateNextBalls()
		{
			if (nextBallsAnimated && !nextBallsDissapeared && !nextBallsApeared)
			{
				AnimateNextBallsChangingSize(nextBallsRectangles, nextBallsCurImages, ref nextBallsAnimationNumbers);
			}
			else
				if (!nextBallsDissapeared && nextBallsApeared)
				{
					AnimateNextBallsApeared(nextBallsRectangles, nextBallsNextImages, ref nextBallsApearedAnimationNumbers);
				}
				else
					if (nextBallsDissapeared)
					{
						AnimateNextBallsDisapeared(nextBallsRectangles, nextBallsCurImages, ref nextBallsDisapearedAnimationNumbers);
					}
		}
		void AnimateNextBallsChangingSize(List<Rectangle> ballRec, List<Image> nextBallImage,ref int []nextBallAnimationNumber)
		{
				for (int x = 0; x < ballRec.Count; x++)
				{
					ClearCell(ballRec[x]);
					BallChangingSize(ballRec[x], ref nextBallAnimationNumber[x], nextBallImage[x], ref nextBallsImageDecreasing[x], Settings.SpeedNextBallAnimation);
				}
		}
		void AnimateNextBallsApeared(List<Rectangle> ballRec, List<Image> nextBallImage, ref int[] nextBallsApearedAnimationNumbers)
		{
				List<Rectangle> newRectangles= new List<Rectangle>();
				for (int x = 0; x < ballRec.Count; x++)
				{
					if (ballRec[x].Width - nextBallsApearedAnimationNumbers[x] * 2 < 0)
					{
						for (int y = 0; y < ballRec.Count; y++)
						{
							nextBallsApearedAnimationNumbers[y] = 0;
						}
						nextBallsApeared = false;
						StartAnimateNextBalls(ballRec, nextBallImage);
						return;
					}
					int halfX = ballRec[x].X + ballRec[x].Width / 2;
					int halfY = ballRec[x].Y + ballRec[x].Height / 2;

					newRectangles.Add(new Rectangle(
						halfX - nextBallsApearedAnimationNumbers[x],
						halfY - nextBallsApearedAnimationNumbers[x],
						nextBallsApearedAnimationNumbers[x] * 2,
						nextBallsApearedAnimationNumbers[x] * 2));
				}
				for (int x = 0; x < ballRec.Count; x++)
				{
					ClearCell(ballRec[x]);
					safeDraw.DrawImage(nextBallImage[x], newRectangles[x]);
					nextBallsApearedAnimationNumbers[x] += Settings.SpeedNextBallsAppearDisapear;
				}
		}

		void AnimateNextBallsDisapeared(List<Rectangle> ballRec, List<Image> nextBallImage, ref int[] nextBallsDisapearedAnimationNumbers)
		{
			List<Rectangle> newRectangles = new List<Rectangle>();
			for (int x = 0; x < ballRec.Count; x++)
			{
				if (nextBallsDisapearedAnimationNumbers[x] >= ballRec[x].Width / 2)
				{
					nextBallsDissapeared = false;
					for (int y = 0; y < ballRec.Count; y++)
					{
						nextBallsDisapearedAnimationNumbers[y] = 0;
					}
					return;
				}

				newRectangles.Add(new Rectangle(
					ballRec[x].X + nextBallsDisapearedAnimationNumbers[x],
					ballRec[x].Y + nextBallsDisapearedAnimationNumbers[x],
					ballRec[x].Width - nextBallsDisapearedAnimationNumbers[x] * 2,
					ballRec[x].Height - nextBallsDisapearedAnimationNumbers[x] * 2));
			}
			for (int x = 0; x < ballRec.Count; x++)
			{
				ClearCell(ballRec[x]);
				safeDraw.DrawImage(nextBallImage[x], newRectangles[x]);
				nextBallsDisapearedAnimationNumbers[x] += Settings.SpeedNextBallsAppearDisapear;
			}
		}
		void AnimatePlacement()
		{
			if (animQueue.Count == 0)
				return;
			if (animQueue.Peek() != AnimationType.Placement)
			{
				return;
			}

			if (placeRecIndex > ballPathRec.Count-1)
			{
				return;
			}
			if (placeRecIndex == ballPathRec.Count-1)
			{
				safeDraw.DrawImage(placeBallImg, ballPathRec[ballPathRec.Count - 1]);
				placeRecIndex = int.MaxValue;
				animQueue.Dequeue();
				return;
			}

			Rectangle recStart = ballPathRec[placeRecIndex];
			Rectangle recEnd = ballPathRec[placeRecIndex + 1];
			Point centerRecStart = new Point(recStart.X + (recStart.Width / 2), recStart.Y + (recStart.Height / 2));
			Point centerRecEnd = new Point(recEnd.X + (recEnd.Width / 2), recEnd.Y + (recEnd.Height / 2));
			Point centerPlaceBallRec = new Point(placeBallRect.X + (placeBallRect.Width / 2), placeBallRect.Y + (placeBallRect.Height / 2));
			
			if (centerPlaceBallRec == centerRecEnd)
			{
				placeRecIndex++;
				if (placeRecIndex == ballPathRec.Count - 1)
				{
					safeDraw.DrawImage(placeBallImg, ballPathRec[ballPathRec.Count - 1]);
					placeRecIndex = int.MaxValue;
					animQueue.Dequeue();
					return;
				}
				recStart = ballPathRec[placeRecIndex];
				recEnd = ballPathRec[placeRecIndex + 1];
				centerRecStart = new Point(recStart.X + (recStart.Width / 2), recStart.Y + (recStart.Height / 2));
				centerRecEnd = new Point(recEnd.X + (recEnd.Width / 2), recEnd.Y + (recEnd.Height / 2));
				centerPlaceBallRec = new Point(placeBallRect.X + (placeBallRect.Width / 2), placeBallRect.Y + (placeBallRect.Height / 2));
				ClearCell(recStart);
				ClearCell(recEnd);
				AnimatePlacementBetweenRect(centerRecStart, centerRecEnd, recEnd, placeBallImg);
			}
			else
			{
				ClearCell(recStart);
				ClearCell(recEnd);
				AnimatePlacementBetweenRect(centerRecStart, centerRecEnd, recEnd, placeBallImg);
			}
		}

		void AnimatePlacementBetweenRect(Point centerRecStart, Point centerRecEnd,Rectangle lastRec, Image img)
		{
			Point centerPlaceBallRec;
			if (centerRecStart == centerRecEnd || img == null)
			{
				return;
			}
			//Если сдвиг вправо
			if (centerRecStart.Y == centerRecEnd.Y && centerRecStart.X < centerRecEnd.X)
			{
				placeBallRect.X += Settings.SpeedBallPlacement;
				centerPlaceBallRec = new Point(placeBallRect.X + (placeBallRect.Width / 2), placeBallRect.Y + (placeBallRect.Height / 2));
				if (centerPlaceBallRec.X > centerRecEnd.X)
				{
					placeBallRect.X = lastRec.X;
				}
			}
			else
				//Если сдвиг влево
				if (centerRecStart.Y == centerRecEnd.Y && centerRecStart.X > centerRecEnd.X)
				{
					placeBallRect.X -= Settings.SpeedBallPlacement;
					centerPlaceBallRec = new Point(placeBallRect.X + (placeBallRect.Width / 2), placeBallRect.Y + (placeBallRect.Height / 2));
					if (centerPlaceBallRec.X  < centerRecEnd.X)
					{
						placeBallRect.X = lastRec.X;
					}
				}
				else
					//Если сдвиг вниз
					if (centerRecStart.Y < centerRecEnd.Y && centerRecStart.X == centerRecEnd.X)
					{
						placeBallRect.Y += Settings.SpeedBallPlacement;
						centerPlaceBallRec = new Point(placeBallRect.X + (placeBallRect.Width / 2), placeBallRect.Y + (placeBallRect.Height / 2));
						if (centerPlaceBallRec.Y > centerRecEnd.Y)
						{
							placeBallRect.Y = lastRec.Y;
						}
					}
					else
						//Если сдвиг вверх
						if (centerRecStart.Y > centerRecEnd.Y && centerRecStart.X == centerRecEnd.X)
						{
							placeBallRect.Y -= Settings.SpeedBallPlacement;
							centerPlaceBallRec = new Point(placeBallRect.X + (placeBallRect.Width / 2), placeBallRect.Y + (placeBallRect.Height / 2));
							if (centerPlaceBallRec.Y < centerRecEnd.Y)
							{
								placeBallRect.Y = lastRec.Y;
							}
						}
			BallChangingSize(placeBallRect,ref placeBallAnimationNumber, placeBallImg,ref placeBallDecreasing,Settings.SpeedBallAnimation);
		}

		/// <summary>
		/// Анимация появлений шариков
		/// </summary>
		void AnimateAppearance()
		{
			if (animQueue.Count != 0)
			if (animQueue.Peek()== AnimationType.Appearance)
			{
				if (AppearBallsRectangles[0].Width - AppearBallAnimationNumber * 2 < 0)
				{
					AppearBallAnimationNumber = 0;
					for (int x = 0; x < AppearBallsRectangles.Count; x++)
					{
						ClearCell(AppearBallsRectangles[x]);
						safeDraw.DrawImage(AppearBallsImages[x], AppearBallsRectangles[x]);
					}
					animQueue.Dequeue();
					return;
				}

				List<Rectangle> newRectangles = new List<Rectangle>();
				for (int x = 0; x < AppearBallsRectangles.Count; x++)
				{
					int halfX = AppearBallsRectangles[x].X + AppearBallsRectangles[x].Width / 2;
					int halfY = AppearBallsRectangles[x].Y + AppearBallsRectangles[x].Height / 2;

					newRectangles.Add(new Rectangle(
						halfX - AppearBallAnimationNumber,
						halfY - AppearBallAnimationNumber,
						AppearBallAnimationNumber * 2,
						AppearBallAnimationNumber * 2));
				}

				for (int x = 0; x < newRectangles.Count; x++)
				{
					ClearCell(AppearBallsRectangles[x]);
					safeDraw.DrawImage(AppearBallsImages[x], newRectangles[x]);
				}
				AppearBallAnimationNumber+=Settings.SpeedBallsAppearDisapear;
			}
		}

		void AnimateDisappearance()
		{
			if (animQueue.Count!=0)
			if (animQueue.Peek()== AnimationType.Disapearance)
			{
				if (DisappearBallAnimationNumber > DisappearBallsRectangles[0].Width / 2)
				{
					for (int x = 0; x < DisappearBallsRectangles.Count; x++)
					{
						ClearCell(DisappearBallsRectangles[x]);
					}
					DisappearBallAnimationNumber = 0;
					animQueue.Dequeue();
					return;
				}

				List<Rectangle> newRectangles = new List<Rectangle>();
				for (int x = 0; x < DisappearBallsRectangles.Count; x++)
				{
					newRectangles.Add(new Rectangle(
						DisappearBallsRectangles[x].X + DisappearBallAnimationNumber,
						DisappearBallsRectangles[x].Y + DisappearBallAnimationNumber,
						DisappearBallsRectangles[x].Width - DisappearBallAnimationNumber * 2,
						DisappearBallsRectangles[x].Height - DisappearBallAnimationNumber * 2));
				}
				for (int x = 0; x < newRectangles.Count; x++)
				{
					ClearCell(DisappearBallsRectangles[x]);
					safeDraw.DrawImage(DisappearBallsImages[x], newRectangles[x]);
				}
				DisappearBallAnimationNumber += Settings.SpeedBallsAppearDisapear;
			}
		}

		private void BallChangingSize(Rectangle selectedBallRectangle, ref int BallAnimationNumber, Image ballImage,ref bool BallImageDecreasing,int speedAnimation)
		{
			if (BallAnimationNumber >= Settings.AnimAmplitude)
			{
				BallAnimationNumber = Settings.AnimAmplitude;
				BallImageDecreasing = false;
			}
			if (BallAnimationNumber <= 0)
			{
				BallAnimationNumber = 0;
				BallImageDecreasing = true;
			}
			Rectangle newAnimImgRect;
			if (BallImageDecreasing)
			{
				int startX = selectedBallRectangle.X + BallAnimationNumber;
				int startY = selectedBallRectangle.Y + BallAnimationNumber;
				newAnimImgRect = new Rectangle(
					startX,
					startY,
					selectedBallRectangle.Width - BallAnimationNumber * 2,
					selectedBallRectangle.Height - BallAnimationNumber * 2);
				BallAnimationNumber += speedAnimation;
			}
			else
			{
				int startX = selectedBallRectangle.X + BallAnimationNumber;
				int startY = selectedBallRectangle.Y + BallAnimationNumber;
				newAnimImgRect = new Rectangle(
					startX,
					startY,
					selectedBallRectangle.Width - BallAnimationNumber * 2,
					selectedBallRectangle.Height - BallAnimationNumber * 2);
				BallAnimationNumber -= speedAnimation;
			}
			safeDraw.DrawImage(ballImage, newAnimImgRect);
		}

		void ClearCell(Rectangle cellRect)
		{
			safeDraw.DrawImage(emptyCellImg, cellRect);
		}


		void animationTimer_Elapsed(object sender, EventArgs e)
		{
			AnimatePlacement();
			AnimateSelection();
			AnimateAppearance();
			AnimateDisappearance();
			AnimateNextBalls();
			safeDraw.Render();
		}
	}
}
