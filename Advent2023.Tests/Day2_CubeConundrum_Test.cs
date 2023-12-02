namespace Advent2023;

public class Day2_CubeConundrum_Test
{
    [Theory]
    [InlineData("testinput/day2.txt", 12, 13, 14, 8)]
    [InlineData("day2.txt", 12, 13, 14, 2105)]
    public void TestPart1(string filename, int redCubes, int greenCubes, int blueCubes, int expected)
    {
        Assert.Equal(expected, Day2_CubeConundrum.SumPossibleGames(filename, redCubes, greenCubes, blueCubes));
    }

    [Theory]
    [InlineData("testinput/day2.txt", 2286)]
    [InlineData("day2.txt", 72422)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day2_CubeConundrum.SumPowers(filename));
    }
}
