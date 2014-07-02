using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lines
{
	[Serializable]
	public class Field
	{
		/// <summary>
		/// Ячейки
		/// </summary>
		public Cell[] cells;

		public Field(int LengthField)
		{
			cells = new Cell[LengthField * LengthField];
			int index = 0;
			//Инициализация ячеек
			for (int y = 1; y < LengthField + 1; y++)
			{
				for (int x = 1; x < LengthField + 1; x++)
				{
					if (y == 1)
					{
						cells[x - 1] = new Cell(x, y);
						cells[x - 1].Index = index;
					}
					else
					{
						cells[y * LengthField - (LengthField - x) - 1] = new Cell(x, y);
						cells[y * LengthField - (LengthField - x) - 1].Index = index;
					}
					index++;
				}
			}
		}
	}
}
