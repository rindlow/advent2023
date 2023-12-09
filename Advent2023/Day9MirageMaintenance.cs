namespace Advent2023;
public static class Day9MirageMaintenance
{
    private static long ExtrapolateNext(int[] sequence)
    {
        List<int> values = [sequence[^1]];
        while (!sequence.All(n => n == 0))
        {
            int[] diff = (from pair in sequence[0..^1].Zip(sequence[1..]) select pair.Second - pair.First).ToArray();
            values.Add(diff[^1]);
            sequence = diff;
        }
        return values.Sum();
    }
    private static long ExtrapolatePrevious(int[] sequence)
    {
        List<int> values = [sequence[0]];
        while (!sequence.All(n => n == 0))
        {
            int[] diff = (from pair in sequence[0..^1].Zip(sequence[1..]) select pair.Second - pair.First).ToArray();
            values.Add(diff[0]);
            sequence = diff;
        }
        return Enumerable.Reverse(values).Aggregate(0, (prev, first) => first - prev);
    }
    private static IEnumerable<int[]> ReadFile(string filename)
    {
        return from line in File.ReadAllLines(filename)
               select (from num in line.Split(' ') select Int32.Parse(num)).ToArray();
    }
    public static long SumExtrapolateNext(string filename)
    {
        return (from reading in ReadFile(filename) select ExtrapolateNext(reading)).Sum();
    }
    public static long SumExtrapolatePrevious(string filename)
    {
        return (from reading in ReadFile(filename) select ExtrapolatePrevious(reading)).Sum();
    }
}
