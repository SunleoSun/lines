using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lines
{
	public partial class RecordsWiev : Form
	{
		public RecordsWiev()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		public void ViewRecords(Records records)
		{
			flInfo.Text = "";
			for (int x=0; x< records.RecordsData.Length;x++)
			{
				Record curRecord = records.RecordsData[x];
				flInfo.Text += (x + 1).ToString() + "). " + "Имя: " + curRecord.Name + "\n" +
					"Счет: " + curRecord.Score.ToString() +
					"  Количество шариков за ход: " + curRecord.CountBallPerTurn.ToString() +
					"  Размер поля: " + curRecord.FieldLength.ToString() + "\n" +
					"Количество цветов в игре: " + curRecord.CountColorsPerGame.ToString() + "\n\n";
			}
		}
	}
}
