using Laba4;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace graphic_lab3
{
    internal class Program
    {
        static uint width = 800;
        static uint height = 800;

        static RenderWindow renderWindow = new RenderWindow(new SFML.Window.VideoMode(width, height), "Laba 4");

        static void Main(string[] args)
        {
            IDrawingTool drawingTool = new SFMLDrawingTool();

            DataInput input = new DataInput("input.txt");
            ParsedInputData data = input.ParseInputFile();

            Drawer drawer = new Drawer(width, height);

            foreach (var figure in data.layers[0])
            {
                drawer.DrawFigure(figure);
            }


            Image image = new Image(drawer.drawingLayer);
            Texture texture = new Texture(image);
            Sprite sprite = new Sprite(texture);


            renderWindow.SetVerticalSyncEnabled(true);
            renderWindow.Closed += (sender, args) => renderWindow.Close();
            while (renderWindow.IsOpen)
            {
                renderWindow.DispatchEvents();
                renderWindow.Clear(Color.Black);
                renderWindow.Draw(sprite);
                renderWindow.Display();
            }
        }
    }
}


//static void CreateTriangle(Vector2f[] points, Action<Vector2f, Vector2f, Color> drawLine, Action<Vector2i, Vector2f[], Color, Color> fillArea)
//{
//    if (points.Length != 3)
//    {
//        Console.WriteLine("Треугольник строится по 3 точкам");
//        return;
//    }
//    drawLine?.Invoke(points[0], points[1], Color.Red);
//    drawLine?.Invoke(points[2], points[1], Color.Red);
//    drawLine?.Invoke(points[2], points[0], Color.Red);
//    Vector2i center = (Vector2i)GetMediansIntersection(points);
//    fillArea?.Invoke(center, points, Color.Yellow, Color.Red);
//}

//static List<Vector2f[]> SplitTriangleWithMedians(Vector2f[] points)
//{
//    List<Vector2f[]> triangles = new List<Vector2f[]>();
//    Vector2f center = GetMediansIntersection(points);
//    Vector2f[] sideMiddles = {
//                GetLineMiddle(points[0], points[1]),
//                GetLineMiddle(points[1], points[2]),
//                GetLineMiddle(points[2], points[0])
//            };
//    Vector2f currentMiddle = sideMiddles[2];
//    for (int i = 0; i < points.Length; i++)
//    {
//        Vector2f[] triangle1 =
//        {
//                    center,
//                    points[i],
//                    currentMiddle
//                };
//        triangles.Add(triangle1);
//        currentMiddle = sideMiddles[i];
//        Vector2f[] triangle2 =
//        {
//                    center,
//                    points[i],
//                    currentMiddle
//                };
//        triangles.Add(triangle2);
//    }
//    return triangles;
//}

//// (-1; -1) - bad value
//static Vector2f getTrianglePointWithinWindow(Vector2f[] trianglePoints)
//{
//    Vector2f center = GetMediansIntersection(trianglePoints);
//    Vector2f pointInsideWindow = new Vector2f(-1, -1);
//    if (PointInsideWindow(trianglePoints[0]))
//    {
//        pointInsideWindow = trianglePoints[0];
//    }
//    else if (PointInsideWindow(trianglePoints[1]))
//    {
//        pointInsideWindow = trianglePoints[1];
//    }
//    else if (PointInsideWindow(trianglePoints[2]))
//    {
//        pointInsideWindow = trianglePoints[2];
//    }
//    while (!PointInsideWindow(center))
//    {
//        center = GetLineMiddle(center, pointInsideWindow);
//    }
//    return center;
//}

//static bool PointInsideWindow(Vector2f point)
//{
//    return point.X >= 0 && point.X <= width && point.Y >= 0 && point.Y <= height;
//}

//static Vector2f GetMediansIntersection(Vector2f[] points)
//{
//    if (points.Length != 3)
//    {
//        Console.WriteLine("Это не координаты вершин треугольника!");
//        return new Vector2f(0, 0);
//    }
//    float intesectionX = (points[0].X + points[1].X + points[2].X) / 3;
//    float intesectionY = (points[0].Y + points[1].Y + points[2].Y) / 3;
//    return new Vector2f(intesectionX, intesectionY);
//}



//private static void DrawLabTriangle(Vector2f[] pointsTriangleA)
//{
//    //CreateTriangle(pointsTriangleA, ddaLine, recursiveFill);
//}