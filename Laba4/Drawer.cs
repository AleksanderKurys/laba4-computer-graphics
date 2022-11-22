using Laba4;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Laba4
{
    public class Drawer
    {
        private uint drawingAreaWidth;
        private uint drawingAreaHeight;
        public Color[,] drawingLayer { get; private set; }

        public Drawer(uint drawingAreaWidth, uint drawingAreaHeight)
        {
            this.drawingAreaWidth = drawingAreaWidth;
            this.drawingAreaHeight = drawingAreaHeight;

            drawingLayer = new Color[drawingAreaHeight, drawingAreaWidth];
        }

        public void ddaLine(Vector2f start, Vector2f end, Color color)
        {
            Vector2i startRounded = new Vector2i((int)Math.Ceiling(start.X), (int)Math.Ceiling(start.Y));
            Vector2i endRounded = new Vector2i((int)Math.Ceiling(end.X), (int)Math.Ceiling(end.Y));
            int L = Math.Max(Math.Abs(endRounded.X - startRounded.X), Math.Abs(endRounded.Y - startRounded.Y));
            float dx = (end.X - start.X) / L, dy = (end.Y - start.Y) / L;
            float x = start.X, y = start.Y;
            for (int i = 0; i <= L; i++)
            {
                int roundedX = (int)Math.Ceiling(x);
                int roundedY = (int)Math.Ceiling(y);

                if (roundedX >= drawingAreaWidth || roundedX < 0
                    || roundedY >= drawingAreaHeight || roundedY < 0)
                {
                    x += dx;
                    y += dy;
                    continue;
                }
                drawingLayer[roundedX, roundedY] = color;
                x += dx;
                y += dy;
            }
        }

        public void recursiveFill(Vector2i startPoint, Color fillColor, Color borderColor)
        {
            Vector2f floatStartPoint = new Vector2f(startPoint.X, startPoint.Y);
            Stack<Vector2i> points = new Stack<Vector2i>();
            points.Push(startPoint);
            while (points.Count != 0)
            {
                var point = points.Pop();

                if (point.X >= drawingAreaWidth || point.X < 0)
                {
                    continue;
                }
                if (point.Y >= drawingAreaHeight || point.Y < 0)
                {   
                    continue;
                }
                if (drawingLayer[point.X, point.Y] == borderColor || drawingLayer[point.X, point.Y] == fillColor)
                {
                    continue;
                }

                drawingLayer[point.X, point.Y] = fillColor;
                points.Push(new Vector2i(point.X - 1, point.Y));
                points.Push(new Vector2i(point.X + 1, point.Y));
                points.Push(new Vector2i(point.X, point.Y + 1));
                points.Push(new Vector2i(point.X, point.Y - 1));
            }
        }

        public void DrawFigure(Figure figure, Color borderColor, Color? fillColor = null)
        {
            List<Vector2f> vertexes = DrawingUtils.GetFigureVertexes(figure);

            int vertexesNumber = vertexes.Count;

            Vector2f currentVertex = vertexes[0];

            ddaLine(vertexes[0], vertexes[vertexesNumber - 1], borderColor);

            for (int i = 1; i < vertexesNumber; i++)
            {
                ddaLine(currentVertex, vertexes[i], borderColor);
                currentVertex = vertexes[i];
            }

            Vector2f? vertexInsideWindow = GetPointInsideWindow(figure);

            if (vertexInsideWindow != null)
            {
                Vector2f pointInsideFigure = DrawingUtils.GetPointInsideFigure(figure);

                while (!CheckPointInsideWindow(pointInsideFigure))
                {
                    pointInsideFigure = DrawingUtils.GetLineMiddle(pointInsideFigure, (Vector2f)vertexInsideWindow);
                }

                Vector2i fillStartPoint = new Vector2i((int)Math.Round(pointInsideFigure.X), (int)Math.Round(pointInsideFigure.Y));

                if (fillColor != null) recursiveFill(fillStartPoint, (Color)fillColor, borderColor);
            }

            DrawTriangulationLines(figure, borderColor);
        }

        public void DrawTriangulationLines(Figure figure, Color borderColor)
        {
            var vertexes = DrawingUtils.GetFigureVertexes(figure);

            for (int i = 2; i <= vertexes.Count - 2; i++)
            {
                ddaLine(vertexes[0], vertexes[i], borderColor);
            }
        }

        public bool CheckPointInsideWindow(Vector2f point)
        {
            if (point.X >= 0 && point.X <= drawingAreaWidth && point.Y >= 0 && point.Y <= drawingAreaHeight)
            {
                return true;
            }

            return false;
        }

        public Vector2f? GetPointInsideWindow(Figure figure)
        {
            var vertexes = DrawingUtils.GetFigureVertexes(figure);

            foreach (var vertex in vertexes)
            {
                if (CheckPointInsideWindow(vertex)) return vertex;
            }

            return null;
        }
    }
}