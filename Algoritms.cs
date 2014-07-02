using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lines.Properties;
using System.Drawing;

namespace Lines
{
	public class Path
	{
		public int PathLength { get { return path.Count; } }
		public List<Cell> path = new List<Cell>();
		public Cell startCell;
		public Cell endCell;
	}
	public static class Algoritms
	{
		/// <summary>
		/// Найти ячейку по координатам
		/// </summary>
		/// <param name="x">Координата Х</param>
		/// <param name="y">Координата Х</param>
		/// <param name="allCells">Массив всех ячеек</param>
		/// <returns>Найденная ячейка или null</returns>
		public static Cell FindCell(int x, int y, Cell[] allCells)
		{
			if (allCells != null)
				return allCells.SingleOrDefault(cell => cell.X == x && cell.Y == y);
			return null;
		}
		/// <summary>
		/// Найти шарик на месте ячейки
		/// </summary>
		/// <param name="cell">Ячейка</param>
		/// <param name="allBalls">Список шариков</param>
		/// <returns>Найденный шарик или null</returns>
		public static Ball FindBall(Cell cell, List<Ball> allBalls)
		{
			if (allBalls != null && cell!=null)
				return allBalls.SingleOrDefault(ball => ball.cell == cell);
			return null;
		}

		/// <summary>
		/// Найти шарик
		/// </summary>
		/// <param name="place">1 - вверху слева, 2 - вверху, 3 - вверху справа,
		/// 4 - справа, 5 - внизу справа, 6 - внизу, 7 - внизу слева, 8 - слева</param>
		/// <param name="curBall">Текущий шарик</param>
		/// <param name="allCells">Все ячейки поля</param>
		/// <param name="allBalls">Все шарики</param>
		/// <returns>Найденный шарик или null</returns>
		public static Ball FindBall(int place, Ball curBall, Cell[] allCells, List<Ball> allBalls)
		{
			switch (place)
			{
				case 1:
					//Если не выходит за пределы поля
					if (curBall.cell.Index - Settings.LengthField - 1 >= 0 && curBall.cell.Index % Settings.LengthField!=0)
					{
						return FindBall(allCells[curBall.cell.Index - Settings.LengthField - 1], allBalls);
					}
				break;
				case 2:
					//Если не выходит за пределы поля
					if (curBall.cell.Index - Settings.LengthField >= 0)
					{
						return FindBall(allCells[curBall.cell.Index - Settings.LengthField], allBalls);
					}
				break;
				case 3:
					//Если не выходит за пределы поля
				if (curBall.cell.Index - Settings.LengthField + 1 >= 0 && curBall.cell.Index % Settings.LengthField != Settings.LengthField-1)
					{
						return FindBall(allCells[curBall.cell.Index - Settings.LengthField+1], allBalls);
					}
				break;
				case 4:
					//Если не выходит за пределы поля
				if (curBall.cell.Index + 1 < Settings.LengthField * Settings.LengthField && curBall.cell.Index % Settings.LengthField != Settings.LengthField - 1)
					{
						return FindBall(allCells[curBall.cell.Index + 1], allBalls);
					}
				break;
				case 5:
					//Если не выходит за пределы поля
				if (curBall.cell.Index + Settings.LengthField + 1 < Settings.LengthField * Settings.LengthField && curBall.cell.Index % Settings.LengthField != Settings.LengthField - 1)
					{
						return FindBall(allCells[curBall.cell.Index +Settings.LengthField+ 1], allBalls);
					}
				break;
				case 6:
					//Если не выходит за пределы поля
					if (curBall.cell.Index + Settings.LengthField < Settings.LengthField * Settings.LengthField)
					{
						return FindBall(allCells[curBall.cell.Index + Settings.LengthField], allBalls);
					}
				break;
				case 7:
					//Если не выходит за пределы поля
				if (curBall.cell.Index + Settings.LengthField - 1 < Settings.LengthField * Settings.LengthField && curBall.cell.Index % Settings.LengthField != 0)
					{
						return FindBall(allCells[curBall.cell.Index + Settings.LengthField-1], allBalls);
					}
				break;
				case 8:
					//Если не выходит за пределы поля
				if (curBall.cell.Index - 1 >= 0 && curBall.cell.Index % Settings.LengthField != 0)
					{
						return FindBall(allCells[curBall.cell.Index - 1], allBalls);
					}
				break;
			}
			return null;
		}

