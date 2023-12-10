using System.Data;
using System.Runtime.InteropServices;

namespace Advent2023;

public readonly struct Position(int row, int col)
{
    public readonly int Row { get; } = row;
    public readonly int Col { get; } = col;
    public override string ToString()
    {
        return $"<Position {Row}, {Col}>";
    }
}
public sealed class Maze
{
    private string[] _rows;
    public Position Start { get; }
    public Maze(string filename)
    {
        _rows = [.. File.ReadAllLines(filename)];
        int startrow = (from i in Enumerable.Range(0, _rows.Length) where _rows[i].Contains('S') select i).First();
        int startcol = _rows[startrow].IndexOf('S');
        Start = new(startrow, startcol);
    }
    private char GetStartShape()
    {
        if ("7|F".Contains(_rows[Start.Row - 1][Start.Col]))
        {
            if ("J-7".Contains(_rows[Start.Row][Start.Col + 1]))
            {
                return 'L';
            }
            if ("J|L".Contains(_rows[Start.Row + 1][Start.Col]))
            {
                return '|';
            }
            if ("L-F".Contains(_rows[Start.Row][Start.Col - 1]))
            {
                return 'J';
            }
        }
        if ("J|L".Contains(_rows[Start.Row + 1][Start.Col]))
        {
            if ("J-7".Contains(_rows[Start.Row][Start.Col + 1]))
            {
                return 'F';
            }
            if ("L-F".Contains(_rows[Start.Row][Start.Col - 1]))
            {
                return '7';
            }
        }
        if ("L-F".Contains(_rows[Start.Row][Start.Col - 1]) &&
            "J-7".Contains(_rows[Start.Row][Start.Col + 1]))
        {
            return '-';
        }
        throw new KeyNotFoundException();
    }
    private char TileAtPosition(Position pos)
    {
        if (pos.Row == Start.Row && pos.Col == Start.Col)
        {
            return GetStartShape();
        }
        return _rows[pos.Row][pos.Col];
    }
    private List<Position> Neighbours(Position pos)
    {
        return TileAtPosition(pos) switch
        {
            'F' => [new Position(pos.Row + 1, pos.Col), new Position(pos.Row, pos.Col + 1)],
            '-' => [new Position(pos.Row, pos.Col - 1), new Position(pos.Row, pos.Col + 1)],
            '7' => [new Position(pos.Row + 1, pos.Col), new Position(pos.Row, pos.Col - 1)],
            'J' => [new Position(pos.Row - 1, pos.Col), new Position(pos.Row, pos.Col - 1)],
            '|' => [new Position(pos.Row - 1, pos.Col), new Position(pos.Row + 1, pos.Col)],
            'L' => [new Position(pos.Row - 1, pos.Col), new Position(pos.Row, pos.Col + 1)],
            _ => [],
        };
    }
    public List<Position> Loop()
    {
        List<Position> visited = [];
        Position current = Start;
        while (true)
        {
            visited.Add(current);
            IEnumerable<Position> neighbours = from n in Neighbours(current) where !visited.Contains(n) select n;
            if (!neighbours.Any())
            {
                return visited;
            }
            current = neighbours.First();
        }
    }
    public int InsideArea()
    {
        List<Position> loop = Loop();
        int area = 0;
        for (int row = 0; row < _rows.Length; row++)
        {
            bool outside = true;
            char corner = '.';
            for (int col = 0; col < _rows[row].Length; col++)
            {
                Position pos = new(row, col);
                if (loop.Contains(pos))
                {
                    char tile = TileAtPosition(pos);
                    if (tile == '|' || (corner == 'L' && tile == '7') || (corner == 'F' && tile == 'J'))
                    {
                        outside = !outside;
                        corner = tile;
                    }
                    else if ("LF".Contains(tile))
                    {
                        corner = tile;
                    }
                }
                else if (!outside)
                {
                    area++;
                }
            }
        }
        return area;
    }
}

public static class Day10PipeMaze
{
    public static int FarthestTile(string filename)
    {
        return new Maze(filename).Loop().Count / 2;

    }
    public static int InsideArea(string filename)
    {
        return new Maze(filename).InsideArea();
    }
}
