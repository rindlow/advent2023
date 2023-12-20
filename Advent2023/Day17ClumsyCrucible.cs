using System.Collections.Generic;
namespace Advent2023;

struct QueueItem
{
    public Position Pos;
    public int NumStraight;
}

sealed class HeatMap
{
    int[][] _map;
    Dictionary<Position, Position> _cameFrom = [];
    HashSet<Position> _examined = [];

    public HeatMap(string filename)
    {
        _map = (from row in File.ReadAllLines(filename)
                select (from digit in row select digit - '0').ToArray()).ToArray();
    }
    private string cameFromIndicator(Position pos)
    {
        if (_cameFrom.TryGetValue(pos, out Position last))
        {
            if (_examined.Contains(pos))
            {
                return (pos.Row - last.Row, pos.Col - last.Col) switch
                {
                    (0, -1) => "<",
                    (0, 1) => ">",
                    (-1, 0) => "^",
                    (1, 0) => "v",
                    _ => ".",
                };
            }
        }
        return $"{_map[pos.Row][pos.Col]}";
    }
    private void Print()
    {
        foreach (int row in Enumerable.Range(0, _map.Length))
        {
            foreach (int col in Enumerable.Range(0, _map[0].Length))
            {
                Console.Write(cameFromIndicator(new Position(row, col)));
            }
            Console.WriteLine();
        }
    }
    private int H(Position pos)
    {
        return _map.Length - pos.Row + _map[0].Length - pos.Col;
    }
    private int D(Position pos)
    {
        return _map[pos.Row][pos.Col];
    }
    public List<Position> ReconstructPath(Position pos)
    {
        List<Position> res = [pos];
        while (_cameFrom.TryGetValue(pos, out Position prevPos))
        {
            res.Add(prevPos);
            pos = prevPos;
        }
        res.Reverse();
        return res;
    }
    private bool IsFourStraight(Position pos, Position next)
    {
        List<Position> res = [];
        if (_cameFrom.TryGetValue(pos, out Position last))
        {
            if (_cameFrom.TryGetValue(last, out Position secondLast))
            {
                if (_cameFrom.TryGetValue(secondLast, out Position thirdLast))
                {
                    res.Add(thirdLast);
                }
                else
                {
                    return false;
                }
                res.Add(secondLast);
            }
            else
            {
                return false;
            }
            res.Add(last);
        }
        else
        {
            return false;
        }
        res.Add(pos);
        bool straight = res.All(p => p.Row == next.Row) || res.All(p => p.Col == next.Col);
        Console.WriteLine($"IsFourStraight({pos}, {next}) {String.Join(',', res)} => {straight}");
        return straight;
    }
    private IEnumerable<Position> Neighbours(Position pos)
    {
        List<Position> neighbours = [];
        if (pos.Row > 0)
        {
            neighbours.Add(new Position(pos.Row - 1, pos.Col));
        }
        if (pos.Col > 0)
        {
            neighbours.Add(new Position(pos.Row, pos.Col - 1));
        }
        if (pos.Row < _map.Length - 1)
        {
            neighbours.Add(new Position(pos.Row + 1, pos.Col));
        }
        if (pos.Col < _map[0].Length - 1)
        {
            neighbours.Add(new Position(pos.Row, pos.Col + 1));
        }
        if (_cameFrom.TryGetValue(pos, out Position last))
        {
            neighbours.Remove(last);
        }
        return from neighbour in neighbours
               where !IsFourStraight(pos, neighbour)
               select neighbour;
    }
    // private bool LastIsFourStraight(PositionWithLast pos, PositionWithLast next)
    // {
    //     List<Position> res = [];
    //     if (pos.Last is PositionWithLast secondLast)
    //     {
    //         if (secondLast.L)
    //     }
    //     return false;
    // }