		/// <summary>
		/// Получить картинку шарика
		/// </summary>
		/// <param name="ball">Шарик</param>
		/// <returns>Картинка шарика</returns>
		public static Image GetBallImage(Ball ball)
		{
			Image img = null;
			switch (ball.color)
			{
				case 1: img = Resources._1; break;
				case 2: img = Resources._2; break;
				case 3: img = Resources._3; break;
				case 4: img = Resources._4; break;
				case 5: img = Resources._5; break;
				case 6: img = Resources._6; break;
				case 7: img = Resources._7; break;
				case 8: img = Resources._8; break;
				case 9: img = Resources._9; break;
				case 10: img = Resources._10; break;
				case 11: img = Resources._11; break;
				case 12: img = Resources._12; break;
				case 13: img = Resources._13; break;
				case 14: img = Resources._14; break;
				case 15: img = Resources._15; break;
				case 16: img = Resources._16; break;
				case 17: img = Resources._17; break;
				case 18: img = Resources._18; break;
				case 19: img = Resources._19; break;
				case 20: img = Resources._20; break;
			}
			return img;
		}
		public static Image GetBallImage(int color)
		{
			Image img = null;
			switch (color)
			{
				case 1: img = Resources._1; break;
				case 2: img = Resources._2; break;
				case 3: img = Resources._3; break;
				case 4: img = Resources._4; break;
				case 5: img = Resources._5; break;
				case 6: img = Resources._6; break;
				case 7: img = Resources._7; break;
				case 8: img = Resources._8; break;
				case 9: img = Resources._9; break;
				case 10: img = Resources._10; break;
				case 11: img = Resources._11; break;
				case 12: img = Resources._12; break;
				case 13: img = Resources._13; break;
				case 14: img = Resources._14; break;
				case 15: img = Resources._15; break;
				case 16: img = Resources._16; break;
				case 17: img = Resources._17; break;
				case 18: img = Resources._18; break;
				case 19: img = Resources._19; break;
				case 20: img = Resources._20; break;
			}
			return img;
		}

		public static Path FindBallPath(Cell startCell, Cell endCell, Cell[] allCells)
		{
			List<Path> paths = new List<Path>();
			int curPathsLength = 1;
			Path startPath = new Path();
			startPath.startCell = startCell;
			startPath.endCell = startCell;
			startPath.path.Add(startCell);
			paths.Add(startPath);
			//FindPathsAround(startPath, allCells, paths);
			while (true)
			{
				//Найти все пути текущей длины
				List<Path> findedPaths = FindPaths(curPathsLength, paths);
				//Если нет путей с текущей длиной, то записать путь с конечной ячейкой
				if (findedPaths.Count == 0)
				{
					return FindPath(endCell, paths);
				}
				for (int x = 0; x < findedPaths.Count; x++)
				{
					//Найти все пути вокруг последней ячейки текущего пути
					FindPathsAround(findedPaths[x], allCells, paths);
				}
				curPathsLength++;
			}
		}

		static void FindPathsAround(Path curPath, Cell[] allCells, List<Path> allPath)
		{
			//Если индекс ячейки слева не выходит за пределы поля
			if ((curPath.endCell.Index + 1) % Settings.LengthField != 1)
				CalcNearPath(curPath, allCells[curPath.endCell.Index - 1], allPath);
			//Если индекс ячейки справа не выходит за пределы поля
			if ((curPath.endCell.Index + 1) % Settings.LengthField != 0)
				CalcNearPath(curPath, allCells[curPath.endCell.Index + 1], allPath);
			//Если индекс ячейки сверху не выходит за пределы поля
			if (curPath.endCell.Index - Settings.LengthField >= 0)
				CalcNearPath(curPath, allCells[curPath.endCell.Index - Settings.LengthField], allPath);
			//Если индекс ячейки снизу не выходит за пределы поля
			if (curPath.endCell.Index + Settings.LengthField < Settings.LengthField * Settings.LengthField)
				CalcNearPath(curPath, allCells[curPath.endCell.Index + Settings.LengthField], allPath);
		}

		static void CalcNearPath(Path curPath, Cell nextPathCell, List<Path> allPath)
		{
			Path newPath = new Path();
			if (!nextPathCell.BallHere)
			{
				newPath.endCell = nextPathCell;
				newPath.startCell = curPath.startCell;
				Cell[] cells = new Cell[curPath.path.Count];
				curPath.path.CopyTo(cells);
				newPath.path = new List<Cell>(cells);
				newPath.path.Add(nextPathCell);
				Path path = FindPath(nextPathCell, allPath);
				if (path != null)
				{
					if (path.PathLength > newPath.PathLength)
						path = newPath;
				}
				else
					allPath.Add(newPath);
			}
		}

		static List<Path> FindPaths(int pathsLengths, List<Path> allPaths)
		{
			List<Path> paths = new List<Path>();
			for (int x=0; x< allPaths.Count;x++)
			{
				if (allPaths[x].PathLength == pathsLengths)
				{
					paths.Add(allPaths[x]);
				}
			}
			return paths;
		}

