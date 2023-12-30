namespace Advent2023;

public class Day24NeverTellMeTheOdds_Test
{
    [Theory]
    [InlineData("testinput/day24.txt", 7, 27, 2)]
    // [InlineData("day24.txt", 200000000000000L, 400000000000000L, 13892)]
    public void TestPart1(string filename, long minPos, long maxPos, int expected)
    {
        Assert.Equal(expected, Day24NeverTellMeTheOdds.NumIntersections(filename, minPos, maxPos));
    }
    [Theory]
    [InlineData("testinput/day24.txt", 47)]
    [InlineData("day24.txt", 13892)] // < 945374682319368
    public void TestPart2(string filename, long expected)
    {
        Assert.Equal(expected, Day24NeverTellMeTheOdds.SumCoordinate(filename));
    }
}
