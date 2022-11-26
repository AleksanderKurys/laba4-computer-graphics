using Laba4;

using Microsoft.VisualBasic;

using SFML.Graphics;
using SFML.System;


namespace Laba4
{
    class WindowsDrawingTool : IDrawingTool {

		Form form;
		Graphics gr;

		public WindowsDrawingTool(int width, int height) {
			form = new Form();
			form.Size = new Size(width, height);
			gr = form.CreateGraphics();
			form.Show();
		}

		public void DrawPixel(SFML.Graphics.Color color, Vector2i position) {
			Brush pen = new SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
			gr.FillRectangle(pen, position.X, position.Y, 1, 1);
		}
	}
}