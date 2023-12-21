namespace Advent2023;

sealed class Garden
{
    public Position Start { get; }
    string[] _plots = [];
    public Garden(string filename)
    {
        _plots = File.ReadAllLines(filename);
        foreach (int row in Enumerable.Range(0, _plots.Length))
        {
            int col = _plots[row].IndexOf('S');
            if (col >= 0)
            {
                Start = new(row, col);
                break;
            }
        }
    }
    public List<Position> Neigbours(Position pos)
    {
        List<Position> neighbours = [];
        if (pos.Row > 0 && _plots[pos.Row - 1][pos.Col] != '#')
        {
            neighbours.Add(new Position(pos.Row - 1, pos.Col));
        }
        if (pos.Row < _plots.Length - 1 && _plots[pos.Row + 1][pos.Col] != '#')
        {
            neighbours.Add(new Position(pos.Row + 1, pos.Col));
        } 
        if (pos.Col > 0 && _plots[pos.Row][pos.Col - 1] != '#')
        {
            neighbours.Add(new Position(pos.Row, pos.Col - 1));
        }
        if (pos.Col < _plots[0].Length - 1 && _plots[pos.Row][pos.Col + 1] != '#')
        {
            neighbours.Add(new Position(pos.Row, pos.Col + 1));
        }    
        return neighbours;
    }
}
public static class Day21StepCounter
{
    public static int ReachedInSteps(string filename, int steps)
    {
        Garden garden = new(filename);
        HashSet<Position> reached = [garden.Start];
        foreach (int i in Enumerable.Range(0, steps))
        {
            reached = (from plot in reached select garden.Neigbours(plot)).SelectMany(p => p).ToHashSet();
            Console.WriteLine($"step {i}");
            Console.WriteLine(String.Join('\n', reached));
            Console.WriteLine();
        }
        return reached.Count;
    }
}
