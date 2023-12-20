namespace Advent2023;
sealed class Platform(string filename)
{
    public char[][] Rows { get; } = (from line in File.ReadAllLines(filename)
                                     select line.ToArray()).ToArray();

    public override string ToString()
    {
        return String.Join('\n', from row in Rows select String.Concat(row));
    }

    public void RollNorth(int rowIndex)
    {
        foreach (int col in Enumerable.Range(0, Rows[rowIndex].Length))
        {
            if (Rows[rowIndex][col] == 'O')
            {
                int target = rowIndex;
                while (target > 0)
                {
                    if (Rows[target - 1][col] == '.')
                    {
                        target--;
                    }
                    else
                    {
                        break;
                    }
                }
                if (target != rowIndex)
                {
                    Rows[target][col] = 'O';
                    Rows[rowIndex][col] = '.';
                }
            }
        }
    }
    public void RollSouth(int rowIndex)
    {
        foreach (int col in Enumerable.Range(0, Rows[rowIndex].Length))
        {
            if (Rows[rowIndex][col] == 'O')
            {
                int target = rowIndex;
                while (target < Rows.Length - 1)
                {
                    if (Rows[target + 1][col] == '.')
                    {
                        target++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (target != rowIndex)
                {
                    Rows[target][col] = 'O';
                    Rows[rowIndex][col] = '.';
                }
            }
        }
    }
    public void RollEast(int colIndex)
    {
        foreach (int row in Enumerable.Range(0, Rows.Length))
        {
            if (Rows[row][colIndex] == 'O')
            {
                int target = colIndex;
                while (target < Rows[row].Length - 1)
                {
                    if (Rows[row][target + 1] == '.')
                    {
                        target++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (target != colIndex)
                {
                    Rows[row][target] = 'O';
                    Rows[row][colIndex] = '.';
                }
            }
        }
    }
    public void RollWest(int colIndex)
    {
        foreach (int row in Enumerable.Range(0, Rows.Length))
        {
            if (Rows[row][colIndex] == 'O')
            {
                int target = colIndex;
                while (target > 0)
                {
                    if (Rows[row][target - 1] == '.')
                    {
                        target--;
                    }
                    else
                    {
                        break;
                    }
                }
                if (target != colIndex)
                {
                    Rows[row][target] = 'O';
                    Rows[row][colIndex] = '.';
                }
            }
        }
    }
    public void Tilt()
    {
        foreach (int i in Enumerable.Range(1, Rows.Length - 1))
        {
            RollNorth(i);
        }
    }
    public void Cycle()
    {
        // Console.WriteLine($"{this}\nTilting north\n");
        foreach (int i in Enumerable.Range(1, Rows.Length - 1))
        {
            RollNorth(i);
        }
        // Console.WriteLine($"{this}\nTilting west\n");
        foreach (int i in Enumerable.Range(1, Rows.Length - 1))
        {
            RollWest(i);
        }
        // Console.WriteLine($"{this}\nTilting south\n");
        foreach (int i in Enumerable.Range(0, Rows.Length - 1).Reverse())
        {
            RollSouth(i);
        }
        // Console.WriteLine($"{this}\nTilting east\n");
        foreach (int i in Enumerable.Range(0, Rows[0].Length - 1).Reverse())
        {
            RollEast(i);
        }

    }
    public int Load()
    {
        int load = 0;
        foreach (var (First, Second) in Rows.Zip(Enumerable.Range(1, Rows.Length).Reverse()))
        {
            int count = (from c in First where c == 'O' select c).Count();
            load += count * Second;
        }
        return load;
    }
}
public static class Day14ParabolicReflectorDish
{
    public static int NorthSupportLoad(string filename)
    {
        Platform platform = new(filename);
        platform.Tilt();
        return platform.Load();
    }
    public static int BillionLoad(string filename)
    {
        Platform platform = new(filename);
        Dictionary<string, int> seen = [];
        int i = 0;
        while (!seen.ContainsKey(platform.ToString()))
        {
            seen.Add(platform.ToString(), i);
            platform.Cycle();
            i++;
        }
        int mod = i - seen[platform.ToString()];
        for (int j = i % mod; j < 1000000000L % mod; j++)
        {
            platform.Cycle();
        }
        return platform.Load();
    }
}
