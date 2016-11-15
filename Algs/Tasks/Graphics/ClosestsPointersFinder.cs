namespace Algs.Tasks.Graphics
{
    //public class ClosestsPointersFinder
    //{
    //    private readonly Point[] points;
    //    private readonly int[] xSorting;
    //    private readonly int[] ySorting;
    //    private Point minPoint1;
    //    private Point minPoint2;
    //    private int minDistance;

    //    public ClosestsPointersFinder(Point[] points)
    //    {
    //        this.points = points;
    //        if (points.Length < 2)
    //        {
    //            const string messageFormat = "must be at least 2 points, but was [{0}]";
    //            throw new InvalidOperationException(string.Format(messageFormat, points.Length));
    //        }
    //        xSorting = new int[points.Length];
    //        ySorting = new int[points.Length];
    //        for (var i = 0; i < points.Length; i++)
    //        {
    //            xSorting[i] = i;
    //            ySorting[i] = i;
    //        }
    //        Array.Sort(xSorting, (i1, i2) => points[i1].x.CompareTo(points[i2].x));
    //        Array.Sort(ySorting, (i1, i2) => points[i1].y.CompareTo(points[i2].y));
    //    }

    //    public Tuple<Point, Point> Find()
    //    {
    //        Search(0, points.Length - 1, 0, points.Length - 1, true);
    //        return Tuple.Create(points[xSorting[minPoint1]], points[xSorting[minPoint2]]);
    //    }

    //    //contract:
    //    //left <= right
    //    public void Search(int xStart, int xFinish, int yStart, int yFinish, bool devideByX)
    //    {
    //        if (xStart == xFinish || yStart == yFinish)
    //            return;
    //        if (devideByX)
    //        {
    //            var len = xFinish - xStart + 1;
    //            var midIndex = len/2;
    //            Search(xStart, midIndex, yStart, yFinish, false);
    //            Search(midIndex + 1, xFinish, yStart, yFinish, false);
    //            var midX = points[xSorting[midIndex]].x;
    //            var left = yStart;
    //            while (points[ySorting[left]].x > midX)
    //            {
    //                left++;
    //                if (left == yFinish)
    //                    return;
    //            }
    //            var right = yStart;
    //            while (points[ySorting[right]].x <= midX)
    //            {
    //                right++;
    //                if (right == yFinish)
    //                    return;
    //            }
    //            while (true)
    //            {
    //                var pLeft = points[ySorting[left]];
    //                var pRight = points[ySorting[right]];
    //                var d = pRight.x - pLeft.x + Math.Abs(pRight.y - pLeft.y);
    //                if (d < minDistance)
    //                {
    //                    minPoint1 = pLeft;
    //                    minPoint2 = pRight;
    //                    minDistance = d;
    //                }
    //                //while()
    //            }
    //        }
    //    }
    //}
}