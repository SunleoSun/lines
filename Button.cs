using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lines
{
	///<summary>Кнопка</summary>
	public class Button
	{
		///<summary>Прямоугольник кнопки</summary>
		Rectangle buttonRectangle;
		///<summary>Получить прямоугольник кнопки</summary>
		public Rectangle GetButtonRectangle	{	get { return buttonRectangle; }	}
		///<summary>Картинка в пассивном состоянии кнопки</summary>
		public Image passiveStateImage;
		///<summary>Картинка в активном состоянии кнопки</summary>
		public Image activeStateImage;
		///<summary>Получить или установить состояние кнопки</summary>
		public bool PushedState {get; set;}

		public Button(Rectangle rectangle)
		{
			this.buttonRectangle = rectangle;
			this.PushedState = false;
		}
		///<summary>Нарисовать кнопку</summary>
		public void DrawButton(SafetyDrawing safeDraw)
		{
			if (!PushedState)
			{
				safeDraw.DrawImage(passiveStateImage, buttonRectangle);
			}
			else
				safeDraw.DrawImage(activeStateImage, buttonRectangle);
		}
	}
}
