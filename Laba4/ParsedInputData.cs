﻿using Laba4;
using System;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace Laba4
{
    public class Figure : List<Line> { }
    public class LayerFigures : List<Figure> { }

    public struct ParsedInputData
    {
        public SFML.Graphics.Color background = SFML.Graphics.Color.Black;
        public List<LayerFigures> layers = new List<LayerFigures>();

        public ParsedInputData()
        {

        }
    }
}

