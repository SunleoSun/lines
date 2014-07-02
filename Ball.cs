using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lines
{
	[Serializable]
	public class Ball
	{
		/// <summary>
		/// Ячейка шарика
		/// </summary>
		public Cell cell;
		/// <summary>
		/// Цвет шарика
		/// </summary>
		public int color;

		public Ball(Cell cell, int color)
		{
			this.cell = cell;
			this.color = color;
		}
	}
}
