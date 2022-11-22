using Laba4;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Laba4
{
    public struct Line
    {
        public float A { get; } = 0;
        public float B { get; } = 0;
        public float C { get; } = 0;

        public Line(float A, float B, float C)
        {
            this.A = A;
            this.B = B;
            this.C = C;
        }
    }
}