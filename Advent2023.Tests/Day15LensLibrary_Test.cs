namespace Advent2023;

public class Day15LensLibrary_Test
{
    [Theory]
    [InlineData("testinput/day15.txt", 1320)]
    [InlineData("day15.txt", 0)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day15LensLibrary.HashFile(filename));
    }

}
