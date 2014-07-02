using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lines.Properties;
using System.Threading;
namespace Lines
{
	public enum UserMessage { Victory, Defeat, Error, Invalid_Place_Ball, Balls_Appeared, Balls_Appeared_And_Disapeared, Balls_Disapeared, Placing_Ball, Select_Ball, DeselectBall, None }

	public partial class GUI : Form
	{
		public GUI()
		{
			InitializeComponent();
		}


		#region Переменные графики
		BufferedGraphics graphicsManager;
		BufferedGraphicsContext bgc;
		Graphics bufferGraphics;
		Graphics realGraphics;
		#endregion

		Engine engine;
		Button bStartGame;
		Button bSettings;
		Button bBackToMenu;
		Button bRecords;
		int ButtonsWidth	{	get	{	return (int)(this.ClientRectangle.Width / Settings.ButtonToFormFactorWidth);	}	}
		int ButtonsHeight { get { return (int)(this.ClientRectangle.Height / Settings.ButtonToFormFactorHeight); } }
		bool gameStarted = false;
		SafetyDrawing safeDraw;
		Animation animation;
		Ball selectedBall;
		List<Rectangle> nextBallsRec = new List<Rectangle>();
		List<Image> nextBallsImg = new List<Image>();
		List<Image> curNextBallsImg = new List<Image>();
		SettingsForm settingsForm;
		Records records;

		private void InitGraphics()
		{
			//Инициализация графики
			bgc = BufferedGraphicsManager.Current;
			realGraphics = this.CreateGraphics();
			graphicsManager = bgc.Allocate(realGraphics, this.ClientRectangle);
			bufferGraphics = graphicsManager.Graphics;
			safeDraw = new SafetyDrawing(bufferGraphics, graphicsManager);
			animation = new Animation(safeDraw);
		}

		private void InitButtons()
		{
			//Инициализируем кнопки
			bStartGame = new Button(new Rectangle(this.Width / 2 - ButtonsWidth / 2, Settings.DrawIntendHeight, ButtonsWidth, ButtonsHeight));
			bStartGame.activeStateImage = Resources.bStartActive;
			bStartGame.passiveStateImage = Resources.bStartPassive;

			bSettings = new Button(new Rectangle(this.Width / 2 - ButtonsWidth / 2, Settings.DrawIntendHeight * 2 + ButtonsHeight, ButtonsWidth, ButtonsHeight));
			bSettings.activeStateImage = Resources.bSettingsActive;
			bSettings.passiveStateImage = Resources.bSettingsPassive;

			bRecords = new Button(new Rectangle(this.Width / 2 - ButtonsWidth / 2, Settings.DrawIntendHeight * 3 + ButtonsHeight * 2, ButtonsWidth, ButtonsHeight));
			bRecords.activeStateImage = Resources.bRecordsActive;
			bRecords.passiveStateImage = Resources.bRecordsPassive;

			bBackToMenu = new Button(new Rectangle(0, Settings.DrawIntendHeight + Settings.LengthField * Settings.DrawOneCellLength + Settings.DrawOneCellLength, ButtonsWidth, ButtonsHeight));
			bBackToMenu.activeStateImage = Resources.bBackToMenuActive;
			bBackToMenu.passiveStateImage = Resources.bBackToMenuPassive;
		}

		/// <summary>
		/// Обновить графические элементы
		/// </summary>
		void RefleshAll()
		{
			DrawBackground(safeDraw);
			if (gameStarted)
			{
				DrawCells(safeDraw);
				DrawBalls(safeDraw);
				DrawScore(engine, bufferGraphics);
			}
			DrawButtons();
		}

		DataContainer CreateDataContainer()
		{
			DataContainer dc = new DataContainer();
			dc.BallColors = Settings.BallColors;
			dc.CountBallColors = Settings.CountBallColors;
			dc.CountLengthLines = Settings.CountLengthLines;
			dc.CountNextBalls = Settings.CountNextBalls;
			dc.DrawOneCellLength = Settings.DrawOneCellLength;
			dc.LengthField = Settings.LengthField;
			dc.SavedGame = Settings.SavedGame;
			dc.SaveScore = Settings.SaveScore;
			dc.SpeedBallAnimation = Settings.SpeedBallAnimation;
			dc.SpeedBallPlacement = Settings.SpeedBallPlacement;
			dc.SpeedBallsAppearDisapear = Settings.SpeedBallsAppearDisapear;
			dc.SpeedNextBallAnimation = Settings.SpeedNextBallAnimation;
			dc.SpeedNextBallsAppearDisapear = Settings.SpeedNextBallsAppearDisapear;
			dc.GameStarted = gameStarted;
			if (Settings.SaveScore)
				dc.records = records.RecordsData;
			if (Settings.SavedGame && gameStarted)
				dc.engineData = engine.GetSetEngineData;
			else
				dc.GameStarted = false;
			return dc;
		}

