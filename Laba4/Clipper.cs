using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Laba4
{
    public class Clipper
    {
        //Алгоритм отсечения Сазерленда-Ходгмана
        public List<Vector2f> ClipFigures(Figure subjectFigure, Figure clipFigure)
        {
            Vector2f cp1;
            Vector2f cp2;
            Vector2f s;
            Vector2f e;

            bool PointInside(Vector2f point)
            {
                return (cp2.X - cp1.X) * (point.Y - cp1.Y) > (cp2.Y - cp1.Y ) * (point.X - cp1.X);
            }

            Vector2f IntersectionPoint()
            {
                Vector2f dc = new Vector2f(cp1.X - cp2.X, cp1.Y - cp2.Y);
                Vector2f dp = new Vector2f(s.X - e.X, s.Y - e.Y);
                float n1 = cp1.X * cp2.Y - cp1.Y * cp2.X;
                float n2 = s.X * e.Y - s.Y * e.X;
                float n3 = (float)(1.0 / (dc.X * dp.Y - dc.Y * dp.X));

                return new Vector2f((n1 * dp.X - n2 * dc.X) * n3, (n1 * dp.Y - n2 * dc.Y) * n3);
            }

            var subjectPolygon = DrawingUtils.GetFigureVertexes(subjectFigure);
            var clipPolygon = DrawingUtils.GetFigureVertexes(subjectFigure);

            var outputPolygon = subjectPolygon;

            cp1 = clipPolygon.Last();
            foreach (var j in clipPolygon)
            {
                cp2 = j;
                var inputList = outputPolygon.GetRange(0, outputPolygon.Count);
                outputPolygon = new List<Vector2f>();
                
                s = inputList.Last(); //последняя точка

                foreach (var i in inputList)
                {
                    e = i;
                    if (PointInside(e))
                    {
                        if (!PointInside(s))
                        {
                            outputPolygon.Add(IntersectionPoint());
                        }
                        outputPolygon.Add(e);
                    }
                    else if (PointInside(s))
                    {
                        outputPolygon.Add(IntersectionPoint());
                    }
                    s = e;
                }
                cp1 = cp2;
            }

            return outputPolygon;
        }
    }
}