		static Path FindPath(Cell endCell, List<Path> allPaths)
		{
			if (allPaths != null && endCell!=null)
				return allPaths.SingleOrDefault(path => path.endCell == endCell);
			return null;
		}

		/// <summary>
		/// Уберает все шарики в линиях одинакового цвета
		/// </summary>
		/// <param name="startBall">Шарик, с которого ведется поиск других подобных шариков</param>
		/// <param name="allCells">Все ячейки поля</param>
		/// <param name="allBalls">Все шарики на поле</param>
		/// <param name="disapearedBalls">Список удаляемых шариков</param>
		public static void BallDisapeared(Ball startBall, Cell[] allCells, List<Ball>allBalls, List<Ball> disapearedBalls)
		{
			List<Ball> tempDisapearedBalls = new List<Ball>();

			tempDisapearedBalls.Add(startBall);
			//Смотрим по диагронали слева сверху вниз справа
			FindDirectrlyBall(1, startBall, allCells, allBalls, ref tempDisapearedBalls);
			FindDirectrlyBall(5, startBall, allCells, allBalls, ref tempDisapearedBalls);
			//Если в линии шариков больше или столько же сколько нужно для их
			//исчезновения - то добавляем шарики в исчезаемые
			if (tempDisapearedBalls.Count >= Settings.CountLengthLines)
				disapearedBalls.AddRange(tempDisapearedBalls);
			tempDisapearedBalls.Clear();

			tempDisapearedBalls.Add(startBall);
			//Смотрим вертикально шарики
			FindDirectrlyBall(2, startBall, allCells, allBalls, ref tempDisapearedBalls);
			FindDirectrlyBall(6, startBall, allCells, allBalls, ref tempDisapearedBalls);
			//Если в линии шариков больше или столько же сколько нужно для их
			//исчезновения - то добавляем шарики в исчезаемые
			if (tempDisapearedBalls.Count >= Settings.CountLengthLines)
				disapearedBalls.AddRange(tempDisapearedBalls);
			tempDisapearedBalls.Clear();

			tempDisapearedBalls.Add(startBall);
			//Смотрим диагональ сверху справа до снизу слева
			FindDirectrlyBall(3, startBall, allCells, allBalls, ref tempDisapearedBalls);
			FindDirectrlyBall(7, startBall, allCells, allBalls, ref tempDisapearedBalls);
			//Если в линии шариков больше или столько же сколько нужно для их
			//исчезновения - то добавляем шарики в исчезаемые
			if (tempDisapearedBalls.Count >= Settings.CountLengthLines)
				disapearedBalls.AddRange(tempDisapearedBalls);
			tempDisapearedBalls.Clear();

			tempDisapearedBalls.Add(startBall);
			//Смотрим горизонтально шарики
			FindDirectrlyBall(4, startBall, allCells, allBalls, ref tempDisapearedBalls);
			FindDirectrlyBall(8, startBall, allCells, allBalls, ref tempDisapearedBalls);
			//Если в линии шариков больше или столько же сколько нужно для их
			//исчезновения - то добавляем шарики в исчезаемые
			if (tempDisapearedBalls.Count >= Settings.CountLengthLines)
				disapearedBalls.AddRange(tempDisapearedBalls);
		}

		/// <summary>
		/// Найти шарик, учитывая направление поиска
		/// </summary>
		/// <param name="direction">Направление: 1 - вверху слева, 2 - вверху, 3 - вверху справа,
		/// 4 - справа, 5 - внизу справа, 6 - внизу, 7 - внизу слева, 8 - слева</param>
		/// <param name="startBall">Шарик, относительно которого осуществляется просмотр</param>
		/// <param name="allCells">Все ячейки поля</param>
		/// <param name="allBalls">Все шарики</param>
		/// <param name="findedBalls">Список найденных шариков</param>
		private static void FindDirectrlyBall(int direction,Ball startBall, Cell[] allCells, List<Ball> allBalls,ref List<Ball> findedBalls)
		{
			Ball nextBall;
			//Находим первый шарик по указанному направлению
			nextBall = FindBall(direction, startBall, allCells, allBalls);
			//Если шарик найден, продолжаем искать шарики и добавлять в массив найденных
			if (nextBall != null)
			{
				for (int x = 0; x < Settings.LengthField * Settings.LengthField; x++)
				{
					if (nextBall != null)
						if (nextBall.color == startBall.color)
						{
							findedBalls.Add(nextBall);
							nextBall = FindBall(direction, nextBall, allCells, allBalls);
						}
						else
							break;
					else
						break;
				}
			}
		}

		
	}
}
