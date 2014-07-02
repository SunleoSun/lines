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
	public partial class NewRecord : Form
	{
		public NewRecord()
		{
			InitializeComponent();
		}

		public string GetName { get { return ftbName.Text; } }

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
