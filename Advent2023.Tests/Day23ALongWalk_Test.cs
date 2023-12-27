namespace Advent2023;

public class Day23ALongWalk_Test
{
    [Theory]
    [InlineData("testinput/day23.txt", 94)]
    [InlineData("day23.txt", 2414)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day23ALongWalk.LongestHike(filename));
    }
    [Theory]
    [InlineData("testinput/day23.txt", 154)]
    [InlineData("day23.txt", 6598)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day23ALongWalk.LongestDryHike(filename));
    }
}
