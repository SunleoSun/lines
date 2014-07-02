using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lines
{
	[Serializable()]
	public class DataContainer
	{
		/// <summary> Длина стороны поля </summary>
		public int LengthField = 10;
		/// <summary> Количество разных цветов шариков пердставленных в игре</summary>
		public int CountBallColors = 5;
		/// <summary> Количество шариков в линии, для ее уничтожения </summary>
		public int CountLengthLines = 5;
		/// <summary>Количество появлений шариков после каждого хода </summary>
		public int CountNextBalls = 3;
		///<summary>Длина обной ячейки</summary>
		public int DrawOneCellLength = 50;
		///<summary>Скорость перемещения шарика</summary>
		public int SpeedBallPlacement = 15;
		public int SpeedBallAnimation = 1;
		public int SpeedNextBallAnimation = 1;
		public int SpeedBallsAppearDisapear = 1;
		public int SpeedNextBallsAppearDisapear = 2;
		public bool SavedGame = true;
		public bool SaveScore = true;
		public int[] BallColors;
		public Record[] records;
		public EngineData engineData;
		public bool GameStarted;
	}
}
