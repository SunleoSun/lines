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
	public partial class SettingsForm : Form
	{
		SelectColors selectColors;

		public SettingsForm()
		{
			InitializeComponent();

			fnudPlaceSpeed.Minimum = 1;
			fnudPlaceSpeed.Maximum = 50;
			fnudBallLength.Minimum = 20;
			fnudBallLength.Maximum = 300;
			fnudCountBallsColors.Minimum = 1;
			fnudCountBallsColors.Maximum = Settings.CountAllColors;
			fnudLinesLength.Minimum = 2;
			fnudCountNextBalls.Minimum = 1;
			fnudFieldLength.Minimum = 3;
			fnudFieldLength.Maximum = 40;
			fnudSpeedAnimation.Minimum = 1;
			fnudSpeedAnimation.Maximum = 20;
			fnudSpeedAnimationNextBalls.Minimum = 1;
			fnudSpeedAnimationNextBalls.Maximum = 20;
			fnudSpeedAppearDisappear.Minimum = 1;
			fnudSpeedAppearDisappear.Maximum = 20;
			fnudSpeedAppearDisappearNextBalls.Minimum = 1;
			fnudSpeedAppearDisappearNextBalls.Maximum = 20;
			if (Settings.BallColors!=null)
				fnudCountBallsColors.Enabled = false;
			else
				fnudCountBallsColors.Enabled = true;
			CalcFnudMaxMinValues();


			fnudCountBallsColors.Value = Settings.CountBallColors;
			fnudBallLength.Value = Settings.DrawOneCellLength;
			fnudFieldLength.Value = Settings.LengthField;
			fnudLinesLength.Value = Settings.CountLengthLines;
			fnudCountNextBalls.Value = Settings.CountNextBalls;
			fcbSaveGame.Checked = Settings.SavedGame;
			fcbSaveRecords.Checked = Settings.SaveScore;
			fnudSpeedAnimation.Value = Settings.SpeedBallAnimation;
			fnudPlaceSpeed.Value = Settings.SpeedBallPlacement;
			fnudSpeedAppearDisappear.Value = Settings.SpeedBallsAppearDisapear;
			fnudSpeedAnimationNextBalls.Value = Settings.SpeedNextBallAnimation;
			fnudSpeedAppearDisappearNextBalls.Value = Settings.SpeedNextBallsAppearDisapear;

		}

		private void CalcFnudMaxMinValues()
		{
			fnudCountNextBalls.Maximum = fnudFieldLength.Value;
			fnudLinesLength.Maximum = fnudFieldLength.Value;
		}

		private void fbSaveAndExit_Click(object sender, EventArgs e)
		{
			if(Settings.BallColors!=null)
				Settings.CountBallColors = Settings.BallColors.Length;
			else
				Settings.CountBallColors = (int)fnudCountBallsColors.Value;
			Settings.CountLengthLines = (int)fnudLinesLength.Value;
			Settings.CountNextBalls = (int)fnudCountNextBalls.Value;
			Settings.DrawOneCellLength = (int)fnudBallLength.Value;
			Settings.LengthField = (int)fnudFieldLength.Value;
			Settings.SavedGame = fcbSaveGame.Checked;
			Settings.SaveScore = fcbSaveRecords.Checked;
			Settings.SpeedBallAnimation = (int)fnudSpeedAnimation.Value;
			Settings.SpeedBallPlacement = (int)fnudPlaceSpeed.Value;
			Settings.SpeedBallsAppearDisapear = (int)fnudSpeedAppearDisappear.Value;
			Settings.SpeedNextBallAnimation = (int)fnudSpeedAnimationNextBalls.Value;
			Settings.SpeedNextBallsAppearDisapear = (int)fnudSpeedAppearDisappearNextBalls.Value;
			Close();
		}

		private void fbExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void fnudFieldLength_ValueChanged(object sender, EventArgs e)
		{
			CalcFnudMaxMinValues();
		}

		private void fbResetSettings_Click(object sender, EventArgs e)
		{
			Settings.BallColors = null;
			fnudCountBallsColors.Enabled = true;
			fnudCountBallsColors.Value = 5;
			fnudLinesLength.Value=5;
			fnudFieldLength.Value = 10;
			fnudCountNextBalls.Value=3;
			fnudBallLength.Value = 50;
			fcbSaveGame.Checked=true;
			fcbSaveRecords.Checked=true;
			fnudPlaceSpeed.Value=15;
			fnudSpeedAnimation.Value = 1;
			fnudSpeedAppearDisappear.Value=1;
			fnudSpeedAnimationNextBalls.Value=1;
			fnudSpeedAppearDisappearNextBalls.Value=2;
		}

		private void fbSetColors_Click(object sender, EventArgs e)
		{
			selectColors = new SelectColors();
			selectColors.ShowDialog();
			Settings.BallColors = selectColors.SelectedColors;

			if (selectColors.SelectedColors!=null)
			{
				fnudCountBallsColors.Enabled = false;
			}
			if (Settings.BallColors != null)
				fnudCountBallsColors.Enabled = false;
			else
				fnudCountBallsColors.Enabled = true;
		}

		private void fnudCountNextBalls_ValueChanged(object sender, EventArgs e)
		{
			CalcFnudMaxMinValues();
		}

	}
}
