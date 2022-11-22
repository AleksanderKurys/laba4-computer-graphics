using Laba4;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Laba4
{
    class DrawingUtils
    {
        public static Vector2f GetIntersectionPoint(Line lineA, Line lineB)
        {
            float intersectionX = (lineB.c - lineA.c) / (lineA.k - lineB.k);
            float intersectionY = lineA.k * intersectionX + lineA.c;
            return new Vector2f(intersectionX, intersectionY);
        }

        public static Vector2f GetLineMiddle(Vector2f start, Vector2f end)
        {
            float midX = (start.X + end.X) / 2;
            float midY = (start.Y + end.Y) / 2;
            return new Vector2f(midX, midY);
        }

        public static List<Vector2f> GetFigureVertexes(Figure figure)
        {
            // TODO: sort figure lines clockwise 

            List<Vector2f> vertexes = new List<Vector2f>();
            int linesNumber = figure.Count;

            Line currentLine = figure[0];
            vertexes.Add(GetIntersectionPoint(figure[0], figure[linesNumber - 1]));

            for(int i = 1; i < linesNumber; i++)
            {
                vertexes.Add(GetIntersectionPoint(currentLine, figure[i]));
                currentLine = figure[i];
            }

            return vertexes;
        }
    }
}