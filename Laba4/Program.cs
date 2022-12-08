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
        static Random random = new Random();

        static void Main(string[] args)
        {
            renderWindow.SetVerticalSyncEnabled(true);
            renderWindow.Closed += (sender, args) => renderWindow.Close();

            IDrawingTool drawingTool = new WindowsDrawingTool((int)width, (int)height);

            DataInput input = new DataInput("input.txt");
            data = input.ParseInputFile();

            List<SFML.Graphics.Color[,]> pixelLayers = new List<SFML.Graphics.Color[,]>();

            Drawer backgroundDrawer = new Drawer(width, height);
            backgroundDrawer.recursiveFill(new Vector2i(0, 0), data.background, SFML.Graphics.Color.Black);
            pixelLayers.Add(backgroundDrawer.drawingLayer);

/*            foreach (var figuresLayer in data.layers)
            {
                Drawer drawer = new Drawer(width, height);
                foreach (var figure in figuresLayer)
                {
                    drawer.DrawFigure(figure, randomColor(), randomColor());
                }
                pixelLayers.Add(drawer.drawingLayer);
            }*/

            Drawer drawer = new Drawer(width, height);
            pixelLayers.Add(drawer.drawingLayer);

            drawer.DrawFigure(new Clipper().ClipFigures(data.layers[1][0], data.layers[2][0]), randomColor(), randomColor());

            var mergedLayer = MergePixelLayers(pixelLayers);

            SFML.Graphics.Color[,] result = ReverseYPixelLayer(mergedLayer);

            for (int i = 0; i < result.GetLength(0); i++) {
                for (int j = 0; j < result.GetLength(1); j++) {
                    drawingTool.DrawPixel(result[i, j], new Vector2i(i, j));
                }
            }

			SFML.Graphics.Image image = new SFML.Graphics.Image(result);
			Texture texture = new Texture(image);
			Sprite sprite = new Sprite(texture);

			while (renderWindow.IsOpen)
			{
				renderWindow.DispatchEvents();
				renderWindow.Clear(SFML.Graphics.Color.Black);
				renderWindow.Draw(sprite);
				renderWindow.Display();
			}
		}

        private static SFML.Graphics.Color[,] ReverseYPixelLayer(SFML.Graphics.Color[,] mergedLayer)
        {
            SFML.Graphics.Color[,] result = new SFML.Graphics.Color[800, 800];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[i, height - j - 1] = mergedLayer[i, j];
                }
            }

            return result;
        }

        public static SFML.Graphics.Color[,] MergePixelLayers(List<SFML.Graphics.Color[,]> layers)
        {
            SFML.Graphics.Color[,] baseLayer = new SFML.Graphics.Color[height, width];

            for (int i = 1; i < layers.Count; i++)
            {
                SFML.Graphics.Color[,] topLayer = layers[i];
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < width; k++)
                    {
                        SFML.Graphics.Color currentPixel = topLayer[j, k];
                        if (currentPixel != SFML.Graphics.Color.Transparent && baseLayer[j, k] != SFML.Graphics.Color.Transparent)
                        {
                            if (true)
                            {
                                baseLayer[j, k].R = (byte)((currentPixel.R + baseLayer[j, k].R) % byte.MaxValue);
                                baseLayer[j, k].G = (byte)((currentPixel.G + baseLayer[j, k].G) % byte.MaxValue);
                                baseLayer[j, k].B = (byte)((currentPixel.B + baseLayer[j, k].B) % byte.MaxValue);
                                baseLayer[j, k].A = currentPixel.A;
                            }
                            else {
                                baseLayer[j, k] = currentPixel;
                            }
                        }
                        else if (currentPixel != SFML.Graphics.Color.Transparent && baseLayer[j, k] == SFML.Graphics.Color.Transparent)
                        {
                            baseLayer[j, k] = currentPixel;
                        }
                    }

                    SFML.Graphics.Image image = new SFML.Graphics.Image(ReverseYPixelLayer(baseLayer));
                    Texture texture = new Texture(image);
                    Sprite sprite = new Sprite(texture);

                    renderWindow.DispatchEvents();
                    renderWindow.Clear(SFML.Graphics.Color.Black);
                    renderWindow.Draw(sprite);
                    renderWindow.Display();
                }
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (baseLayer[i, j] == SFML.Graphics.Color.Transparent) baseLayer[i, j] = layers[0][i,j];
                }
            }
            return baseLayer;
        }

        private static SFML.Graphics.Color randomColor()
        {
            return new SFML.Graphics.Color((byte)random.Next(255),
                (byte)random.Next(255),
                (byte)random.Next(255),
                255);
        }

    }

}