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
        static ParsedInputData data;

        static void Main(string[] args)
        {
            renderWindow.SetVerticalSyncEnabled(true);
            renderWindow.Closed += (sender, args) => renderWindow.Close();

            IDrawingTool drawingTool = new SFMLDrawingTool();

            DataInput input = new DataInput("input.txt");
            data = input.ParseInputFile();

            List<Color[,]> pixelLayers = new List<Color[,]>();

            Drawer backgroundDrawer = new Drawer(width, height);
            backgroundDrawer.recursiveFill(new Vector2i(0, 0), data.background, Color.Black);
            pixelLayers.Add(backgroundDrawer.drawingLayer);

            foreach (var figuresLayer in data.layers)
            {
                Drawer drawer = new Drawer(width, height);
                foreach (var figure in figuresLayer)
                {
                    drawer.DrawFigure(figure, Color.Red, Color.Yellow);
                }
                pixelLayers.Add(drawer.drawingLayer);
            }

            var mergedLayer = MergePixelLayers(pixelLayers);

            Color[,] result = ReverseYPixelLayer(mergedLayer);

            Image image = new Image(result);
            Texture texture = new Texture(image);
            Sprite sprite = new Sprite(texture);

            while (renderWindow.IsOpen)
            {
                renderWindow.DispatchEvents();
                renderWindow.Clear(Color.Black);
                renderWindow.Draw(sprite);
                renderWindow.Display();
            }
        }

        private static Color[,] ReverseYPixelLayer(Color[,] mergedLayer)
        {
            Color[,] result = new Color[800, 800];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[i, height - j - 1] = mergedLayer[i, j];
                }
            }

            return result;
        }

        public static Color[,] MergePixelLayers(List<Color[,]> layers)
        {
            Color[,] baseLayer = new Color[height, width];

            for (int i = 1; i < layers.Count; i++)
            {
                Color[,] topLayer = layers[i];
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < width; k++)
                    {
                        Color currentPixel = topLayer[j, k];
                        if (currentPixel != Color.Transparent && baseLayer[j, k] != Color.Transparent)
                        {
                            baseLayer[j, k].R += (byte)(currentPixel.B / 5);
                            baseLayer[j, k].G += (byte)(currentPixel.G / 5);
                            baseLayer[j, k].B += (byte)(currentPixel.R / 5);
                            baseLayer[j, k].A = currentPixel.A;
                        }
                        else if (currentPixel != Color.Transparent && baseLayer[j, k] == Color.Transparent)
                        {
                            baseLayer[j, k] = currentPixel;
                        }

                        if (i > 0 && k % 100 == 0 && renderWindow.IsOpen && currentPixel != Color.Transparent )
                        {
                            Image image = new Image(ReverseYPixelLayer(baseLayer));
                            Texture texture = new Texture(image);
                            Sprite sprite = new Sprite(texture);

                            renderWindow.DispatchEvents();
                            renderWindow.Clear(Color.Black);
                            renderWindow.Draw(sprite);
                            renderWindow.Display();
                        }
                    }
                }
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (baseLayer[i, j] == Color.Transparent) baseLayer[i, j] = layers[0][i,j];
                }
            }
            return baseLayer;
        }
    }
}