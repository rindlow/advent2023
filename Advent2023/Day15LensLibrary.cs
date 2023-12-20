using System.Reflection.Emit;

namespace Advent2023;

sealed class Hash(string data)
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

sealed class LensMap
{
    Dictionary<int, List<Lens>> _map = [];


    public void Remove(string label)
    {
        int hash = new Hash(label).Value;
        if (_map.TryGetValue(hash, out List<Lens>? value))
        {
            IEnumerable<Lens> lenses = from l in value where l.Label == label select l;
            if (lenses.Any())
            {
                value.Remove(lenses.First());
            }
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
    }
    public int FocusingPower()
    {
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
               from step in line.Split(',')
               select step;
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
            if (step.Contains('-'))
            {
                map.Remove(step[..step.IndexOf('-')]);
            }
            else
            {
                var split = step.Split('=');
                map.Add(new Lens() { Label = split[0], FocalLength = Int32.Parse(split[1]) });
            }
        }
        return map.FocusingPower();
    }
}