		void LoadDataFromDataContainer(DataContainer dc)
		{
			if (dc == null)
				return;
			Settings.BallColors = dc.BallColors;
			Settings.CountBallColors = dc.CountBallColors;
			Settings.CountLengthLines = dc.CountLengthLines;
			Settings.CountNextBalls = dc.CountNextBalls;
			Settings.DrawOneCellLength = dc.DrawOneCellLength;
			Settings.LengthField = dc.LengthField;
			Settings.SavedGame = dc.SavedGame;
			Settings.SaveScore = dc.SaveScore;
			Settings.SpeedBallAnimation = dc.SpeedBallAnimation;
			Settings.SpeedBallPlacement = dc.SpeedBallPlacement;
			Settings.SpeedBallsAppearDisapear = dc.SpeedBallsAppearDisapear;
			Settings.SpeedNextBallAnimation = dc.SpeedNextBallAnimation;
			Settings.SpeedNextBallsAppearDisapear = dc.SpeedNextBallsAppearDisapear;
			gameStarted = dc.GameStarted;
			if (dc.SaveScore && dc.records!=null)
				records.RecordsData = dc.records;
			if (dc.SavedGame && dc.engineData!=null)
				engine.GetSetEngineData = dc.engineData;
		}

		void ChangeFormSize()
		{
			System.Drawing.Size size = new System.Drawing.Size();
			size.Width = Settings.DrawOneCellLength * Settings.LengthField + Settings.DrawIntendWidth*2;
			int y = Settings.DrawIntendHeight + Settings.LengthField * Settings.DrawOneCellLength + Settings.DrawOneCellLength;
			size.Height = y + (int)(y/Settings.ButtonToFormFactorHeight);
			this.ClientSize = size;
		}

		void CreateRecord()
		{
				Record record = new Record();
				record.Score = engine.GetScore;
				record.FieldLength = Settings.LengthField;
				if (Settings.BallColors!=null)
				{
					record.CountColorsPerGame = Settings.BallColors.Length;
				}
				else
					record.CountColorsPerGame = Settings.CountBallColors;
				record.CountBallPerTurn = Settings.CountNextBalls;

				bool isRecord = records.AddRecord(record);
				if (isRecord)
				{
					NewRecord nr = new NewRecord();
					nr.ShowDialog();
					record.Name = nr.GetName;
					ShowRecords();
				}
		}

		private void ShowRecords()
		{
			RecordsWiev rw = new RecordsWiev();
			rw.ViewRecords(records);
			rw.ShowDialog();
		}

		public Rectangle FindCellDrawCoord(Cell cell)
		{
			return new Rectangle(
				Settings.DrawIntendWidth + (cell.X-1)*Settings.DrawOneCellLength,
				Settings.DrawIntendHeight + Settings.DrawOneCellLength + (cell.Y - 1) * Settings.DrawOneCellLength,
				Settings.DrawOneCellLength,
				Settings.DrawOneCellLength);
		}

