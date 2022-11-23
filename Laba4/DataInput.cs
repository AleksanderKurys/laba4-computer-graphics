using Laba4;
using System;
using System.IO;
using System.Text.RegularExpressions;
using SFML.Graphics;
using SFML.System;

namespace Laba4
{
    public class DataInput
    {
        private string fileName;

        public DataInput(string fileName)
        {
            this.fileName = fileName;
        }

        public ParsedInputData ParseInputFile()
        {
            string[] data = File.ReadAllLines(fileName);

            ParsedInputData parsedInputData = new ParsedInputData();
            parsedInputData.background = (Color)getBackgroundColorFromString(data[0]);
            int layersNumber = (int)getLayersNumberFromString(data[1]);

            int i = 3;

            for (int layerIndex = 0; layerIndex < layersNumber; layerIndex++)
            {
                // data[i] - current string from input file
                LayerFigures layer = new LayerFigures();

                while (getLayerIndexFromString(data[i]) == null) i++;

                if (getLayerIndexFromString(data[i]) != null)
                {
                    while (getFiguresNumberFromString(data[i]) == null) i++;
                    int figuresNumber = (int)getFiguresNumberFromString(data[i]);

                    for (int figureIndex = 0; figureIndex < figuresNumber; figureIndex++)
                    {
                        while (getFigureSidesFromString(data[i]) == null) i++;
                        int figureSides = (int)getFigureSidesFromString(data[i]);

                        Figure figure = new Figure();

                        for (int sideIndex = 0; sideIndex < figureSides; sideIndex++)
                        {
                            while (getLineFromString(data[i]) == null) i++;

                            Line line = (Line)getLineFromString(data[i]);
                            figure.Add(line);
                            i++;
                        }

                        layer.Add(figure);
                    }
                }

                parsedInputData.layers.Add(layer);
            }

            return parsedInputData;
        }

        private Color? getBackgroundColorFromString(string str)
        {
            Regex regex = new Regex("^background: R = (.*?), G = (.*?), B = (.*?), A = (.*?)$");
            Match match = regex.Match(str);

            try
            {
                byte red = byte.Parse(match.Groups[1].Value);    // Red
                byte green = byte.Parse(match.Groups[2].Value);    // Green
                byte blue = byte.Parse(match.Groups[3].Value);    // Blue
                byte alpha = byte.Parse(match.Groups[4].Value);    // Alpha
                return new Color(red, green, blue, alpha);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private int? getLayersNumberFromString(string str)
        {
            Regex regex = new Regex("^layersNumber: (.*?)$");
            Match match = regex.Match(str);

            try
            {
                int layersNumber = int.Parse(match.Groups[1].Value);    // layersNumber
                return layersNumber;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private int? getLayerIndexFromString(string str)
        {
            Regex regex = new Regex("^layerIndex: (.*?)$");
            Match match = regex.Match(str);

            try
            {
                int layerIndex = int.Parse(match.Groups[1].Value);    // layerIndex
                return layerIndex;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private int? getFiguresNumberFromString(string str)
        {
            Regex regex = new Regex("^figuresNumber: (.*?)$");
            Match match = regex.Match(str);

            try
            {
                int figuresNumber = int.Parse(match.Groups[1].Value);    // figuresNumber
                return figuresNumber;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private int? getFigureSidesFromString(string str)
        {
            Regex regex = new Regex("^figureSides: (.*?)$");
            Match match = regex.Match(str);

            try
            {
                int figureSides = int.Parse(match.Groups[1].Value);    // figureSides
                return figureSides;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Line? getLineFromString(string str)
        {
            Regex regex = new Regex("^line: A = (.*?), B = (.*?), C = (.*?)$");
            Match match = regex.Match(str);

            try
            {
                float A = float.Parse(match.Groups[1].Value);    // A
                float B = float.Parse(match.Groups[2].Value);    // B
                float C = float.Parse(match.Groups[3].Value);    // C
                return new Line(A, B, C);        // TODO

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

