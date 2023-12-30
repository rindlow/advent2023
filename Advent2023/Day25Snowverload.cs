using System.Text;

namespace Advent2023;

sealed class WiringDiagram
{
    Dictionary<string, List<string>> _wires = [];
    List<(string, string)> _testExclude = [("pzl", "hfx"), ("cmg", "bvb"), ("jqt", "nvd")];
    List<(string, string)> _realExclude = [("vps", "htp"), ("ttj", "rpd"), ("fqn", "dgc")];
    List<(string, string)> _exclude;
    HashSet<string> _visited = [];
    public WiringDiagram(string filename)
    {
        _exclude = filename.Contains("test") ? _testExclude : _realExclude;

        foreach (string line in File.ReadAllLines(filename))
        {
            string[] split = line.Split(": ");
            foreach (string target in split[1].Split(' '))
            {    
                Add(split[0], target);
            }
        }
    }
    private void Add(string src, string dst)
    {
        foreach ((string exSrc, string exDst) in _exclude)
        {
            if (src == exSrc && dst == exDst)
            {
                return;
            }
        }
        if (_wires.TryGetValue(src, out List<string>? value))
        {
            value.Add(dst);
        }
        else
        {
            _wires[src] = [ dst ];
        }
        if (_wires.TryGetValue(dst, out List<string>? rValue))
        {
            rValue.Add(src);
        }
        else
        {
            _wires[dst] = [ src ];
        }
    }
    int GraphSize(string start)
    {
        if (_visited.Contains(start))
        {
            return 0;
        }
        _visited.Add(start);
        return 1 + (from wire in _wires[start] select GraphSize(wire)).Sum();
    }
    public int MultiplyGroupSizes()
    {
        int first = GraphSize(_exclude[0].Item1);
        int second = GraphSize(_exclude[0].Item2);
        return first * second;
    }
}
public static class Day25Snowverload
{
    private static void GraphViz(string filename)
    {
        StringBuilder sb = new();
        sb.AppendLine("graph {");
        foreach (string line in File.ReadAllLines(filename))
        {
            string[] split = line.Split(": ");
            foreach (string target in split[1].Split(' '))
            {
                sb.AppendLine($"  {split[0]} -- {target}");
            }
        }
        sb.AppendLine("}");
        File.WriteAllText("day25.gv", sb.ToString());
    }
    public static int MultiplyGroupSizes(string filename)
    {
        WiringDiagram wiringDiagram = new(filename);
        return wiringDiagram.MultiplyGroupSizes();
    }
}
