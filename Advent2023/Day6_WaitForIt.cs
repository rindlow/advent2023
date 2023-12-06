namespace Advent2023;

internal sealed class Race(int duration, Int64 record)
{
    public int Duration = duration;
    public Int64 Record = record;
    private bool BeatsRecord(Int64 charge)
    {
        return (Duration - charge) * charge > Record;
    }
    public int FindTransition(int left, int right)
    {
        bool leftState = BeatsRecord(left);
        while (right - left > 1)
        {
            int mid = (left + right) / 2;
            if (BeatsRecord(mid) == leftState)
            {
                left = mid;
            }
            else
            {
                right = mid;
            }
        }
        return right;
    }
    public int NumberOfWays()
    {
        int start = FindTransition(0, Duration / 2);
        int end = FindTransition(Duration / 2, Duration);
        return end - start;
    }
}
public static class Day6WaitForIt
{
    public static int ProductOfNumberOfWays(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var durations = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..];
        var records = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..];
        var races = durations.Zip(records, (duration, record) => new Race(Int32.Parse(duration), Int32.Parse(record)));
        return (from race in races select race.NumberOfWays()).Aggregate(1, (a, b) => a * b);
    }
    public static int ProductOfNumberOfWaysSingleRace(string filename)
    {
        var lines = File.ReadAllLines(filename);
        int duration = Int32.Parse(String.Concat(lines[0].Split(':')[1].Split(' ')));
        Int64 record = Int64.Parse(String.Concat(lines[1].Split(':')[1].Split(' ')));
        return new Race(duration, record).NumberOfWays();
    }
}
