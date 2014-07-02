using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lines
{
	[Serializable]
	public class Record
	{
		public int Score {get; set;}
		public int FieldLength {get; set;}
		public int CountBallPerTurn {get; set;}
		public int CountColorsPerGame { get; set; }
		public string Name {get; set;}
	}

	public class Records
	{
		List<Record> records = new List<Record>();

		public Record[] RecordsData { get { return records.ToArray(); } set { records = value.ToList(); } }

		public bool AddRecord(Record record)
		{
			List<Record> tmpRecords = new List<Record>(records);
			if (records.Count == 0)
			{
				records.Add(record);
				return true;
			}
			if (records.All(rec => rec.Score < record.Score))
			{
				if (records.Count < 6)
				{
					//Добавляем элемент в массив
					records.Add(record);
					//Сортируем по убыванию
					Sort();
				}
				else
				{
					int minScore = records.Min(rec => rec.Score);
					for (int x=0; x< records.Count;x++)
					{
						if (records[x].Score==minScore)
							records.Remove(records[x]);
					}
					//Добавляем элемент в массив
					records.Add(record);
					//Сортируем по убыванию
					Sort();
				}
				return true;
			}
			return false;
		}

		private void Sort()
		{
			records = records.OrderByDescending(rec => rec.Score).ToList();
		}
	}
}