    // private IEnumerable<PositionWithLast> NeighboursWithPath(PositionWithLast positionWithLast)
    // {
    //     Position pos = positionWithLast.Pos;
    //     List<Position> neighbours = [];
    //     if (pos.Row > 0)
    //     {
    //         neighbours.Add(new Position(pos.Row - 1, pos.Col));
    //     }
    //     if (pos.Col > 0)
    //     {
    //         neighbours.Add(new Position(pos.Row, pos.Col - 1));
    //     }
    //     if (pos.Row < _map.Length - 1)
    //     {
    //         neighbours.Add(new Position(pos.Row + 1, pos.Col));
    //     }
    //     if (pos.Col < _map[0].Length - 1)
    //     {
    //         neighbours.Add(new Position(pos.Row, pos.Col + 1));
    //     }
    //     if (positionWithLast.Last is Position last)
    //     {
    //         neighbours.Remove(last);
    //     }
    //     Console.WriteLine($"candidates = {String.Join(',', neighbours)}");
    //     var lastRows = from p in positionWithLast.Path.TakeLast(3) select p.Pos.Row;
    //     var lastCols = from p in positionWithLast.Path.TakeLast(3) select p.Pos.Col;
    //     List<PositionWithLast> path = positionWithLast.Path.ToList();
    //     path.Add(positionWithLast);
    //     return from neighbour in neighbours 
    //            where lastRows.Count() < 3 || !(lastRows.All(row => row == neighbour.Row) || lastCols.All(col => col == neighbour.Col))
    //            select new PositionWithLast() { Pos = neighbour, Path = path };
    // }
    private int MinHeatLoss(Position start, Position goal)
    {
        PriorityQueue<Position, int> openSet = new();
        Dictionary<Position, int> gScore = [];
        Dictionary<Position, int> fScore = [];
        gScore[start] = 0;
        fScore[start] = H(start);
        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            Position current = openSet.Dequeue();
            Console.WriteLine($"\nLooking at {current} (gScore {gScore[current]}, fScore {fScore[current]})");
            _examined.Add(current);
            Print();
            if (current == goal)
            {
                return gScore[goal];
            }
            foreach (Position neighbour in Neighbours(current))
            {
                int tGScore = gScore[current] + D(neighbour);
                // Console.WriteLine($"  neighbour {neighbour} tGScore = {gScore[current]} + {D(neighbour)} = {tGScore}");
                if (tGScore < gScore.GetValueOrDefault(neighbour, 999999))
                {
                    _cameFrom[neighbour] = current;
                    gScore[neighbour] = tGScore;
                    fScore[neighbour] = tGScore + H(neighbour);
                    if (!(from item in openSet.UnorderedItems
                          where item.Element == neighbour
                          select item.Element).Any())
                    {
                        // Console.WriteLine($"    enqueuing {neighbour} ({fScore[neighbour]})");
                        openSet.Enqueue(neighbour, fScore[neighbour]);
                    }
                }
            }
        }
        return -1;
    }
    class PositionWithLast
    {
        public Position Pos;
        public Position? Last;
        public override string ToString()
        {
            return $"<{Pos} [{Last}]>";
        }
    }
    // private int MinHeatLoss2(PositionWithLast start, PositionWithLast goal)
    // {
    //     PriorityQueue<PositionWithLast, int> openSet = new();
    //     Dictionary<PositionWithLast, int> gScore = [];
    //     Dictionary<PositionWithLast, int> fScore = [];
    //     gScore[start] = 0;
    //     fScore[start] = H(start.Pos);
    //     openSet.Enqueue(start, fScore[start]);
    //     while (openSet.Count > 0)
    //     {
    //         PositionWithLast current = openSet.Dequeue();
    //         Console.WriteLine($"\nLooking at {current} (gScore {gScore[current]}, fScore {fScore[current]})");
    //         if (start == goal)
    //         {
    //             return gScore[goal];
    //         }
    //         foreach (PositionWithLast neighbour in NeighboursWithPath(current))
    //         {
    //             // Console.WriteLine($"  neighbour {neighbour} tGScore = {gScore[current]} + {D(neighbour)} = {tGScore}");
    //             int tGScore = gScore[current] + D(neighbour.Pos);
    //             if (tGScore < gScore.GetValueOrDefault(neighbour, 999999))
    //             {
    //                 gScore[neighbour] = tGScore;
    //                 fScore[neighbour] = tGScore + H(neighbour.Pos);
    //                 if (!(from item in openSet.UnorderedItems
    //                       where item.Element == neighbour
    //                       select item.Element).Any())
    //                 {
    //                     openSet.Enqueue(neighbour, fScore[neighbour]);
    //                 }
    //             }
    //         }
    //     }
    //     return -1;
    // }
    public int MinimizeHeatLoss()
    {
        return MinHeatLoss(new Position(0, 0), new Position(_map.Length - 1, _map[0].Length - 1));
        // return MinHeatLoss2(new PositionWithLast() { Pos = new Position(0, 0), Last = null }, 
        //                     new PositionWithLast() { Pos = new Position(_map.Length - 1, _map[0].Length - 1), Last = null });
    }
}
public static class Day17ClumsyCrucible
{
    public static int MinimizeHeatLoss(string filename)
    {
        return new HeatMap(filename).MinimizeHeatLoss();
    }
}
