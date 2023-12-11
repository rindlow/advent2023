namespace Advent2023;
sealed class Image
{
    readonly string[] _image;
    readonly HashSet<int> _emptyCols;
    readonly List<int> _emptyRows = [];
    public Image(string filename)
    {
        _image = File.ReadAllLines(filename);
        bool rowIsEmpty;
        _emptyCols = new(Enumerable.Range(0, _image[0].Length));
        foreach (int row in Enumerable.Range(0, _image.Length))
        {
            rowIsEmpty = true;
            foreach (int col in Enumerable.Range(0, _image[0].Length))
            {
                if (_image[row][col] != '.')
                {
                    _emptyCols.Remove(col);
                    rowIsEmpty = false;
                }
            }
            if (rowIsEmpty)
            {
                _emptyRows.Add(row);
            }
        }
    }
    private Position ExpandedPosition(int row, int col, int distance)
    {
        return new Position(
            row + distance * (from empty in _emptyRows where empty < row select empty).Count(),
            col + distance * (from empty in _emptyCols where empty < col select empty).Count());
    }
    private IEnumerable<Position> Galaxies(int distance)
    {
        return from row in Enumerable.Range(0, _image.Length)
               from col in Enumerable.Range(0, _image[0].Length)
               where _image[row][col] == '#'
               select ExpandedPosition(row, col, distance);
    }
    public IEnumerable<(Position, Position)> GalaxyPairs(int distance)
    {
        Position[] galaxies = Galaxies(distance).ToArray();
        return from i in Enumerable.Range(0, galaxies.Length - 1)
               from j in Enumerable.Range(i, galaxies.Length - i)
               where i != j
               select (galaxies[i], galaxies[j]);
    }
}
public static class Day11CosmicExpansion
{
    public static long SumDistances(string filename, int distance)
    {
        return (from pair in new Image(filename).GalaxyPairs(distance)
                select (long)(Math.Abs(pair.Item1.Row - pair.Item2.Row) + Math.Abs(pair.Item1.Col - pair.Item2.Col))
               ).Sum();
    }
}
