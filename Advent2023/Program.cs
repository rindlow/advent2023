using System.Diagnostics;

namespace Advent2023;

public class Day
{
    protected Lazy<string> Preload { get; set; }
    protected Lazy<string> Part1 { get; set; }
    protected Lazy<string> Part2 { get; set; }

    protected int No;

    public Day(int no, Lazy<string>preload, Lazy<string>part1, Lazy<string>part2)
    {
        this.No = no;
        this.Part1 = part1;
        this.Part2 = part2;
        this.Preload = preload;
    }

    private string FormatTime(Stopwatch stopwatch) {
        if (stopwatch.ElapsedMilliseconds > 0) {
           return $"{stopwatch.ElapsedMilliseconds} ms";
        } else {
            return $"{stopwatch.Elapsed.TotalMicroseconds} µs";
        }
    }

    public void Print()
    {
        Console.WriteLine($"* Day {this.No}");
        this.Preload.Value.ToString();
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Console.WriteLine($"  Part 1: {this.Part1.Value}");
        stopwatch.Stop();
        Console.WriteLine($"    Finished in {FormatTime(stopwatch)}");
        stopwatch.Reset();
        stopwatch.Start();
        Console.WriteLine($"  Part 2: {this.Part2.Value}");
        stopwatch.Stop();
        Console.WriteLine($"    Finished in {FormatTime(stopwatch)}"); 
        Console.WriteLine();    
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent 2020\n");
        Day[] Days = {
            new(1,
                new Lazy<string>(() => Day1_Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1_Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1_Trebuchet.SumCalibrationValuesWithText("input/day1.txt").ToString())),
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
    }
}
