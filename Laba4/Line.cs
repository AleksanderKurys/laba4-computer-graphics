using Laba4;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Laba4
{
    public struct Line
    {
        public float k { get; } = 0;
        public float c { get; } = 0;

        public Line(float k, float c)
        {
            this.k = k;
            this.c = c;
        }
    }
}