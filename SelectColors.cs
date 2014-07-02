using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lines.Properties;

namespace Lines
{
	public partial class SelectColors : Form
	{
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;

		public SelectColors()
		{
			InitializeComponent();
			int ButtonHeight = 35;


			Size size = new Size();
			size.Width = Length_One_Ball * Count_Balls_In_Line;
			size.Height = Length_One_Ball * 4 + ButtonHeight;
			this.ClientSize = size;
			selectedColors = Settings.BallColors;
			InitBalls();
			bgc = BufferedGraphicsManager.Current;
			realGraphics = this.CreateGraphics();
			graphicsManager = bgc.Allocate(realGraphics, this.ClientRectangle);
			bufferGraphics = graphicsManager.Graphics;

			button1 = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();

			button1.Location = new System.Drawing.Point(0, Length_One_Ball * 4);
			button1.Size = new System.Drawing.Size(this.ClientSize.Width / 2, ButtonHeight);
			button1.Name = "button1";
			button1.TabIndex = 0;
			button1.Text = "Подтвердить";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			button2.Location = new Point(this.ClientSize.Width / 2, Length_One_Ball * 4);
			button2.Width =  this.ClientSize.Width / 2;
			button2.Height = ButtonHeight;
			button2.Name = "button2";
			button2.TabIndex = 1;
			button2.Text = "Отменить";
			button2.UseVisualStyleBackColor = true;
			button2.Click += new System.EventHandler(this.button2_Click);
			Controls.Add(button1);
			Controls.Add(button2);
			this.ResumeLayout(false);

		}

		const int Length_One_Ball = 100;
		const int Count_Balls_In_Line = 5;
		BallForSelect[] balls = new BallForSelect[Settings.CountAllColors];
		int[] selectedColors;

		#region Переменные графики
		BufferedGraphics graphicsManager;
		BufferedGraphicsContext bgc;
		Graphics bufferGraphics;
		Graphics realGraphics;
		#endregion

		public int[] SelectedColors 
		{
			get
			{
				List<int> colors = new List<int>();
				for (int x=0; x< balls.Length;x++)
					if (balls[x].ballSelected)
						colors.Add(balls[x].ballColor);
				int[] arrayColors = new int[colors.Count];
				colors.CopyTo(arrayColors);
				if (arrayColors.Length==0)
					return null;
				return arrayColors;
			}
		}

		void SelectBalls(int[] colors)
		{
			if (colors==null)
				return;
			for (int y=0; y<colors.Length ;y++)
			for (int x=0; x< balls.Length;x++)
				if (colors[y] == balls[x].ballColor)
					balls[x].ballSelected = true;
		}

		BallForSelect FindBallForSelect(int x, int y)
		{
			for (int z = 0; z < balls.Length; z++)
			{
				if (balls[z].ballRectangle.Contains(x, y))
				{
					return balls[z];
				}
			}
			return null;
		}

		void DrawFrames(Graphics graphics)
		{
			Image frame = Resources.Selection;
			for (int x=0; x< balls.Length;x++)
			{
				if (balls[x].ballSelected)
				{
					graphics.DrawImage(frame, balls[x].ballRectangle);
				}
			}
		}


		void InitBalls()
		{
			int ballCounter = 0;
			for (int x = 0; x < balls.Length; x++)
			{
				balls[x] = new BallForSelect();
			}

			for (int y = 0; y < Settings.CountAllColors / Count_Balls_In_Line; y++)
			{
				for (int x = 0; x < Count_Balls_In_Line; x++)
				{
					Rectangle rec = new Rectangle(x * Length_One_Ball, y * Length_One_Ball, Length_One_Ball, Length_One_Ball);
					Image ballImg = Algoritms.GetBallImage(y * Count_Balls_In_Line + x + 1);
					balls[ballCounter].ballColor = y * Count_Balls_In_Line + x + 1;
					balls[ballCounter].ballRectangle = rec;
					ballCounter++;
				}
			}
			SelectBalls(selectedColors);
		}

		void DrawBalls(Graphics graphics)
		{
			for (int y = 0; y < Settings.CountAllColors; y++)
			{
				graphics.DrawImage(Algoritms.GetBallImage(balls[y].ballColor), balls[y].ballRectangle);
			}
		}

		void DrawBackground(Graphics graphics)
		{
			graphics.FillRectangle(Brushes.Black, this.ClientRectangle);
		}

		void RefleshAll()
		{
			DrawBackground(bufferGraphics);
			DrawBalls(bufferGraphics);
			DrawFrames(bufferGraphics);
			graphicsManager.Render();
		}

		private void SelectColors_Paint(object sender, PaintEventArgs e)
		{
			RefleshAll();
		}

		private void SelectColors_MouseClick(object sender, MouseEventArgs e)
		{
			BallForSelect ball = FindBallForSelect(e.X, e.Y);
			if (ball == null)
				return;
			if (ball.ballSelected)
				ball.ballSelected = false;
			else
				ball.ballSelected = true;
			RefleshAll();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			balls = new BallForSelect[0];
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Settings.BallColors = SelectedColors;
			this.Close();
		}
	}

	class BallForSelect
	{
		public Rectangle ballRectangle;
		public bool ballSelected = false;
		public int ballColor = 0;
		public BallForSelect() { }
	}

}
