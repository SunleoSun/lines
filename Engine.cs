using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lines
{
	[Serializable]
	public class EngineData
	{
		public Field field=null;
		public Ball[] balls = null;
		public int score;
		///<summary>Массив индексов цветов, которые будут присутствовать в игре</summary>
		public int[] colorsInGame;
		/// <summary>Массив цветов следующих шариков </summary>
		public int[] nextColors;
		///<summary>Путь движения текущего выделенного шарика</summary>
		public Cell[] ballPath;
		///<summary>Шарики, которые появились на поле</summary>
		public Ball[] apearedBalls;
		///<summary>Шарики, которые исчезнут с поля</summary>
		public Ball[] disapearedBalls;
	}

	public class Engine
	{
		/// <summary>Поле </summary>
		Field field;
		/// <summary>Получить поле </summary>
		public Field GetField	{	get { return field; }	}
		/// <summary> Список шариков</summary>
		List<Ball> balls;
		/// <summary> Получить список шариков</summary>
		public List<Ball> GetBalls	{	get { return balls; }	}
		/// <summary>Счет </summary>
		int score = 0;
		/// <summary> Счет </summary>
		public int GetScore	{	get { return score; } }
		///<summary>Массив индексов цветов, которые будут присутствовать в игре</summary>
		int[] colorsInGame;
		/// <summary>Массив цветов следующих шариков </summary>
		int[] nextColors;
		/// <summary> Массив цветов следующих шариков </summary>
		public int[] GetNextColors 	{	get { return nextColors; }	}
		/// <summary> Выделенный шарик </summary>
		Ball selectedBall;
		///<summary>Получить выделенный шарик</summary>
		public Lines.Ball GetSelectedBall	{	get { return selectedBall; }}
		///<summary>Путь движения текущего выделенного шарика</summary>
		Cell[] ballPath;
		///<summary>Получить путь движения текущего выделенного шарика</summary>
		public Cell[] GetBallPath { get { return ballPath; } }
		///<summary>Шарики, которые появились на поле</summary>
		Ball[] apearedBalls;
		///<summary>Получить шарики, которые появились на поле</summary>
		public Ball[] GetApearedBalls { get { return apearedBalls; } }
		///<summary>Шарики, которые исчезнут с поля</summary>
		Ball[] disapearedBalls;
		///<summary>Получить шарики, которые исчезнут с поля</summary>
		public Ball[] GetDisapearedBalls { get { return disapearedBalls; } }

		public EngineData GetSetEngineData 
		{
			get
			{
				EngineData ed = new EngineData();
				ed.apearedBalls = apearedBalls;
				ed.ballPath = ballPath;
				ed.balls = new Ball[balls.Count];
				balls.CopyTo(ed.balls);
				ed.colorsInGame = colorsInGame;
				ed.disapearedBalls = disapearedBalls;
				ed.field = field;
				ed.nextColors = nextColors;
				ed.score = score;
				return ed;
			}
			set
			{
				apearedBalls=value.apearedBalls;
				ballPath = value.ballPath;
				balls = new List<Ball>(value.balls);
				colorsInGame = value.colorsInGame;
				disapearedBalls = value.disapearedBalls;
				field = value.field;
				nextColors = value.nextColors;
				score = value.score;
			}
		}
		/// <summary>
		/// Начать игру
		/// </summary>
		public void StartGame()
		{
			colorsInGame = new int[Settings.CountBallColors];
			nextColors = new int[Settings.CountNextBalls];
			balls = new List<Ball>();
			field = new Field(Settings.LengthField);
			selectedBall = null;
			score = 0;
			//Создаем все цвета шариков
			CreateColors();
			//Находим новые цвета следующих шариков
			CreateNextBallsColor(ref nextColors);
		}

		///<summary>Выбирает цвета шариков для текущей игры</summary>
		private void CreateColors()
		{
			Random rand = new Random();
			if (Settings.BallColors!=null)
			{
				colorsInGame = Settings.BallColors;
				return;
			}
			//Цикл по всему массиву цветов шариков
			for (int z = 0; z < colorsInGame.Length; z++)
				//Бесконечный цикл
				for (int x = 0; x < Settings.CycleMaxLength; x++)
				{
					//Устанавливаем случайное число цвета
					int color = rand.Next(1, Settings.CountAllColors+1);
					bool alreadyExists = false;
					//Ищем на наличие повторений такого цвета
					for (int y = 0; y < colorsInGame.Length; y++)
					{
						//Если цвет повторился то выход из цикла
						if (colorsInGame[y] == color)
						{
							alreadyExists = true;
							break;
						}
					}
					//Если цвета шарика нет, то добавить в массив
					if (!alreadyExists)
					{
						colorsInGame[z] = color;
					}
				}
		}
		/// <summary>
		/// Определить цвета следующих шариков
		/// </summary>
		/// <param name="colors">Массив следующих цветов</param>
		public void CreateNextBallsColor(ref int[] colors)
		{
			Random rand = new Random();
			//Инициализация цветов случ числами
			for (int x = 0; x < Settings.CountNextBalls; x++)
			{
				int colorIndex = rand.Next(Settings.CountBallColors);
				colors[x] = colorsInGame[colorIndex];
			}
		}

		/// <summary>
		/// Установить следующие шарики на поле
		/// </summary>
		/// <returns>Результат установки</returns>
		public UserMessage SetBalls()
		{
			apearedBalls = new Ball[Settings.CountNextBalls];
			List<Ball> tempDisapearedBalls = new List<Ball>();
			Random rand = new Random();
			for (int x=0; x< Settings.CountNextBalls;x++)
			{
				for (int y=0; y< Settings.CycleMaxLength;y++)
				{
					
					//Если все ячейки заняты шариками, поражение
					if (balls.Count+1 >= Settings.LengthField*Settings.LengthField)
						return UserMessage.Defeat;
					//Если бесконечный цикл
					if (y == Settings.CycleMaxLength - 1)
						return UserMessage.Error;
					//Находим случайную ячейку, и проверяем, чтобы не было на ней шариков
					int cellIndex = rand.Next(0,Settings.LengthField * Settings.LengthField);
					if (Algoritms.FindBall(field.cells[cellIndex], balls)==null)
					{
						//Создаем шарик и добавляем в список установленных
						Ball ball = new Ball(field.cells[cellIndex],nextColors[x]);
						balls.Add(ball);
						field.cells[cellIndex].BallHere = true;
						for (int z = 0; z < apearedBalls.Length; z++)
							if (apearedBalls[z] == null)
							{
								apearedBalls[z] = ball;
								break;
							}

						//Получаем шарики которые исчезнут
						Algoritms.BallDisapeared(ball, field.cells, balls, tempDisapearedBalls);
						//Если все шарики установлены
						if (x == Settings.CountNextBalls - 1)
						{
							//Находим новые цвета следующих шариков
							CreateNextBallsColor(ref nextColors);
							//Если есть исчезнувшие шарики
							if (tempDisapearedBalls.Count > 0)
							{
								//Удаляем шарики
								DisapearBalls(ball,tempDisapearedBalls);
								disapearedBalls = new Ball[tempDisapearedBalls.Count];
								tempDisapearedBalls.CopyTo(disapearedBalls);
								score += tempDisapearedBalls.Count;
								return UserMessage.Balls_Appeared_And_Disapeared;
							}
							else
								return UserMessage.Balls_Appeared;
						}
						break;
					}
					else
						continue;
				}
			}
			return UserMessage.None;
		}

		/// <summary>
		/// Переместить выделенный шарик в указанную ячейку
		/// </summary>
		/// <param name="cell">Ячейка</param>
		/// <returns>Результат</returns>
		public UserMessage PlaceBall(Cell cell)
		{
			UserMessage message = UserMessage.None;
			//Перемещаем шарик
			selectedBall.cell.BallHere = false;
			cell.BallHere = true;
			selectedBall.cell = cell;
			List<Ball> tempDisapearedBalls = new List<Ball>();
			Algoritms.BallDisapeared(selectedBall, field.cells, balls, tempDisapearedBalls);
			//Если есть исчезнувшие шарики
			if (tempDisapearedBalls.Count > 0)
			{
				//Удаляем шарики
				DisapearBalls(selectedBall,tempDisapearedBalls);
				score += tempDisapearedBalls.Count;
				disapearedBalls = new Ball[tempDisapearedBalls.Count];
				tempDisapearedBalls.CopyTo(disapearedBalls);
				message = UserMessage.Balls_Disapeared;
			}
			else
				message = UserMessage.Placing_Ball;
			return message;
		}

		void DisapearBalls(Ball placedBall, List<Ball> disapearedBalls)
		{
			//Удаляем шарики которые должны исчезнуть
			for (int x = 0; x < disapearedBalls.Count; x++)
			{
				disapearedBalls[x].cell.BallHere = false;
				balls.Remove(disapearedBalls[x]);
			}
		}
		/// <summary>
		/// Определение какие сообщения возникнут в 
		/// результате нажатия на ячейку поля пользователем
		/// </summary>
		/// <param name="cell">Ячейка</param>
		/// <returns>Результат нажатия на ячейку</returns>
		public UserMessage FieldClick(Cell cell)
		{
			UserMessage message = UserMessage.None;
			Ball ball = Algoritms.FindBall(cell,balls);
			//Если шарик не выделен, выделить
			if (ball!= null)
			{
				if (ball == selectedBall)
				{
					selectedBall = null;
					message = UserMessage.DeselectBall;
				}
				else
				{
					selectedBall = ball;
					message = UserMessage.Select_Ball;
				}
			}
			//Если место свободно и был выделен шарик
			if (ball==null&& selectedBall!=null)
			{
				//Создаем траекторию движения шарика
				Path path = Algoritms.FindBallPath(selectedBall.cell, cell, field.cells);
				if (path!=null)
				{
					ballPath = new Cell[path.path.Count];
					path.path.CopyTo(ballPath);
					//Переместить шарик на свободное место
					message = PlaceBall(cell);
					//Убрать выделение шарика
					selectedBall = null;
				}
				else
				{
					message = UserMessage.Invalid_Place_Ball;
				}
			}
			return message;
		}
	}
}
