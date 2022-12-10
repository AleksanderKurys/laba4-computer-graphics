using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Laba4
{
    public class Clipper
    {
        private class Edge
        {
            public Edge(Vector2f from, Vector2f to)
            {
                this.From = from;
                this.To = to;
            }

            public readonly Vector2f From;
            public readonly Vector2f To;
        }

        public static List<Vector2f> GetIntersectedPolygon(Figure subjectPolygon, Figure clipPolygon)
        {
            Vector2f[] subjectPoly = DrawingUtils.GetFigureVertexes(subjectPolygon).ToArray();
            Vector2f[] clipPoly = DrawingUtils.GetFigureVertexes(clipPolygon).ToArray();

            if (subjectPoly.Length < 3 || clipPoly.Length < 3)
            {
                throw new ArgumentException(string.Format("The polygons passed in must have at least 3 Vector2fs: subject={0}, clip={1}", subjectPoly.Length.ToString(), clipPoly.Length.ToString()));
            }

            List<Vector2f> outputList = subjectPoly.ToList();

            //	Make sure it's clockwise
            if (!IsClockwise(subjectPoly))
            {
                outputList.Reverse();
            }

            //	Walk around the clip polygon clockwise
            foreach (Edge clipEdge in IterateEdgesClockwise(clipPoly))
            {
                List<Vector2f> inputList = outputList.ToList();		//	clone it
                outputList.Clear();

                if (inputList.Count == 0)
                {
                    //	Sometimes when the polygons don't intersect, this list goes to zero.  Jump out to avoid an index out of range exception
                    break;
                }

                Vector2f S = inputList[inputList.Count - 1];

                foreach (Vector2f E in inputList)
                {
                    if (IsInside(clipEdge, E))
                    {
                        if (!IsInside(clipEdge, S))
                        {
                            Vector2f? Vector2f = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                            if (Vector2f == null)
                            {
                                throw new ApplicationException("Line segments don't intersect");		//	may be colinear, or may be a bug
                            }
                            else
                            {
                                outputList.Add(Vector2f.Value);
                            }
                        }

                        outputList.Add(E);
                    }
                    else if (IsInside(clipEdge, S))
                    {
                        Vector2f? Vector2f = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                        if (Vector2f == null)
                        {
                            throw new ApplicationException("Line segments don't intersect");		//	may be colinear, or may be a bug
                        }
                        else
                        {
                            outputList.Add(Vector2f.Value);
                        }
                    }

                    S = E;
                }
            }

            return outputList;
        }


        public static List<Vector2f> GetUnintersectedPolygon(Figure subjectPolygon, Figure clipPolygon)
        {
            Vector2f[] subjectPoly = DrawingUtils.GetFigureVertexes(subjectPolygon).ToArray();
            Vector2f[] clipPoly = DrawingUtils.GetFigureVertexes(clipPolygon).ToArray();

            if (subjectPoly.Length < 3 || clipPoly.Length < 3)
            {
                throw new ArgumentException(string.Format("The polygons passed in must have at least 3 Vector2fs: subject={0}, clip={1}", subjectPoly.Length.ToString(), clipPoly.Length.ToString()));
            }

            List<Vector2f> outputList = subjectPoly.ToList();

            //	Make sure it's clockwise
            if (!IsClockwise(subjectPoly))
            {
                outputList.Reverse();
            }

            //	Walk around the clip polygon clockwise
            foreach (Edge clipEdge in IterateEdgesClockwise(clipPoly))
            {
                List<Vector2f> inputList = outputList.ToList();		//	clone it
                outputList.Clear();

                if (inputList.Count == 0)
                {
                    //	Sometimes when the polygons don't intersect, this list goes to zero.  Jump out to avoid an index out of range exception
                    break;
                }

                Vector2f S = inputList[inputList.Count - 1];

                foreach (Vector2f E in inputList)
                {
                    if (IsInside(clipEdge, E))
                    {
                        if (!IsInside(clipEdge, S))
                        {
                            Vector2f? Vector2f = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                            if (Vector2f == null)
                            {
                                throw new ApplicationException("Line segments don't intersect");		//	may be colinear, or may be a bug
                            }
                            else
                            {
                                outputList.Add(Vector2f.Value);
                            }
                        }

                        outputList.Add(E);
                    }
                    else if (IsInside(clipEdge, S))
                    {
                        Vector2f? Vector2f = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                        if (Vector2f == null)
                        {
                            throw new ApplicationException("Line segments don't intersect");		//	may be colinear, or may be a bug
                        }
                        else
                        {
                            outputList.Add(Vector2f.Value);
                        }
                    }

                    S = E;
                }
            }

            double XX = 0;
            double YY = 0;

            List<Vector2f> result = new();
            for (int i = 0; i < outputList.Count; i++)
            {
                if (!subjectPoly.Contains(outputList[i]))
                {
                    XX += outputList[i].X;
                    YY += outputList[i].Y;
                    result.Add(outputList[i]);
                }
            }

            foreach(var i in subjectPoly)
            {
                if(!outputList.Contains(i))
                {
                    XX += i.X;
                    YY += i.Y;
                    result.Add(i);
                }
            }

            XX /= result.Count;
            YY /= result.Count;

            result = result.OrderBy(x => Math.Atan2(x.X - XX, x.Y - YY)).ToList();

            return result;
        }

        private static IEnumerable<Edge> IterateEdgesClockwise(Vector2f[] polygon)
        {
            if (IsClockwise(polygon))
            {
                for (int cntr = 0; cntr < polygon.Length - 1; cntr++)
                {
                    yield return new Edge(polygon[cntr], polygon[cntr + 1]);
                }

                yield return new Edge(polygon[polygon.Length - 1], polygon[0]);
            }
            else
            {
                for (int cntr = polygon.Length - 1; cntr > 0; cntr--)
                {
                    yield return new Edge(polygon[cntr], polygon[cntr - 1]);
                }

                yield return new Edge(polygon[0], polygon[polygon.Length - 1]);
            }
        }

        private static Vector2f? GetIntersect(Vector2f line1From, Vector2f line1To, Vector2f line2From, Vector2f line2To)
        {
            Vector2f direction1 = line1To - line1From;
            Vector2f direction2 = line2To - line2From;
            float dotPerp = (direction1.X * direction2.Y) - (direction1.Y * direction2.X);

            // If it's 0, it means the lines are parallel so have infinite intersection Vector2fs
            if (IsNearZero(dotPerp))
            {
                return null;
            }

            Vector2f c = line2From - line1From;
            float t = (c.X * direction2.Y - c.Y * direction2.X) / dotPerp;

            return line1From + (new Vector2f(direction1.X * t, direction1.Y * t));
        }

        private static bool IsInside(Edge edge, Vector2f test)
        {
            bool? isLeft = IsLeftOf(edge, test);
            if (isLeft == null)
            {
                //	Colinear Vector2fs should be considered inside
                return true;
            }

            return !isLeft.Value;
        }
        private static bool IsClockwise(Vector2f[] polygon)
        {
            for (int cntr = 2; cntr < polygon.Length; cntr++)
            {
                bool? isLeft = IsLeftOf(new Edge(polygon[0], polygon[1]), polygon[cntr]);
                if (isLeft != null)		//	some of the Vector2fs may be colinear.  That's ok as long as the overall is a polygon
                {
                    return !isLeft.Value;
                }
            }

            throw new ArgumentException("All the Vector2fs in the polygon are colinear");
        }

        private static bool? IsLeftOf(Edge edge, Vector2f test)
        {
            Vector2f tmp1 = edge.To - edge.From;
            Vector2f tmp2 = test - edge.To;

            double x = (tmp1.X * tmp2.Y) - (tmp1.Y * tmp2.X);		//	dot product of perpendicular?

            if (x < 0)
            {
                return false;
            }
            else if (x > 0)
            {
                return true;
            }
            else
            {
                //	Colinear Vector2fs;
                return null;
            }
        }

        private static bool IsNearZero(double testValue)
        {
            return Math.Abs(testValue) <= .000000001d;
        }

    }
}
