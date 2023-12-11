namespace Advent2023;

public class Day11CosmicExpansion_Test
{
    [Theory]
    [InlineData("testinput/day11.txt", 1, 374)]
    [InlineData("day11.txt", 1, 9647174)]
    public void TestPart1(string filename, int distance, int expected)
    {
        Assert.Equal(expected, Day11CosmicExpansion.SumDistances(filename, distance));
    }
    [Theory]
    [InlineData("testinput/day11.txt", 9, 1030L)]
    [InlineData("testinput/day11.txt", 99, 8410L)]
    [InlineData("day11.txt", 999999, 377318892554L)]
    public void TestPart2(string filename, int distance, long expected)
    {
        Assert.Equal(expected, Day11CosmicExpansion.SumDistances(filename, distance));
    }
}
