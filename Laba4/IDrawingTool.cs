using Laba4;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Laba4
{
    public interface IDrawingTool
    {
        public void DrawPixel(SFML.Graphics.Color color, Vector2i position);
    }
}

