using Laba4;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Laba4
{
    class DrawingUtils
    {
        public static Vector2f GetIntersectionPoint(Line line1, Line line2)
        {
            float det = line1.A * line2.B - line2.A * line1.B; //A1B2 - A2B1
            float intersectionX = (line2.B * line1.C - line1.B * line2.C) / det;
            float intersectionY = (line1.A * line2.C - line2.A * line1.C) / det;
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

        public static Vector2f GetPointInsideFigure(Figure figure)
        {
            var vertexes = DrawingUtils.GetFigureVertexes(figure);

            Vector2f pointInsideFigure = new Vector2f(0, 0);
            foreach (var vertex in vertexes)
            {
                pointInsideFigure.X += vertex.X;
                pointInsideFigure.Y += vertex.Y;
            }

            pointInsideFigure.X /= vertexes.Count;
            pointInsideFigure.Y /= vertexes.Count;

            return pointInsideFigure;
        }

        public static Vector2f GetPointInsideFigure(List<Vector2f> vertexes)
        {
            Vector2f pointInsideFigure = new Vector2f(0, 0);
            foreach (var vertex in vertexes)
            {
                pointInsideFigure.X += vertex.X;
                pointInsideFigure.Y += vertex.Y;
            }

            pointInsideFigure.X /= vertexes.Count;
            pointInsideFigure.Y /= vertexes.Count;

            return pointInsideFigure;
        }
    }
}