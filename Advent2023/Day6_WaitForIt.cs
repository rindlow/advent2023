namespace Advent2023;

class Race(int duration, Int64 record)
{
    public int Duration = duration;
    public Int64 Record = record;
    private Int64 Distance(Int64 charge) {
        return (Duration - charge) * charge;
    }
    public int NumberOfWays() {
        return (from charge in Enumerable.Range(0, Duration) where Distance(charge) > Record select charge).Count();
    }
}
public static class Day6_WaitForIt
{
    public static int ProductOfNumberOfWays(string filename) {
        var lines = File.ReadAllLines(filename);
        var durations = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..];
        var records = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..];
        var races = durations.Zip(records, (duration, record) => new Race (Int32.Parse(duration), Int32.Parse(record)));
        return (from race in races select race.NumberOfWays()).Aggregate(1, (a, b) => a * b);
    }
    public static int ProductOfNumberOfWaysSingleRace(string filename) {
        var lines = File.ReadAllLines(filename);
        int duration = Int32.Parse(String.Concat(lines[0].Split(':')[1].Split(' ')));
        Int64 record = Int64.Parse(String.Concat(lines[1].Split(':')[1].Split(' ')));
        return new Race(duration, record).NumberOfWays();
    }
}
