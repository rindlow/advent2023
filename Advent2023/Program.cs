using System.Diagnostics;

namespace Advent2023;

public class Day(int no, Lazy<string> preload, Lazy<string> part1, Lazy<string> part2)
{
    protected Lazy<string> Preload { get; set; } = preload;
    protected Lazy<string> Part1 { get; set; } = part1;
    protected Lazy<string> Part2 { get; set; } = part2;

    protected int No = no;
    public TimeSpan TotalTime = TimeSpan.Zero;

    public static string FormatTime(TimeSpan elapsed) {
        if (elapsed.TotalMilliseconds > 1) {
           return $"{elapsed.TotalMilliseconds:F1} ms";
        } else {
            return $"{elapsed.TotalMicroseconds:F0} µs";
        }
    }

    public void Print()
    {
        Console.WriteLine($"* Day {No}");
        Preload.Value.ToString();
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Console.WriteLine($"  Part 1: {Part1.Value}");
        stopwatch.Stop();
        TotalTime += stopwatch.Elapsed;
        Console.WriteLine($"    Finished in {FormatTime(stopwatch.Elapsed)}");
        stopwatch.Reset();
        stopwatch.Start();
        Console.WriteLine($"  Part 2: {Part2.Value}");
        stopwatch.Stop();
        TotalTime += stopwatch.Elapsed;
        Console.WriteLine($"    Finished in {FormatTime(stopwatch.Elapsed)}"); 
        Console.WriteLine();    
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent 2023\n");
        Day[] Days = {
            new(1,
                new Lazy<string>(() => Day1_Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1_Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1_Trebuchet.SumCalibrationValuesWithText("input/day1.txt").ToString())),
            new(2,
                new Lazy<string>(() => Day2_CubeConundrum.SumPossibleGames("input/day2.txt", 12, 13, 14).ToString()),
                new Lazy<string>(() => Day2_CubeConundrum.SumPossibleGames("input/day2.txt", 12, 13, 14).ToString()),
                new Lazy<string>(() => Day2_CubeConundrum.SumPowers("input/day2.txt").ToString())),
            new(3,
                new Lazy<string>(() => Day3_GearRatios.SumOfPartNumbers("input/day3.txt").ToString()),
                new Lazy<string>(() => Day3_GearRatios.SumOfPartNumbers("input/day3.txt").ToString()),
                new Lazy<string>(() => Day3_GearRatios.SumOfGearRatios("input/day3.txt").ToString())),
            new(4,
                new Lazy<string>(() => Day4_Scratchcards.SumPoints("input/day4.txt").ToString()),
                new Lazy<string>(() => Day4_Scratchcards.SumPoints("input/day4.txt").ToString()),
                new Lazy<string>(() => Day4_Scratchcards.NumberOfCards("input/day4.txt").ToString())),     
            new(5,
                new Lazy<string>(() => Day5_IfYouGiveASeedAFertilizer.LowestLocationNumber("input/day5.txt").ToString()),
                new Lazy<string>(() => Day5_IfYouGiveASeedAFertilizer.LowestLocationNumber("input/day5.txt").ToString()),
                new Lazy<string>(() => Day5_IfYouGiveASeedAFertilizer.LowestLocationNumberRange("input/day5.txt").ToString())),
        };

        string[] argv = System.Environment.GetCommandLineArgs();
        if (argv.Length < 2)
        {
            for (int i = 0; i < Days.Length; i++)
            {
                Days[i]?.Print();
            }
        }
        else
        {
            for (int i = 0; i < argv.Length; i++)
            {
                if (int.TryParse(argv[i], out int day) && day > 0 && day < 26 && Days[day - 1] != null)
                {
                    Days[day - 1].Print();
                }
            }
        }
        TimeSpan totalTime = TimeSpan.Zero;
        foreach (Day day in Days) {
            totalTime += day.TotalTime;
        }
        Console.WriteLine($"Total time: {Day.FormatTime(totalTime)}");
    }
}