		/// <summary>
		/// Найти ячейку по координатам точек на форме
		/// </summary>
		/// <param name="x">Координата Х</param>
		/// <param name="y">Координата У</param>
		/// <returns>Найденная ячейка или null</returns>
		public Cell FindCell(int x, int y)
		{
			//Если координаты выходят за пределы игрового поля
			if (
				(x < Settings.DrawIntendWidth || x > Settings.DrawIntendWidth + Settings.DrawOneCellLength * Settings.LengthField) || 
				(y < Settings.DrawIntendHeight + Settings.DrawOneCellLength || y > Settings.DrawIntendHeight + Settings.DrawOneCellLength + Settings.DrawOneCellLength * Settings.LengthField)
				)
			{
				return null;
			}

			int coordinateX = 1;
			int coordinateY = 1;

			//Смотрим по координате X
			for (int z=0; z< Settings.LengthField;z++)
			{
				if (x > Settings.DrawIntendWidth + z * Settings.DrawOneCellLength && 
					x < Settings.DrawIntendWidth + (z + 1) * Settings.DrawOneCellLength)
				{
					coordinateX = z+1;
					break;
				}
			}
			//Смотрим по координате Y
			for (int z = 0; z < Settings.LengthField; z++)
			{
				if (y > Settings.DrawIntendHeight + Settings.DrawOneCellLength + z * Settings.DrawOneCellLength &&
					y < Settings.DrawIntendHeight + Settings.DrawOneCellLength + (z + 1) * Settings.DrawOneCellLength)
				{
					coordinateY = z+1;
					break;
				}
			}
			//Создаем ячейку
			Cell cell = Algoritms.FindCell(coordinateX, coordinateY, engine.GetField.cells);
			return cell;
		}

		void DrawBackground(SafetyDrawing safeDraw)
		{
			var image = Resources.Background2;
			safeDraw.DrawImage(image, new Rectangle(0, 0, this.Width, this.Height));
		}

		void DrawCells(SafetyDrawing safeDraw)
		{
			var cellImage = Resources.Cell;
			//Рисуем поле
			for (int y=0; y< Settings.LengthField;y++)
				for (int x = 0; x < Settings.LengthField; x++)
				{
					safeDraw.DrawImage(
						cellImage, new Rectangle(
						Settings.DrawIntendWidth+x*Settings.DrawOneCellLength,
						Settings.DrawIntendHeight + Settings.DrawOneCellLength + y * Settings.DrawOneCellLength,
						Settings.DrawOneCellLength,
						Settings.DrawOneCellLength));
				}
			//Рисуем поле следующих шариков
			int fieldLength = Settings.LengthField * Settings.DrawOneCellLength;
			int nextBallsFieldLength = Settings.CountNextBalls * Settings.DrawOneCellLength;
			nextBallsRec.Clear();
			for (int x=0; x< Settings.CountNextBalls;x++)
			{
				Rectangle nextBallRec =  new Rectangle(
						Settings.DrawIntendWidth + fieldLength / 2 - nextBallsFieldLength/2 + x * Settings.DrawOneCellLength,
						Settings.DrawIntendHeight,
						Settings.DrawOneCellLength,
						Settings.DrawOneCellLength);
				nextBallsRec.Add(nextBallRec);
				safeDraw.DrawImage(
						cellImage,nextBallRec);
			}
		}

		/// <summary>
		/// Нарисовать все шарики
		/// </summary>
		/// <param name="graphics"></param>
		public void DrawBalls(SafetyDrawing safeDraw)
		{
			for (int x=0; x< engine.GetBalls.Count;x++)
			{
				DrawBall(safeDraw, engine.GetBalls[x]);
			}
		}

		/// <summary>
		/// Нарисовать шарик
		/// </summary>
		/// <param name="graphics">Обьект графики</param>
		/// <param name="ball">Шарик</param>
		public void DrawBall(SafetyDrawing safeDraw, Ball ball)
		{
			//Получаем прямоугольник шарика
			Rectangle ballRect = FindCellDrawCoord(ball.cell);
			//Получаем картинку шарика
			Image img = Algoritms.GetBallImage(ball);
			//Рисуем шарик
			safeDraw.DrawImage(img, ballRect);
		}

		void DrawScore(Engine engine,Graphics graphics)
		{
			int nextBallsFieldLength = Settings.CountNextBalls * Settings.DrawOneCellLength;
			int fieldLength = Settings.LengthField * Settings.DrawOneCellLength;
			Image scoreImg = Resources.scroll;
			Rectangle scoreRect = new Rectangle(
						Settings.DrawIntendWidth + fieldLength / 2 - fieldLength / 5,
						Settings.DrawIntendHeight+ Settings.DrawOneCellLength + fieldLength,
						 fieldLength *2 / 5,
						ButtonsHeight-Settings.DrawIntendHeight);
			Font font = new Font(new FontFamily("Times New Roman"),20);
			graphics.DrawImage(scoreImg, scoreRect);
			graphics.DrawString(engine.GetScore.ToString(), font, Brushes.Blue, scoreRect.X + scoreRect.Width / 2 - engine.GetScore.ToString().Length*font.SizeInPoints/2, scoreRect.Y + scoreRect.Height / 2 - font.GetHeight() / 2);
		}

