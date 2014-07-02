using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
namespace Lines
{
	public static class Saver
	{
		static string settingsFilePath = "Settings.xml";
		public static void SaveData(DataContainer dc)
		{
			try
			{
				Stream stream = File.Open(settingsFilePath, FileMode.Create);
				SoapFormatter formatter = new SoapFormatter();
				formatter.Serialize(stream, dc);
				stream.Close();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public static DataContainer LoadData()
		{
			try
			{
				Stream stream = File.Open(settingsFilePath, FileMode.Open);
				SoapFormatter formatter = new SoapFormatter();
				DataContainer dc = (DataContainer)formatter.Deserialize(stream);
				stream.Close();
				return dc;
			}
			catch (System.Exception)
			{
				return null;
			}
		}
	}
}
