namespace Advent2023;

sealed class DesertMap
{
    public string Instructions;
    public Dictionary<string, (string, string)> Network;
    public DesertMap(string filename)
    {
        var lines = File.ReadAllLines(filename);
        Instructions = lines[0];
        Network = new(from line in lines[2..]
                      select new KeyValuePair<string, (string, string)>(key: line[..3],
                                                                        value: (line[7..10], line[12..15])));
    }
    public string Next(string node, char instruction)
    {
        var element = Network[node];
        if (instruction == 'L')
        {
            return element.Item1;
        }
        else
        {
            return element.Item2;
        }
    }
    public int LoopLength(string node)
    {
        int steps = 0;
        while (true)
        {
            foreach (char instruction in Instructions)
            {
                steps++;
                node = Next(node, instruction);
                if (node[2] == 'Z')
                {
                    return steps;
                }
            }
        }
    }
}
public static class Day8HauntedWasteland
{
    public static int NumberOfSteps(string filename)
    {
        DesertMap map = new(filename);
        int steps = 0;
        string current = "AAA";
        while (current != "ZZZ")
        {
            foreach (char instruction in map.Instructions)
            {
                steps++;
                current = map.Next(current, instruction);
                if (current == "ZZZ")
                {
                    return steps;
                }
            }
        }
        return steps;
    }
    public static long NumberOfStepsParallel(string filename)
    {
        DesertMap map = new(filename);
        return (from node in map.Network.Keys
                where node[2] == 'A'
                select map.LoopLength(node))
               .Aggregate(1L, (multiple, loop) => Mathematics.Lcm(loop, multiple));
    }
}
