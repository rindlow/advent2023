namespace Advent2023;

struct CacheKey
{
    public int Index;
    public int GroupLen;
}

sealed class Condition
{
    string _row;
    int[] _groups;
    Dictionary<CacheKey, long> _cache;
    public Condition(string line)
    {
        var spaceSplit = line.Split(' ');
        _row = spaceSplit[0];
        _groups = [.. from num in spaceSplit[1].Split(',')
                      select Int32.Parse(num)];
        _cache = new();
    }


    public long Possible(string row, int[] groups, int startPos)
    {
        CacheKey key = new() { Index = startPos, GroupLen = groups.Length };
        if (_cache.TryGetValue(key, out long cached))
        {
            return cached;
        }
        if (groups.Length == 0)
        {
            if (startPos <= row.Length)
            {
                return row[startPos..].Contains('#') ? 0 : 1;
            }
            return 1;
        }
        long possible = 0;
        for (int pos = startPos; pos < row.Length; pos++)
        {
            if (row[pos] == '.')
            {
                continue;
            }
            if (pos + groups[0] <= row.Length
                && !row[pos..(pos + groups[0])].Contains('.')
                && (pos + groups[0] == row.Length
                    || row[pos + groups[0]] != '#'))
            {
                possible += Possible(row, groups[1..], pos + groups[0] + 1);
            }
            if (row[pos] == '#')
            {
                break;
            }
        }
        _cache[key] = possible;
        return possible;
    }
    public long PossibleArrangements()
    {
        return  Possible(_row, _groups, 0);
    }
    public Condition Unfold()
    {
        _row = String.Join('?', Enumerable.Repeat(_row, 5));
        _groups = Enumerable.Repeat(_groups, 5).SelectMany(e => e).ToArray();
        return this;
    }
}
public static class Day12HotSprings
{
    public static Int128 SumPossibleArrangements(string filename)
    {
        return (from line in File.ReadAllLines(filename) 
                select new Condition(line).PossibleArrangements()).Sum();
    }
    public static Int128 SumUnfoldedArrangements(string filename)
    {
        return (from line in File.ReadAllLines(filename)
                select new Condition(line).Unfold().PossibleArrangements()).Sum();
    }
}
