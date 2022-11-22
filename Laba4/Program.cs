using Laba4;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace Laba4
{
    public class Program
    {
        public static uint width = 800;
        public static uint height = 800;

        static RenderWindow renderWindow = new RenderWindow(new SFML.Window.VideoMode(width, height), "Laba 4");

        static void Main(string[] args)
        {
            IDrawingTool drawingTool = new SFMLDrawingTool();

            DataInput input = new DataInput("input.txt");
            ParsedInputData data = input.ParseInputFile();

            List<Color[,]> pixelLayers = new List<Color[,]>();

            foreach (var figuresLayer in data.layers)
            {
                Drawer drawer = new Drawer(width, height);
                foreach (var figure in figuresLayer)
                {
                    drawer.DrawFigure(figure, Color.Red, Color.Yellow);
                }
                pixelLayers.Add(drawer.drawingLayer);
            }

            var mergedLayers = MergePixelLayers(pixelLayers);

            Color[,] result = new Color[800, 800];
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    result[i, height - j - 1] = mergedLayers[i, j];
                }
            }

            Image image = new Image(result);
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

        public static Color[,] MergePixelLayers(List<Color[,]> layers)
        {
            Color[,] baseLayer = layers[0];

            for (int i = 1; i < layers.Count; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < width; k++)
                    {
                        baseLayer[j, k] += layers[i][j, k];
                    }
                }
            }

            return baseLayer;
        }
    }
}