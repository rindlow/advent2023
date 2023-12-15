namespace Advent2023;

public class Day15LensLibrary_Test
{
    [Theory]
    [InlineData("testinput/day15.txt", 1320)]
    [InlineData("day15.txt", 510388)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day15LensLibrary.HashFile(filename));
    }
    [Theory]
    [InlineData("testinput/day15.txt", 145)]
    [InlineData("day15.txt", 291774)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day15LensLibrary.FocusingPower(filename));
    }
}
