namespace Advent2023;

sealed class Condition
{
    string _row;
    int[] _groups;
    static int nConditions;
    public Condition(string line)
    {
        Debug(0, "##################################################");
        var spaceSplit = line.Split(' ');
        _row = spaceSplit[0];
        _groups = [.. from num in spaceSplit[1].Split(',')
                      select Int32.Parse(num)];
        nConditions++;        
    }
    private static void Debug(int indent, string line)
    {
        return;
        Console.Write(String.Concat(from i in Enumerable.Range(0, indent) select "  "));
        Console.WriteLine(line);
    }
    public static int Possible(string row, int[] groups, int indent)
    {
        
        Debug(indent, $"Possible('{row}', [{String.Join(',', groups)}])");
        if (row.Length < groups.Sum() + groups.Length - 1)
        {
            Debug(indent, "remaining row shorter than groups, returning 0");
            return 0;
        }
        if (row.Length == 0 && groups.Length == 0)
        {
            Debug(indent, "both row and group empty, returning 1");
            return 1;
        }
        if (row[0] == '.')
        {
            Debug(indent, "looking at '.'");
            return Possible(row[1..], groups, indent + 1);
        }
        if (row[0] == '#')
        {
            Debug(indent, "looking at '#'");
            if (groups.Length == 0)
            {
                Debug(indent, "no groups, returning 0");
                return 0;
            }
            for (int i = 0; i < groups[0]; i++)
            {
                if (row[i] == '.')
                {
                    Debug(indent, "found '.', returning 0");
                    return 0;
                }
            }
            if (row.Length > groups[0] && row[groups[0]] == '#')
            {
                Debug(indent, "found '#' after group, returning 0");
                return 0;
            }
            if (row.Length == groups[0] && groups.Length == 1)
            {
                Debug(indent, "end of row, returning 1");
                return 1;
            }
            if (groups.Length > 0)
            {
                Debug(indent, "recursing");
                return Possible(row[(groups[0] + 1)..], groups[1..], indent + 1);
            }
            Debug(indent, "end, returning 1");
            return 1;
        }
        if (row[0] == '?')
        {
            Debug(indent, "looking at '?'");
            int nPoss = 0;
            // Assume #
            if (groups.Length != 0)
            {
                Debug(indent, "groups.Length != 0");
                bool dotFound = false;
                for (int i = 0; i < groups[0]; i++)
                {
                    if (row[i] == '.')
                    {
                        dotFound = true;
                        break;
                    }
                }
                if (dotFound)
                {
                    Debug(indent, "found '.', adding 0");
                    nPoss += 0;
                }
                else if (row.Length > groups[0] && row[groups[0]] == '#')
                {
                    Debug(indent, "found '#' after group, adding 0");
                    nPoss += 0;
                }
                else if (row.Length == groups[0] && groups.Length == 1)
                {
                    Debug(indent, "end of row, adding 1");
                    nPoss += 1;
                }
                else if (groups.Length > 0)
                {
                    nPoss += Possible(row[(groups[0] + 1)..], groups[1..], indent + 1);
                }
            }
            Debug(indent, $"? after assuming # {nPoss}");

            // Assume .
            nPoss += Possible(row[1..], groups, indent + 1);
            Debug(indent, $"? returning {nPoss}");
            return nPoss;
        }
        Debug(indent, "end, returning 1");
        return 1;
    }
    private static IEnumerable<string> Possibilities(string row)
    {
        IEnumerable<string> poss = [""];
        foreach (char c in row)
        {
            if (c == '.' || c == '#') 
            {
                poss = from p in poss select p + c;
            }
            else
            {
                poss = (from p in poss select p + '.').Concat(from p in poss select p + '#');
            }
        }
        // foreach (string p in poss)
        // {
        //     Console.WriteLine(p);
        // }
        // Console.WriteLine();
        return poss;
    }
    private static bool Matches(string row, int[] groups)
    {
        var x = from q in row.Split('.', StringSplitOptions.RemoveEmptyEntries) select q.Length;
        return Enumerable.SequenceEqual(x, groups);
    }
    public static int Possible2(string row, int[] groups, int indent)
    {
        return (from perm in Possibilities(row) where Matches(perm, groups) select perm).Count();
    }
    public static long Possible3(string row, int[] groups, int indent)
    {
        Debug(indent, $"Possible3({row}, {String.Join(',', groups)})");
        if (groups.Length == 0)
        {
            Debug(indent, $"empty groups, returning");
            return row.Any(c => c == '#') ? 0 : 1;
        }
        int maxGroup = groups.Max();
        int maxIndex = Array.IndexOf(groups, maxGroup);
        Debug(indent, $"maxGroup = {maxGroup}, maxIndex = {maxIndex}");
        int run = 0;
        long possible = 0;
        for (int i = 0; i < row.Length; i++)
        {
            if (row[i] == '.')
            {
                run = 0;
            }
            else
            {
                run += 1;
                if (run >= maxGroup)
                {
                    int center = maxGroup;
                    int groupsLeft = groups[..maxIndex].Sum();
                    int groupsRight = groups[(maxIndex + 1)..].Sum();
                    int minLeft = groups[..maxIndex].Sum() + maxIndex;
                    int minRight = groupsRight + groups.Length - maxIndex - 1;
                    Debug(indent, $"i = {i} groupsLeft = {groupsLeft} groupsRight = {groupsRight}");
                    int leftLen = i + 1 - maxGroup;
                    Debug(indent, $"leftLen = {leftLen}");
                    if (leftLen > 0)
                    {
                        if (row[leftLen - 1] == '#')
                        {
                            Debug(indent, "# found at left edge");
                            continue;
                        }
                        leftLen--;
                        center++;
                    }
                    int rightStart = i + 1;
                    Debug(indent, $"rightStart = {rightStart}");
                    if (rightStart < row.Length)
                    {
                        if (row[rightStart] == '#')
                        {
                            Debug(indent, "# found at right edge");
                            continue;
                        }
                        rightStart++;
                        center++;
                    }
                    Debug(indent, $"{groupsLeft} -- {maxGroup} -- {groupsRight}");
                    Debug(indent, $"{leftLen} -- {center} -- {rightStart}");
                    string rowLeft = row[..leftLen];
                    string rowRight = row[rightStart..];
                    Debug(indent, $"'{rowLeft}': {groupsLeft}, '{rowRight}': {groupsRight}");

                    if ((groupsLeft == 0 && rowLeft.Any(c => c == '#')) || (groupsRight == 0 && rowRight.Any(c => c == '#')))
                    {
                        Debug(indent, $"# found in wrong place");
                        continue;
                    }
                    Debug(indent, $"Left:");
                    long possLeft = Possible3(rowLeft, groups[..maxIndex], indent + 1);
                    if (possLeft > 0)
                    {
                        Debug(indent, $"Right:");
                        long possRight = Possible3(rowRight, groups[(maxIndex + 1)..], indent + 1);
                        Debug(indent, $"possLeft = {possLeft}, possRight = {possRight}, product = {possLeft * possRight}");
                        possible += possLeft * possRight;
                    }
                    else
                    {
                        Debug(indent, "possLeft = 0, no action");
                    }
                }
            }
            
        }
        Debug(indent, $"returning {possible}\n");
        return possible;
    }
    public long PossibleArrangements()
    {
        long p = Possible(_row, _groups, 0);
        // int p = Possible3(_row, _groups, 0);
        // Debug(0, $"Possible returned {p}");
        // if (p != p2)
        // {
        //     Debug(0, $"{_row} {String.Join(',', _groups)}: mismatch {p} != {p2}");
        //     // foreach (string poss in Possibilities(_row))
        //     // {
        //     //     Console.WriteLine(poss);
        //     // }
        //     // Console.WriteLine();
        //     // Environment.Exit(1);
        // }
        return p;
    }
    public Condition Unfold()
    {
        _row = String.Join('?', Enumerable.Repeat(_row, 5));
        _groups = [.. _groups, .. _groups, .. _groups, .. _groups, .. _groups];
        // Console.WriteLine($"{nConditions} Unfold row = {_row}");
        // Console.WriteLine($"{nConditions} Unfold groups = {String.Join(',', _groups)}");
        return this;
    }
}
public static class Day12HotSprings
{
    public static Int128 SumPossibleArrangements(string filename)
    {
        return (from line in File.ReadAllLines(filename).AsParallel() select new Condition(line).PossibleArrangements()).Sum();
    }
    public static Int128 SumUnfoldedArrangements(string filename)
    {
        return (from line in File.ReadAllLines(filename).AsParallel() select new Condition(line).Unfold().PossibleArrangements()).Sum();
    }
}
