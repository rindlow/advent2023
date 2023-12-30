namespace Advent2023;
sealed class PatternNote(IEnumerable<string> rows)
{
    readonly string[] _rows = rows.ToArray();

    private static int Reflection(string[] rows)
    {
        foreach (int i in Enumerable.Range(0, rows.Length - 1))
        {
            int j = 0;
            while (rows[i - j] == rows[i + j + 1])
            {
                j++;
                if (i - j < 0 || i + j + 1 >= rows.Length)
                {
                    return i + 1;
                }
            }
        }
        return 0;
    }
    private static bool DiffOne(string a, string b)
    {
        return (from pair in a.Zip(b) where pair.First != pair.Second select pair.First).Count() == 1;
    }
    private static int ReflectionSmudge(string[] rows)
    {
        foreach (int i in Enumerable.Range(0, rows.Length - 1))
        {
            bool smudgeFound = false;
            int j = 0;
            bool isEqual = rows[i - j] == rows[i + j + 1];
            bool isDiffOne = DiffOne(rows[i - j], rows[i + j + 1]);
            if (isDiffOne)
            {
                smudgeFound = true;
            }
            while (isEqual || isDiffOne)
            {
                j++;
                if (i - j < 0 || i + j + 1 >= rows.Length)
                {
                    if (smudgeFound)
                    {
                        return i + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                isEqual = rows[i - j] == rows[i + j + 1];
                isDiffOne = DiffOne(rows[i - j], rows[i + j + 1]);
                if (isDiffOne)
                {
                    smudgeFound = true;
                }
            }
        }
        return 0;
    }
    private static string[] Transpose(string[] rows)
    {
        return (from i in Enumerable.Range(0, rows[0].Length)
                select String.Concat(from row in rows select row[i])).ToArray();
    }
    public int Summary()
    {
        int above = Reflection(_rows);
        if (above > 0)
        {
            return 100 * above;
        }
        return Reflection(Transpose(_rows));
    }
    public int SummarySmudge()
    {
        int above = ReflectionSmudge(_rows);
        if (above > 0)
        {
            return 100 * above;
        }
        int left = ReflectionSmudge(Transpose(_rows));
        if (left == 0)
        {
            Console.WriteLine("No reflection found!");
        }
        return left;
    }
}
public static class Day13PointOfIncidence
{
    private static List<PatternNote> ReadFile(string filename)
    {
        List<PatternNote> notes = [];
        List<string> lines = [];

        foreach (string line in File.ReadAllLines(filename))
        {
            if (line.Length == 0)
            {
                notes.Add(new PatternNote(lines));
                lines = [];
            }
            else
            {
                lines.Add(line);
            }
        }
        notes.Add(new PatternNote(lines));
        return notes;
    }
    public static int SumPatternNotes(string filename)
    {
        return (from note in ReadFile(filename) select note.Summary()).Sum();
    }
    public static int SumPatternNotesSmudge(string filename)
    {
        return (from note in ReadFile(filename) select note.SummarySmudge()).Sum();
    }
}
