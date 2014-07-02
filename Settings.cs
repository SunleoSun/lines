using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lines
{
	public static class Settings
	{
		/// <summary> Длина стороны поля </summary>
		public static int LengthField = 10;
		/// <summary> Количество разных цветов шариков пердставленных в игре</summary>
		public static int CountBallColors = 5;
		/// <summary> Максимальное количество разных цветов шариков представленных в ресурсах</summary>
		public static int CountAllColors = 20;
		/// <summary> Количество шариков в линии, для ее уничтожения </summary>
		public static int CountLengthLines = 5;
		/// <summary>Количество появлений шариков после каждого хода </summary>
		public static int CountNextBalls = 3;
		/// <summary>Максимальное количество итераций цикла </summary>
		public static int CycleMaxLength = 100000;
		///<summary>Длина обной ячейки</summary>
		public static int DrawOneCellLength = 50;
		///<summary>Размер отступа по ширене от края формы до поля</summary>
		public static int DrawIntendWidth = 10;
		///<summary>Размер отступа по высоте от края формы до поля следующих цветов</summary>
		public static int DrawIntendHeight = 10;
		///<summary>Соотношение ширины формы к ширине кнопки</summary>
		public static double ButtonToFormFactorWidth = 4;
		///<summary>Соотношение высоты формы к высоте кнопки</summary>
		public static double ButtonToFormFactorHeight = 8;
		///<summary>Скорость перемещения шарика</summary>
		public static int SpeedBallPlacement = 15;
		public static int SpeedBallAnimation = 1;
		public static int SpeedNextBallAnimation = 1;
		public static int AnimAmplitude = DrawOneCellLength / 4;
		public static int SpeedBallsAppearDisapear = 1;
		public static int SpeedNextBallsAppearDisapear = 2;
		public static bool SavedGame = true;
		public static bool SaveScore = true;
		public static int[] BallColors;
	}
}
