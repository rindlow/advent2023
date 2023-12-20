namespace Advent2023;

public class Day18LavaductLagoon_Test
{
    [Theory]
    [InlineData("testinput/day18.txt", 62)]
    [InlineData("day18.txt", 0)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day18LavaductLagoon.Volume(filename));
    }

}
