using System.Reflection;
using System.Text;

namespace Advent2023;
enum ModuleType
{
    Broadcaster,
    Flipflop,
    Conjunction,
}
enum PulseType
{
    Low,
    High,
}
sealed class Pulse
{
    public PulseType Type { get; }
    public string Source { get; }
    public string Destination { get; }
    static Dictionary<PulseType, int> _sent = [];
    public Pulse(PulseType type, string source, string destination)
    {
        Type = type;
        Source = source;
        Destination = destination;
        _sent[type]++;
    }
    public override string ToString()
    {
        return $"{Source} -{Type}-> {Destination}";
    }
    public static void ResetCounter()
    {
        _sent = new() { { PulseType.High, 0 }, { PulseType.Low, 0 } };
    }
    public static long CounterProduct()
    {
        return _sent[PulseType.Low] * _sent[PulseType.High];
    }
}
sealed class PulseModule
{
    public string Name { get; }
    public string[] Destinations { get; }
    readonly ModuleType _moduleType;
    readonly Dictionary<string, PulseType> _inputs = [];
    bool _isOn;
    readonly List<string> _watchList = ["db", "gr", "vc", "lz"];
    readonly static Dictionary<string, long> _lastLow = [];

    public PulseModule(string line)
    {
        var split = line.Split(" -> ");
        switch (split[0][0])
        {
            case '%':
                _moduleType = ModuleType.Flipflop;
                Name = split[0][1..];
                _isOn = false;
                break;
            case '&':
                _moduleType = ModuleType.Conjunction;
                Name = split[0][1..];
                break;
            default:
                _moduleType = ModuleType.Broadcaster;
                Name = split[0];
                break;
        }
        Destinations = split[1].Split(", ");
        foreach (string watch in _watchList)
        {
            _lastLow[watch] = 0;
        }
    }
    private PulseType Flip()
    {
        _isOn = !_isOn;
        if (_isOn)
        {
            return PulseType.High;
        }
        return PulseType.Low;
    }
    public void AddInput(string input)
    {
        _inputs[input] = PulseType.Low;
    }
    public IEnumerable<Pulse> Forward(Pulse pulse, int buttonPress)
    {
        if (_moduleType == ModuleType.Broadcaster)
        {
            return from destination in Destinations select new Pulse(pulse.Type, Name, destination);
        }
        if (_moduleType == ModuleType.Flipflop && pulse.Type == PulseType.Low)
        {
            PulseType output = Flip();
            return from destination in Destinations select new Pulse(output, Name, destination);
        }
        if (_moduleType == ModuleType.Conjunction)
        {
            _inputs[pulse.Source] = pulse.Type;
            if (_inputs.Values.All(p => p == PulseType.High))
            {
                if (buttonPress > 0 && _watchList.Contains(Name))
                {
                    // Console.WriteLine($"{buttonPress}: {Name} emitting Low (last {_lastLow[Name]}, n = {buttonPress - _lastLow[Name]})");
                    _lastLow[Name] = buttonPress;
                }
                return from destination in Destinations select new Pulse(PulseType.Low, Name, destination);
            }
            return from destination in Destinations select new Pulse(PulseType.High, Name, destination);
        }
        return [];
    }
    public string GraphViz()
    {
        StringBuilder stringBuilder = new();
        string shape = _moduleType switch
        {
            ModuleType.Broadcaster => "box",
            ModuleType.Flipflop => "invtriangle",
            ModuleType.Conjunction => "ellipse",
            _ => throw new NotImplementedException(),
        };
        stringBuilder.AppendLine($"  {Name} [shape={shape}]");
        foreach (string dest in Destinations)
        {
            stringBuilder.AppendLine($"  {Name} -> {dest}");
        }
        return stringBuilder.ToString();
    }
    public static long CycleLength()
    {
        return _lastLow.Values.Aggregate(1L, Mathematics.Lcm);
    }
}
sealed class PulseNetwork
{
    Dictionary<string, PulseModule> _modules = [];
    public PulseNetwork(string filename)
    {
        // _modules["output"] = new PulseModule();
        foreach (string line in File.ReadAllLines(filename))
        {
            PulseModule newModule = new(line);
            _modules[newModule.Name] = newModule;
        }
        foreach (PulseModule module in _modules.Values)
        {
            foreach (string destination in module.Destinations)
            {
                if (_modules.TryGetValue(destination, out PulseModule? pulseModule))
                {
                    pulseModule.AddInput(module.Name);
                }
            }
        }
    }
    public bool PressButton(int buttonPress = 0)
    {
        Queue<Pulse> queue = new();
        queue.Enqueue(new Pulse(PulseType.Low, "button", "broadcaster"));
        while (queue.Count > 0)
        {
            Pulse current = queue.Dequeue();
            // Console.WriteLine(current);
            if (!_modules.ContainsKey(current.Destination))
            {
                // Console.WriteLine($"ignoring unknown module {current.Destination} {current.Type}");
                if (current.Destination == "rx" && current.Type == PulseType.Low)
                {
                    return true;
                }
                continue;
            }
            foreach (Pulse p in _modules[current.Destination].Forward(current, buttonPress))
            {
                queue.Enqueue(p);
            }
        }
        return false;
    }
    public void GraphViz()
    {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine("digraph G {");
        foreach (PulseModule pulseModule in _modules.Values)
        {
            stringBuilder.Append(pulseModule.GraphViz());
        }
        stringBuilder.AppendLine("}");
        File.WriteAllText("day20.gv", stringBuilder.ToString());
    }
}
public static class Day20PulsePropagation
{
    public static long HighTimesLow(string filename)
    {
        Pulse.ResetCounter();
        PulseNetwork pulseNetwork = new(filename);
        foreach (int i in Enumerable.Range(0, 1000))
        {
            pulseNetwork.PressButton();
        }
        return Pulse.CounterProduct();
    }
    public static long PressesUntilRxLow(string filename)
    {
        Pulse.ResetCounter();
        PulseNetwork pulseNetwork = new(filename);
        foreach (int i in Enumerable.Range(1, 5000))
        {
            pulseNetwork.PressButton(i);
        }
        return PulseModule.CycleLength();
    }
}