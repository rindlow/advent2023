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
            foreach(char instruction in map.Instructions)
            {
                steps++;
                var element = map.Network[current];
                Console.WriteLine($"current = {current}, instruction = {instruction}, element = {element}");
                if (instruction == 'L')
                {
                    current = element.Item1;
                }
                else
                {
                    current = element.Item2;
                }
                if (current == "ZZZ")
                {
                    return steps;
                }
            }
        }
        return steps;
    }
}
