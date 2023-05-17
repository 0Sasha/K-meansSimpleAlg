namespace K_means;

public class Point
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Point? Centroid { get; set; } = null;

    public Point(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString() => X + " " + Y + " " + Z;

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is Point p) return Math.Abs(X - p.X) < double.Epsilon &&
                Math.Abs(Y - p.Y) < double.Epsilon && Math.Abs(Z - p.Z) < double.Epsilon;
        return false;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        var p1 = new Point(17, 54, 65);
        var p2 = new Point(45, 47, 24);
        var p3 = new Point(1, 14, 85);
        var res = FindKMeans(2, p1, p2, p3);
        Console.ReadLine();

        p1 = new Point(17.23, 54, -65);
        p2 = new Point(45, -47, 24.94);
        p3 = new Point(-1, 14.36, 85);
        var p4 = new Point(12, 13, 14);
        var p5 = new Point(56, 50, 40);
        res = FindKMeans(3, p1, p2, p3, p4, p5);
        Console.ReadLine();
    }


    public static Point[] FindKMeans(int k, params Point[] points)
    {
        if (points == null || points.Length == 0) throw new ArgumentException(null, nameof(points));
        if (k < 2 || k > points.Length) throw new ArgumentException(null, nameof(k));

        Point[] centroids = points.Take(k).ToArray();
        Point[]? oldCentroids = null;

        while(!AreEquel(centroids, oldCentroids))
        {
            foreach (Point point in points) point.Centroid = FindClosestCentroid(point, centroids);
            oldCentroids = centroids;
            centroids = GetNewCentroids(centroids, points);
        }
        return centroids;
    }

    private static bool AreEquel(Point[] points1, Point[]? points2)
    {
        if (points2 == null) return false;
        for (int i = 0; i < points1.Length; i++)
            if (!points1[i].Equals(points2[i])) return false;
        return true;
    }

    private static Point FindClosestCentroid(Point point, Point[] centroids)
    {
        double closestDistance = double.MaxValue;
        Point centroid = centroids[0];
        foreach(var curCentr in centroids)
        {
            var dist = GetDistance(point, curCentr);
            if (dist - closestDistance < double.Epsilon)
            {
                closestDistance = dist;
                centroid = curCentr;
            }
        }
        return centroid;
    }

    private static double GetDistance(Point point, Point curCentr)
    {
        var distX = point.X - curCentr.X;
        var distY = point.Y - curCentr.Y;
        var distZ = point.Z - curCentr.Z;
        return Math.Sqrt(distX * distX + distY * distY + distZ * distZ);
    }

    private static Point[] GetNewCentroids(Point[] centroids, Point[] points)
    {
        Point[] newCentroids = new Point[centroids.Length];
        for (int i = 0; i < centroids.Length; i++)
        {
            var curPoints = points.Where(p => p.Centroid == centroids[i]).ToArray();
            newCentroids[i] = new(curPoints.Select(p => p.X).Average(),
                curPoints.Select(p => p.Y).Average(), curPoints.Select(p => p.Z).Average());
        }
        return newCentroids;
    }
}
