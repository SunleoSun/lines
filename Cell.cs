using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lines
{
	/// <summary>
	/// Класс, представляющий ячейку игрового поля
	/// </summary>
	[Serializable]
	public class Cell
	{
		int x;
		/// <summary> Координата X ячейки </summary>
		public int X
		{
			get { return x; }
			set
			{
				//Координата ячейки не должна выходить за пределы поля
				if (value > Settings.LengthField || value < 1)
				{
					return;
				}
				else
					x = value;
			}
		}

		int y;
		/// <summary> Координата Y ячейки </summary>
		public int Y
		{
			get { return y; }
			set
			{
				//Координата ячейки не должна выходить за пределы поля
				if (value > Settings.LengthField || value < 1)
				{
					return;
				}
				else
					y = value;
			}
		}

		/// <summary> Шарик находится в этой ячейке </summary>
		public bool BallHere { get; set; }

		///<summary>Индекс текущей ячейки в общем массиве ячеек поля</summary>
		public int Index { get; set; }

		//Конструкторы
		public Cell() { }

		public Cell(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}
