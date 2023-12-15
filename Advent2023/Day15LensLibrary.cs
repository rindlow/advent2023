using System.Reflection.Emit;

namespace Advent2023;

class Hash(string data)
{
    public int Value { get; } = data.Aggregate(0, (acc, c) => ((acc + c) * 17) % 256);
}

struct Lens 
{
    public string Label;
    public int FocalLength;

    public override string ToString()
    {
        return $"<Lens {Label}: {FocalLength}>";
    }
}

class LensMap
{
    Dictionary<int, List<Lens>> _map = [];
    

    public void Remove(string label)
    {
        int hash = new Hash(label).Value;
        if (_map.ContainsKey(hash))
        {
            IEnumerable<Lens> lenses = from l in _map[hash] where l.Label == label select l;
            if (lenses.Any())
            {
                _map[hash].Remove(lenses.First());
            }
            Console.WriteLine($"Sub({label}) hash = {hash} hashline = {String.Join(',', _map[hash])}");
        }
    }
    public void Add(Lens lens)
    {
        int hash = new Hash(lens.Label).Value;
        if (_map.TryGetValue(hash, out List<Lens>? value))
        {
            bool updated = false;
            List<Lens> lenses = [];
            foreach (int i in Enumerable.Range(0, value.Count))
            {
                if (value[i].Label == lens.Label)
                {
                    lenses.Add(new Lens() { Label = lens.Label, FocalLength = lens.FocalLength });
                    updated = true;
                }
                else
                {
                    lenses.Add(value[i]);
                }
            }
            if (!updated)
            {
                lenses.Add(lens);
            }
            _map[hash] = lenses;
        }
        else
        {
            _map[hash] = [lens];
        }
        Console.WriteLine($"Add({lens}) hash = {hash} hashline = {String.Join(',', _map[hash])}");
    }
    public int FocusingPower()
    {
        foreach (var kv in _map)
        {
            Console.WriteLine($"box {kv.Key} ({kv.Key + 1})");
            foreach (var (First, Second) in Enumerable.Range(0, kv.Value.Count).Zip(kv.Value))
            {
                Console.WriteLine($"{First} ({First + 1}) * {Second.FocalLength}");
            }
            Console.WriteLine();
        }
        return (from kv in _map 
                select (kv.Key + 1) * (from pair in Enumerable.Range(0, kv.Value.Count).Zip(kv.Value)
                                       select (pair.First + 1) * pair.Second.FocalLength).Sum()).Sum();
    }
}
public static class Day15LensLibrary
{
    private static IEnumerable<string> ReadFile(string filename)
    {
        return from line in File.ReadAllLines(filename)
               from step in line.Split(',') select step;
    }
    public static int HashFile(string filename)
    {
        return (from step in ReadFile(filename) 
                select new Hash(step).Value).Sum();
    }
    public static int FocusingPower(string filename)
    {
        LensMap map = new();
        foreach (string step in ReadFile(filename))
        {
            string label = step[..2];
            switch (step[2])
            {
                case '-':
                    map.Remove(label);
                    break;
                case '=':
                    map.Add(new Lens() {Label = label, FocalLength = step[3] - '0' });
                    break;
            }
        }
        return map.FocusingPower();
    }
}
