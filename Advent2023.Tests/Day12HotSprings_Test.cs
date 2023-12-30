namespace Advent2023;

public class Day12HotSprings_Test
{
    [Theory]
    [InlineData("testinput/day12.txt", 21)]
    [InlineData("day12.txt", 8022)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day12HotSprings.SumPossibleArrangements(filename));
    }
    [Theory]
    [InlineData("testinput/day12.txt", 525152)]
    [InlineData("day12.txt", 4968620679637L)]
    public void TestPart2(string filename, long expected)
    {
        Assert.Equal(expected, Day12HotSprings.SumUnfoldedArrangements(filename));
    }
}
