namespace Advent2023;

public class Day17ClumsyCrucible_Test
{
    [Theory]
    [InlineData("testinput/day17.txt", 102)]
    //[InlineData("day17.txt", 0)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day17ClumsyCrucible.MinimizeHeatLoss(filename));
    }

}
