namespace Advent2023;

public class Day6_WaitForIt_Test
{
    [Theory]
    [InlineData("testinput/day6.txt", 288)]
    [InlineData("day6.txt", 281600)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day6WaitForIt.ProductOfNumberOfWays(filename));
    }

    [Theory]
    [InlineData("testinput/day6.txt", 71503)]
    [InlineData("day6.txt", 33875953)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day6WaitForIt.ProductOfNumberOfWaysSingleRace(filename));
    }

}
