using System.Security.Cryptography;

namespace Advent2023;
class Crossing(double x, double y, double t0, double t1)
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public double T0 { get; } = t0;
    public double T1 { get; } = t1;
    public override string ToString()
    {
        return $"({X:F2}, {Y:F2}, t0={T0:F2}, t1 = {T1:F2})";
    }
}
class Equation(double a, double b)
{
    // y = Ax + B
    public double A { get; } = a;
    public double B { get; } = b;
}
class Pos3d
{
    public long X { get; }
    public long Y { get; }
    public long Z { get; }

    public Pos3d(long x, long y, long z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Pos3d(IEnumerable<long> longs)
    {
        X = longs.First();
        Y = longs.Skip(1).First();
        Z = longs.Skip(2).First();
    }
    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}>";
    }
    public Pos3d Diff(Pos3d other)
    {
        return new(X - other.X, Y - other.Y, Z - other.Z);
    }
    public Pos3d? DividedBy(int n)
    {
        if (X % n == 0 && Y % n == 0 && Z % n == 0)
        {
            return new(X / n, Y / n, Z / n);
        }
        return null;
    }
}
class HailStone
{
    Pos3d _position;
    Pos3d _velocity;
    public HailStone(string line)
    {
        var split = line.Split(" @ ");
        _position = new((from pos in split[0].Split(", ") select Int64.Parse(pos)).ToArray());
        _velocity = new((from pos in split[1].Split(", ") select Int64.Parse(pos)).ToArray());
    }
    public override string ToString()
    {
        return $"{String.Join(", ", _position)} @ {String.Join(", ", _velocity)}";
    }
    public Equation Slope()
    {
        double tx0 = -1.0 * _position.X / _velocity.X;
        double y0 = _position.Y + tx0 * _velocity.Y;
        double ty0 = -1.0 * _position.Y / _velocity.Y;
        double x0 = _position.X + ty0 * _velocity.X;
        return new Equation(-y0 / x0, y0);
    }
    public Crossing Cross(HailStone other)
    {
        Equation mySlope = Slope();
        Equation otherSlope = other.Slope();
        double x = (otherSlope.B - mySlope.B) / (mySlope.A - otherSlope.A);
        double t0 = (x - _position.X) / _velocity.X;
        double t1 = (x - other._position.X) / other._velocity.X;
        return new Crossing(x, x * mySlope.A + mySlope.B, t0, t1);
    }
    public Pos3d PositionAtT(int t)
    {
        return new(_position.X + t * _velocity.X, _position.Y + t * _velocity.Y, _position.Z + t * _velocity.Z);
    }
}
public static class Day24NeverTellMeTheOdds
{
    public static int NumIntersections(string filename, long minPos, long maxPos)
    {
        HailStone[] hailStones = (from line in File.ReadAllLines(filename) select new HailStone(line)).ToArray();
        int intersections = 0;
        foreach (int i in Enumerable.Range(0, hailStones.Length - 1))
        {
            foreach (int j in Enumerable.Range(i + 1, hailStones.Length - i - 1))
            {
                Crossing crossing = hailStones[i].Cross(hailStones[j]);
                if (minPos <= crossing.X && crossing.X <= maxPos
                    && minPos <= crossing.Y && crossing.Y <= maxPos
                    && 0 <= crossing.T0 && 0 <= crossing.T1)
                {
                    intersections++;
                }
            }
        }
        return intersections;
    }
    public static long SumCoordinate(string filename)
    {
        HailStone[] hailStones = (from line in File.ReadAllLines(filename) select new HailStone(line)).ToArray();
        int n = 100;
        int steps = 100;
        Pos3d[][] t = (from i in Enumerable.Range(0, n) select (from hs in hailStones select hs.PositionAtT(i)).ToArray()).ToArray();
        foreach (Pos3d[] a in t)
        {
            Console.WriteLine(String.Join(", ", a.ToList()));
        }
        Dictionary<string, int> diffs = [];
        foreach (int step in Enumerable.Range(1, steps))
        {
            foreach (int i in Enumerable.Range(0, t.Length - step))
            {
                var tDiff = from a in t[i] from b in t[i + step] 
                            select a.Diff(b).DividedBy(step)?.ToString();
                foreach (string diff in tDiff)
                {
                    if (diff is null)
                    {
                        continue;
                    }
                    if (diffs.ContainsKey(diff))
                    {
                        diffs[diff] += 1;
                    }
                    else
                    {
                        diffs[diff] = 1;
                    }
                }
            } 
        }
        var maxHits = diffs.Values.Where(x => x < n - 1).Max();
        var maxString = diffs.First(kv => kv.Value == maxHits).Key;
        Console.WriteLine($"maxString = {maxString} (maxHits = {maxHits})");
        if (maxHits == 1)
        {
            return 0;
        }
        foreach (int i in Enumerable.Range(0, t.Length - 1))
        {
            var tDiff = from a in t[i] from b in t[i + 1] 
                        select a.Diff(b).ToString();
            // Console.WriteLine($"{String.Join(", ", tDiff)}");
            foreach (var (First, Second) in tDiff.Zip(Enumerable.Range(0, tDiff.Count())))
            {
                if (First == maxString)
                {
                    int aIndex = Second / t[i].Length;
                    int bIndex = Second % t[i].Length;
                    Console.WriteLine($"{i}: Found {First} at {Second} ({aIndex}, {bIndex})");
                    Console.WriteLine(String.Join(',', t[i].ToList()));
                    Pos3d a = t[i][aIndex];
                    Pos3d b = t[i + 1][bIndex];
                    Console.WriteLine($"a = {a}, b = {b}");
                    Pos3d d = a.Diff(b);
                    long x = a.X + i * d.X;
                    long y = a.Y + i * d.Y;
                    long z = a.Z + i * d.Z;
                    Console.WriteLine($"x = {x}, y = {y}, z = {z}");
                    return x + y + z;
                }
            }
        }
        
        foreach (var kv in diffs)
        {
            if (kv.Value > 1 && kv.Value < n - 1)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }
        }
        return 0;
    }
}
