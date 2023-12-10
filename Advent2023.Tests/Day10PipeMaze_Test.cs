namespace Advent2023;

public class Day10PipeMaze_Test
{
    [Theory]
    [InlineData("testinput/day10.txt", 4)]
    [InlineData("day10.txt", 6725)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day10PipeMaze.FarthestTile(filename));
    }
    [Theory]
    [InlineData("testinput/day10b.txt", 4)]
    [InlineData("day10.txt", 383)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day10PipeMaze.InsideArea(filename));
    }

}
