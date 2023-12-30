namespace Advent2023;

sealed class Cube(int x, int y, int z)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Z { get; set; } = z;

    public override string ToString()
    {
        return $"<{X},{Y},{Z}>";
    }

    public bool IsBelow(Cube other)
    {
        // Console.WriteLine($"Cube.IsBelow {X} == {other.X} && {Y} == {other.Y} && {Z} == {other.Z} - 1 = {X == other.X && Y == other.Y && Z == other.Z - 1}");
        return X == other.X && Y == other.Y && Z == other.Z - 1;
    }
    public Cube FallBy(int dz)
    {
        Z -= dz;
        return this;
    }
}

sealed class Brick
{
    public int MinZ { get; }
    IEnumerable<Cube> _cubes;
    public int Id { get; }
    static int _index;
    public Brick(string line)
    {
        string[] split = line.Split('~');
        int[] startPos = (from dim in split[0].Split(',') select Int32.Parse(dim)).ToArray();
        int[] endPos = (from dim in split[1].Split(',') select Int32.Parse(dim)).ToArray();
        if (startPos[0] != endPos[0])
        {
            _cubes = from x in Enumerable.Range(startPos[0], endPos[0] - startPos[0] + 1)
                     select new Cube(x, startPos[1], startPos[2]);
        }
        else if (startPos[1] != endPos[1])
        {
            _cubes = from y in Enumerable.Range(startPos[1], endPos[1] - startPos[1] + 1)
                     select new Cube(startPos[0], y, startPos[2]);
        }
        else
        {
            _cubes = from z in Enumerable.Range(startPos[2], endPos[2] - startPos[2] + 1)
                     select new Cube(startPos[0], startPos[1], z);
        }
        MinZ = startPos[2];
        Id = _index++;
    }
    public override string ToString()
    {
        return $"<Brick {Id} {String.Join(',', _cubes)}>";
    }
    public bool IsBelow(Brick other)
    {
        // var x = from cube in _cubes from otherCube in other._cubes select cube.IsBelow(otherCube);
        // Console.WriteLine($"IsBelow {this}, {other}: {String.Join(',', x)} = {x.Any(x => x)}");

        return (from cube in _cubes from otherCube in other._cubes select cube.IsBelow(otherCube)).Any(x => x);
    }
    public int ZDistance(Brick other)
    {
        var zDistances = from cube in _cubes
                         from otherCube in other._cubes
                         where cube.X == otherCube.X && cube.Y == otherCube.Y
                         select cube.Z - otherCube.Z;
        if (!zDistances.Any())
        {
            return MinZ;
        }
        return zDistances.Min();
    }
    public void FallBy(int dz)
    {
        _cubes = from cube in _cubes select cube.FallBy(dz);
    }
}

sealed class BrickStack
{
    List<Brick> _bricks;
    public BrickStack(string filename)
    {
        _bricks = (from line in File.ReadAllLines(filename)
                   select new Brick(line)).ToList();
        Fall();
    }
    public override string ToString()
    {
        return String.Join('\n', _bricks.OrderByDescending(b => b.MinZ));
    }
    public void Fall()
    {
        List<Brick> fallen = [];
        foreach (Brick brick in _bricks.OrderBy(b => b.MinZ))
        {
            if (brick.MinZ == 1)
            {
                fallen.Add(brick);
                continue;
            }
            IEnumerable<int> zdistances = from other in fallen
                                          select brick.ZDistance(other);
            if (zdistances.Any(dz => dz == 1))
            {
                fallen.Add(brick);
                continue;
            }
            if (zdistances.Min() < 200)
            {
                brick.FallBy(zdistances.Min() - 1);
                fallen.Add(brick);
                continue;
            }
            Console.WriteLine($"no brick below {brick}");
        }
    }
    public int CanBeDisintegrated()
    {
        return _bricks.Count - Supports().Count;
    }

    private HashSet<int> Supports()
    {
        Brick[] bricks = [.. _bricks.OrderBy(b => b.MinZ)];
        Dictionary<int, List<int>> below = [];
        foreach (int i in Enumerable.Range(0, bricks.Length - 1))
        {
            foreach (int j in Enumerable.Range(i + 1, bricks.Length - i - 1))
            {
                if (bricks[i].IsBelow(bricks[j]))
                {
                    if (below.TryGetValue(j, out List<int>? blist))
                    {
                        blist.Add(i);
                    }
                    else
                    {
                        below[j] = [i];
                    }
                }
            }
        }

        return (from support in below.Values where support.Count == 1 select support[0]).ToHashSet();
    }

    public int SumOtherFalling()
    {
        Brick[] bricks = [.. _bricks.OrderBy(b => b.MinZ)];
        Dictionary<int, List<int>> supportedBy = [];
        foreach (int i in Enumerable.Range(0, bricks.Length - 1))
        {
            foreach (int j in Enumerable.Range(i + 1, bricks.Length - i - 1))
            {
                if (bricks[i].IsBelow(bricks[j]))
                {
                    if (supportedBy.TryGetValue(i, out List<int>? slist))
                    {
                        slist.Add(j);
                    }
                    else
                    {
                        supportedBy[i] = [j];
                    }
                }
            }
        }
        // foreach (var kv in supportedBy)
        // {
        //     Console.WriteLine($"{kv.Key}: {String.Join(',', kv.Value)}");
        // }
        int nFalls = 0;
        Dictionary<int, HashSet<int>> cache = [];
        foreach (int first in Supports())
        {
            Console.WriteLine($"NEW first: {first} (nFalls = {nFalls})");
            Queue<int> queue = new();
            queue.Enqueue(first);
            HashSet<int> falls = [];
            while (queue.Count > 0)
            {
                // foreach (var kv in cache)
                // {
                //     Console.WriteLine($"  cache {kv.Key}: {String.Join(',', kv.Value)}");
                // }
                int current = queue.Dequeue();
                Console.WriteLine($"looking at {current}");
                if (cache.TryGetValue(current, out HashSet<int>? cacheline))
                {
                    Console.WriteLine($" found {current} in cache ({String.Join(',', cacheline)})");
                    falls.UnionWith(cacheline);
                    Console.WriteLine($"  falls now {String.Join(',', falls)}");
                    continue;
                }
                HashSet<int> curFalls = [];
                if (supportedBy.TryGetValue(current, out List<int>? value))
                {
                    value.ForEach(n =>
                    {
                        if (!curFalls.Contains(n))
                        {
                            Console.WriteLine($"  enqueuing {n}");
                            queue.Enqueue(n);
                            curFalls.Add(n);
                        }
                    });
                }
                if (cache.TryGetValue(current, out HashSet<int>? cvalue))
                {
                    cvalue.UnionWith(curFalls);
                }
                else
                {
                    cache[current] = [.. curFalls];
                }
                falls.UnionWith(curFalls);
                curFalls.Clear();
            }
            Console.WriteLine($"{first}: {falls.Count} [{String.Join(',', falls)}]");
            nFalls += falls.Count;
            falls.Clear();
        }
        return nFalls;
    }
}

public static class Day22SandSlabs
{
    public static int NumberOfDisintegratableBricks(string filename)
    {
        BrickStack brickStack = new(filename);
        return brickStack.CanBeDisintegrated();
    }
    public static int SumOtherFalling(string filename)
    {
        BrickStack brickStack = new(filename);
        return brickStack.SumOtherFalling();
    }
}
