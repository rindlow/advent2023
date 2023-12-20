namespace Advent2023;

public class Day16TheFloorWillBeLava_Test
{
    [Theory]
    [InlineData("testinput/day16.txt", 46)]
    [InlineData("day16.txt", 7798)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day16TheFloorWillBeLava.EnergizedTiles(filename));
    }
    [Theory]
    [InlineData("testinput/day16.txt", 51)]
    [InlineData("day16.txt", 8026)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day16TheFloorWillBeLava.MaxEnergizedTiles(filename));
    }

}