		void DrawButtons()
		{
			if (!gameStarted)
			{
				bStartGame.DrawButton(safeDraw);
				bSettings.DrawButton(safeDraw);
				bRecords.DrawButton(safeDraw);
			}
			else
				bBackToMenu.DrawButton(safeDraw);

		}

		void AnimateAppearance(Engine engine)
		{
			List<Rectangle> appearRect = new List<Rectangle>();
			List<Image> appearImg = new List<Image>();
			for (int x=0; x< engine.GetApearedBalls.Length;x++)
			{
				appearRect.Add(FindCellDrawCoord( engine.GetApearedBalls[x].cell));
				appearImg.Add(Algoritms.GetBallImage(engine.GetApearedBalls[x]));
			}
			animation.StartAnimateAppearance(appearRect, appearImg);
		}

		void AnimateDisappearance(Engine engine)
		{
			List<Rectangle> appearRect = new List<Rectangle>();
			List<Image> appearImg = new List<Image>();
			for (int x = 0; x < engine.GetDisapearedBalls.Length; x++)
			{
				appearRect.Add(FindCellDrawCoord(engine.GetDisapearedBalls[x].cell));
				appearImg.Add(Algoritms.GetBallImage(engine.GetDisapearedBalls[x]));
			}
			animation.StartAnimateDisappearance(appearRect, appearImg);

		}
		
		void AnimatePlacement(Engine engine)
		{
			List<Rectangle> rectPath = new List<Rectangle>();
			Image imgBall = Algoritms.GetBallImage(selectedBall);
			for (int x = 0; x < engine.GetBallPath.Length; x++)
			{
				rectPath.Add(FindCellDrawCoord(engine.GetBallPath[x]));
			}
			animation.StartAnimatePlacement(rectPath, imgBall);

		}
		void AnimateAppearanceNextBalls(Engine engine)
		{
			nextBallsImg.Clear();
			for (int x = 0; x < engine.GetNextColors.Length; x++)
			{
				nextBallsImg.Add(Algoritms.GetBallImage(engine.GetNextColors[x]));
			}
			animation.StartAnimateAppearedNextBalls(nextBallsRec, nextBallsImg);
		}

		void AnimateDisappearanceNextBalls(Engine engine)
		{
			Image[] array = new Image[nextBallsImg.Count];
			nextBallsImg.CopyTo(array);
			curNextBallsImg = new List<Image>(array);
			animation.StartAnimateDisappearedNextBalls(nextBallsRec, curNextBallsImg);
		}
		/// <summary>
		/// Изменить статус нажатия кнопки
		/// </summary>
		/// <param name="x">Координата Х нажатиия на форме</param>
		/// <param name="y">Координата У нажатиия на форме</param>
		/// <returns>Измененная кнопка или null</returns>
		Button ChangeStateButton(int x, int y)
		{
			Point clickedPlace = new Point(x,y);
			Button button;
			if (!gameStarted)
			{
				button = ChangeStateOneButton(clickedPlace, bStartGame);
				if (button != null)
					return button;
				button = ChangeStateOneButton(clickedPlace, bSettings);
				if (button != null)
					return button;
				button = ChangeStateOneButton(clickedPlace, bRecords);
			}
			else
				button = ChangeStateOneButton(clickedPlace, bBackToMenu);
			return button;
		}

