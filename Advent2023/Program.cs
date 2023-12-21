using System.Diagnostics;

namespace Advent2023;

public class Day(int no, Lazy<string> preload, Lazy<string> part1, Lazy<string> part2)
{
    protected Lazy<string> Preload { get; set; } = preload;
    protected Lazy<string> Part1 { get; set; } = part1;
    protected Lazy<string> Part2 { get; set; } = part2;

    private readonly int _no = no;
    public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;

    public static string FormatTime(TimeSpan elapsed)
    {
        if (elapsed.TotalMilliseconds > 1)
        {
            return $"{elapsed.TotalMilliseconds:F1} ms";
        }
        else
        {
            return $"{elapsed.TotalMicroseconds:F0} µs";
        }
    }

    public void Print()
    {
        Console.WriteLine($"* Day {_no}");
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

internal sealed class Program
{
    static void Main()
    {
        Console.WriteLine("Advent 2023\n");
        Day[] Days = [
            new(1,
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValuesWithText("input/day1.txt").ToString())),
            new(2,
                new Lazy<string>(() => Day2CubeConundrum.SumPossibleGames("input/day2.txt", 12, 13, 14).ToString()),
                new Lazy<string>(() => Day2CubeConundrum.SumPossibleGames("input/day2.txt", 12, 13, 14).ToString()),
                new Lazy<string>(() => Day2CubeConundrum.SumPowers("input/day2.txt").ToString())),
            new(3,
                new Lazy<string>(() => Day3GearRatios.SumOfPartNumbers("input/day3.txt").ToString()),
                new Lazy<string>(() => Day3GearRatios.SumOfPartNumbers("input/day3.txt").ToString()),
                new Lazy<string>(() => Day3GearRatios.SumOfGearRatios("input/day3.txt").ToString())),
            new(4,
                new Lazy<string>(() => Day4Scratchcards.SumPoints("input/day4.txt").ToString()),
                new Lazy<string>(() => Day4Scratchcards.SumPoints("input/day4.txt").ToString()),
                new Lazy<string>(() => Day4Scratchcards.NumberOfCards("input/day4.txt").ToString())),
            new(5,
                new Lazy<string>(() => Day5IfYouGiveASeedAFertilizer.LowestLocationNumber("input/day5.txt").ToString()),
                new Lazy<string>(() => Day5IfYouGiveASeedAFertilizer.LowestLocationNumber("input/day5.txt").ToString()),
                new Lazy<string>(() => Day5IfYouGiveASeedAFertilizer.LowestLocationNumberRange("input/day5.txt").ToString())),
            new(6,
                new Lazy<string>(() => Day6WaitForIt.ProductOfNumberOfWays("input/day6.txt").ToString()),
                new Lazy<string>(() => Day6WaitForIt.ProductOfNumberOfWays("input/day6.txt").ToString()),
                new Lazy<string>(() => Day6WaitForIt.ProductOfNumberOfWaysSingleRace("input/day6.txt").ToString())),
            new(7,
                new Lazy<string>(() => Day7CamelCards.TotalWinnings("input/day7.txt").ToString()),
                new Lazy<string>(() => Day7CamelCards.TotalWinnings("input/day7.txt").ToString()),
                new Lazy<string>(() => Day7CamelCards.TotalWinningsWithJokers("input/day7.txt").ToString())),
            new(8,
                new Lazy<string>(() => Day8HauntedWasteland.NumberOfSteps("input/day8.txt").ToString()),
                new Lazy<string>(() => Day8HauntedWasteland.NumberOfSteps("input/day8.txt").ToString()),
                new Lazy<string>(() => Day8HauntedWasteland.NumberOfStepsParallel("input/day8.txt").ToString())),
            new(9,
                new Lazy<string>(() => Day9MirageMaintenance.SumExtrapolateNext("input/day9.txt").ToString()),
                new Lazy<string>(() => Day9MirageMaintenance.SumExtrapolateNext("input/day9.txt").ToString()),
                new Lazy<string>(() => Day9MirageMaintenance.SumExtrapolatePrevious("input/day9.txt").ToString())),
            new(10,
                new Lazy<string>(() => Day10PipeMaze.FarthestTile("input/day10.txt").ToString()),
                new Lazy<string>(() => Day10PipeMaze.FarthestTile("input/day10.txt").ToString()),
                new Lazy<string>(() => Day10PipeMaze.InsideArea("input/day10.txt").ToString())),
            new(11,
                new Lazy<string>(() => Day11CosmicExpansion.SumDistances("input/day11.txt", 1).ToString()),
                new Lazy<string>(() => Day11CosmicExpansion.SumDistances("input/day11.txt", 1).ToString()),
                new Lazy<string>(() => Day11CosmicExpansion.SumDistances("input/day11.txt", 999999).ToString())),
            new(12,
                new Lazy<string>(() => Day12HotSprings.SumPossibleArrangements("input/day12.txt").ToString()),
                new Lazy<string>(() => Day12HotSprings.SumPossibleArrangements("input/day12.txt").ToString()),
                new Lazy<string>(() => Day12HotSprings.SumPossibleArrangements("input/day12.txt").ToString())),
            new(13,
                new Lazy<string>(() => Day13PointOfIncidence.SumPatternNotes("input/day13.txt").ToString()),
                new Lazy<string>(() => Day13PointOfIncidence.SumPatternNotes("input/day13.txt").ToString()),
                new Lazy<string>(() => Day13PointOfIncidence.SumPatternNotes("input/day13.txt").ToString())),
            new(14,
                new Lazy<string>(() => Day14ParabolicReflectorDish.NorthSupportLoad("input/day14.txt").ToString()),
                new Lazy<string>(() => Day14ParabolicReflectorDish.NorthSupportLoad("input/day14.txt").ToString()),
                new Lazy<string>(() => Day14ParabolicReflectorDish.BillionLoad("input/day14.txt").ToString())),
            new(15,
                new Lazy<string>(() => Day15LensLibrary.HashFile("input/day15.txt").ToString()),
                new Lazy<string>(() => Day15LensLibrary.HashFile("input/day15.txt").ToString()),
                new Lazy<string>(() => Day15LensLibrary.FocusingPower("input/day15.txt").ToString())),
            new(16,
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValuesWithText("input/day1.txt").ToString())),
            new(17,
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValuesWithText("input/day1.txt").ToString())),
            new(18,
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValuesWithText("input/day1.txt").ToString())),
            new(19,
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValuesWithText("input/day1.txt").ToString())),
            new(20,
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValues("input/day1.txt").ToString()),
                new Lazy<string>(() => Day1Trebuchet.SumCalibrationValuesWithText("input/day1.txt").ToString())),
            new(21,
                new Lazy<string>(() => Day21StepCounter.ReachedInSteps("input/day21.txt", 6).ToString()),
                new Lazy<string>(() => Day21StepCounter.ReachedInSteps("Advent2023.Tests/testinput/day21.txt", 6).ToString()),
                new Lazy<string>(() => Day21StepCounter.ReachedInSteps("input/day21.txt", 6).ToString())),
        ];
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
        foreach (Day day in Days)
        {
            totalTime += day.TotalTime;
        }
        Console.WriteLine($"Total time: {Day.FormatTime(totalTime)}");
    }
}
