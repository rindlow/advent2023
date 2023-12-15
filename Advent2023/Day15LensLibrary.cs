namespace Advent2023;
public static class Day15LensLibrary
{
    private static int Hash(string data)
    {
        Console.WriteLine($"Hash({data})");
        return data.Aggregate(0, (acc, c) => ((acc + c) * 17) % 256);
    }
    public static int HashFile(string filename)
    {
        var x = from line in File.ReadAllLines(filename) select (from step in line.Split(',') select Hash(step)).Sum();

        return x.Sum();
    }
}