		void SetBalls(bool firstSet)
		{
			if (!firstSet)
				AnimateDisappearanceNextBalls(engine);
			var res = engine.SetBalls();
			if (res == UserMessage.Balls_Appeared_And_Disapeared)
			{
				AnimateAppearanceNextBalls(engine);
				AnimateAppearance(engine);
				AnimateDisappearance(engine);
				DrawScore(engine, bufferGraphics);
			}
			if (res == UserMessage.Balls_Appeared)
			{
				AnimateAppearanceNextBalls(engine);
				AnimateAppearance(engine);
				DrawScore(engine, bufferGraphics);
				safeDraw.Render();
			}
			if (res == UserMessage.Defeat)
			{
				CreateRecord();
				var dialogRes = MessageBox.Show("Начать игру сначала?", "", MessageBoxButtons.YesNo);
				if (dialogRes == DialogResult.Yes)
				{
					nextBallsRec = new List<Rectangle>();
					gameStarted = true;
					engine.StartGame();
					RefleshAll();
					SetBalls(true);
				}
				if (dialogRes == DialogResult.No)
				{
					gameStarted = false;
					animation.StopAnimations();
					RefleshAll();
				}
			}
		}
		/// <summary>
		/// Изменить статус нажатия конкретной кнопки
		/// </summary>
		/// <param name="clickedPlace">Точка нажатия</param>
		/// <param name="button">Кнопка</param>
		/// <returns>Измененная кнопка</returns>
		private Button ChangeStateOneButton(Point clickedPlace, Button button)
		{
			if (button.GetButtonRectangle.Contains(clickedPlace))
			{
				if (button.PushedState)
					button.PushedState = false;
				else
					button.PushedState = true;
				return button;
			}
			return null;
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			//Инициализируем движок
			engine = new Engine();

			records = new Records();
			DataContainer dc = Saver.LoadData();
			LoadDataFromDataContainer(dc);
			ChangeFormSize();
			InitGraphics();
			InitButtons();
			RefleshAll();
			if (gameStarted)
			{
				AnimateAppearanceNextBalls(engine);
			}
		}

		private void GUI_Paint(object sender, PaintEventArgs e)
		{
			DrawBackground(safeDraw);
			RefleshAll();
			DrawButtons();
			safeDraw.Render();
		}

		private void GUI_MouseDown(object sender, MouseEventArgs e)
		{
			ChangeStateButton(e.X, e.Y);
			DrawButtons();
			safeDraw.Render();
		}

		private void GUI_MouseUp(object sender, MouseEventArgs e)
		{
			//Работаем с кнопками
			Button button = ChangeStateButton(e.X, e.Y);
			if (button != null)
			{
				if (button == bStartGame)
				{
					nextBallsRec = new List<Rectangle>();
					gameStarted = true;
					engine.StartGame();
					RefleshAll();
					SetBalls(true);
					safeDraw.Render();
				}
				else
					if (button == bSettings)
					{
						settingsForm = new SettingsForm();
						settingsForm.ShowDialog();
						ChangeFormSize();
						InitGraphics();
						InitButtons();
						RefleshAll();
					}
					else
						if (button == bBackToMenu)
						{
							gameStarted = false;
							animation.StopAnimations();
							RefleshAll();
						}
						else
							if (button == bRecords)
							{
								ShowRecords();
							}
				DrawButtons();
				safeDraw.Render();
			}
		}

		private void GUI_MouseClick(object sender, MouseEventArgs e)
		{
			if (gameStarted&&!animation.Locked)
			{
				Cell cell = FindCell(e.X, e.Y);
				if (cell == null)
					return;
				var res = engine.FieldClick(cell);
				if (res == UserMessage.Select_Ball)
				{
					Rectangle rec = FindCellDrawCoord(cell);
					Image img =Algoritms.GetBallImage(Algoritms.FindBall(cell,engine.GetBalls));
					selectedBall = Algoritms.FindBall(cell, engine.GetBalls);
					DrawBalls(safeDraw);
					safeDraw.Render();
					animation.StartAnimateSelection(rec, img);
				}
				if (res == UserMessage.DeselectBall)
				{
					selectedBall = null;
					animation.DisableAnimateSelection();
					DrawBalls(safeDraw);
					safeDraw.Render();
				}
				if (res == UserMessage.Placing_Ball)
				{
					animation.DisableAnimateSelection();
					AnimatePlacement(engine);
					SetBalls(false);
					safeDraw.Render();
				}
				if (res == UserMessage.Balls_Disapeared)
				{
					animation.DisableAnimateSelection();
					AnimatePlacement(engine);
					AnimateDisappearance(engine);
					DrawScore(engine, bufferGraphics);
					if (engine.GetBalls.Count==0)
					{
						SetBalls(false);
					}
					safeDraw.Render();
				}
			}
		}

		private void GUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			Saver.SaveData(CreateDataContainer());
		}

	}
}
