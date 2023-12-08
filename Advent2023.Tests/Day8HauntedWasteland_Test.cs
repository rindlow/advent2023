namespace Advent2023;

public class Day8HauntedWasteland_Test
{
    [Theory]
    [InlineData("testinput/day8.txt", 6)]
    [InlineData("day8.txt", 17621)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day8HauntedWasteland.NumberOfSteps(filename));
    }
    [Theory]
    [InlineData("testinput/day8b.txt", 6L)]
    [InlineData("day8.txt", 20685524831999L)]
    public void TestPart2(string filename, long expected)
    {
        Assert.Equal(expected, Day8HauntedWasteland.NumberOfStepsParallel(filename));
    }
}
