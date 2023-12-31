namespace Advent2023;
enum Direction
{
    Right,
    Down,
    Left,
    Up
}
struct Beam
{
    public Position Position;
    public Direction Direction;
    public override string ToString()
    {
        return $"<Beam {Position} {Direction}>";
    }
}
struct StackItem
{
    public Beam Beam;
    public List<Beam> Path;
}
sealed class Floor(string filename)
{
    string[] _tiles = File.ReadAllLines(filename);
    Dictionary<Beam, HashSet<Position>> _cache = [];
    private char TileAt(Position pos)
    {
        return _tiles[pos.Row][pos.Col];
    }
    public Beam? Next(Beam beam, Direction direction)
    {
        int newRow = direction switch
        {
            Direction.Right or Direction.Left => beam.Position.Row,
            Direction.Up when beam.Position.Row > 0 => beam.Position.Row - 1,
            Direction.Down when beam.Position.Row < _tiles.Length - 1 => beam.Position.Row + 1,
            _ => -1,
        };
        int newCol = direction switch
        {
            Direction.Up or Direction.Down => beam.Position.Col,
            Direction.Left when beam.Position.Col > 0 => beam.Position.Col - 1,
            Direction.Right when beam.Position.Col < _tiles[0].Length - 1 => beam.Position.Col + 1,
            _ => -1,
        };
        if (newRow < 0 || newCol < 0)
        {
            return null;
        }
        return new Beam() { Position = new Position(newRow, newCol), Direction = direction };
    }
    public int EnergizedTiles(Beam start)
    {
        Stack<StackItem> stack = [];
        HashSet<Position> energized = [];
        stack.Push(new StackItem() { Beam = start, Path = [] });
        while (stack.Count > 0)
        {
            StackItem item = stack.Pop();
            Beam current = item.Beam;
            List<Beam> path = item.Path;

            if (_cache.TryGetValue(current, out HashSet<Position>? cached))
            {
                energized.UnionWith(cached);
                continue;
            }
            if (path.Contains(current))
            {
                continue;
            }
            energized.Add(current.Position);
            path.Add(current);

            List<Direction> nextDirections = TileAt(current.Position) switch
            {
                '.' => [current.Direction],
                '/' when current.Direction == Direction.Right => [Direction.Up],
                '/' when current.Direction == Direction.Left => [Direction.Down],
                '/' when current.Direction == Direction.Up => [Direction.Right],
                '/' when current.Direction == Direction.Down => [Direction.Left],
                '\\' when current.Direction == Direction.Right => [Direction.Down],
                '\\' when current.Direction == Direction.Left => [Direction.Up],
                '\\' when current.Direction == Direction.Up => [Direction.Left],
                '\\' when current.Direction == Direction.Down => [Direction.Right],
                '-' when current.Direction == Direction.Right => [Direction.Right],
                '-' when current.Direction == Direction.Left => [Direction.Left],
                '-' when current.Direction == Direction.Up => [Direction.Left, Direction.Right],
                '-' when current.Direction == Direction.Down => [Direction.Left, Direction.Right],
                '|' when current.Direction == Direction.Up => [Direction.Up],
                '|' when current.Direction == Direction.Down => [Direction.Down],
                '|' when current.Direction == Direction.Right => [Direction.Up, Direction.Down],
                '|' when current.Direction == Direction.Left => [Direction.Up, Direction.Down],
                _ => [],
            };
            foreach (Direction direction in nextDirections)
            {
                if (Next(current, direction) is Beam beam)
                {
                    stack.Push(new StackItem() { Beam = beam, Path = path });
                }
                else
                {
                    foreach (int i in Enumerable.Range(0, path.Count - 1))
                    {
                        _cache[path[i]] = (from visited in path[(i + 1)..] select visited.Position).ToHashSet();
                    }
                }
            }
        }

        return energized.Count;
    }
    public IEnumerable<Beam> Edges()
    {
        var x = (from row in Enumerable.Range(0, _tiles.Length)
                 select new Beam() { Position = new Position(row, 0), Direction = Direction.Right })
            .Concat(from row in Enumerable.Range(0, _tiles.Length)
                    select new Beam() { Position = new Position(row, _tiles[0].Length - 1), Direction = Direction.Left })
            .Concat(from col in Enumerable.Range(0, _tiles[0].Length)
                    select new Beam() { Position = new Position(0, col), Direction = Direction.Down })
            .Concat(from col in Enumerable.Range(0, _tiles[0].Length)
                    select new Beam() { Position = new Position(_tiles.Length - 1, col), Direction = Direction.Up });
        return x;
    }
}
public static class Day16TheFloorWillBeLava
{
    public static int EnergizedTiles(string filename)
    {
        return new Floor(filename)
            .EnergizedTiles(new Beam() { Position = new Position(0, 0), Direction = Direction.Right });

    }
    public static int MaxEnergizedTiles(string filename)
    {
        Floor floor = new Floor(filename);
        return (from edge in floor.Edges() select floor.EnergizedTiles(edge)).Max();
    }
}
