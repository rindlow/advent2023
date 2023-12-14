namespace Advent2023;
class PatternNote
{
    string[] _rows;
    public PatternNote(IEnumerable<string> rows)
    {
        _rows = rows.ToArray();
    }
    private static int Reflection(string[] rows)
    {
        Console.WriteLine($"Reflection\n {String.Join("\n ", rows)}");
        foreach (int i in Enumerable.Range(0, rows.Length - 1))
        {
            int j = 0;
            Console.WriteLine($"i = {i}, j = {j} comparing rows {i - j} and {i + j + 1}");
            Console.WriteLine($"{rows[i - j]}\n{rows[i + j + 1]}");
            while (rows[i - j] == rows[i + j + 1])
            {
                j++;
                if (i - j < 0 || i + j + 1 >= rows.Length)
                {
                    Console.WriteLine($"bounds reached, returning {i + 1}");
                    return i + 1;
                }
                Console.WriteLine($"i = {i}, j = {j} comparing rows {i - j} and {i + j + 1}");
                Console.WriteLine($"{rows[i - j]}\n{rows[i + j + 1]}");
            }
        }
        Console.WriteLine($"end reached, returning 0");
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
        Console.WriteLine($"above = {above}");
        if (above > 0)
        {
            return 100 * above;
        }
        return Reflection(Transpose(_rows));
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
}
