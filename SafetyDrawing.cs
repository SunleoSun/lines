using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lines
{
	public class SafetyDrawing
	{
		Graphics graphics;
		BufferedGraphics graphicsManager;

		public SafetyDrawing(Graphics graphics, BufferedGraphics graphicsManager)
		{
			this.graphics = graphics;
			this.graphicsManager = graphicsManager;
		}

		public void DrawImage(Image img, Rectangle rec)
		{
			lock (this)
			{
				graphics.DrawImage(img, rec);
			}
		}

		public void Render()
		{
				graphicsManager.Render();
		}
	}
}
