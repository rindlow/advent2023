namespace Advent2023;

public class Day3_GearRatios_Test
{
    [Theory]
    [InlineData("testinput/day3.txt", 4361)]
    [InlineData("day3.txt", 527144)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day3GearRatios.SumOfPartNumbers(filename));
    }

    [Theory]
    [InlineData("testinput/day3.txt", 467835)]
    [InlineData("day3.txt", 81463996)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day3GearRatios.SumOfGearRatios(filename));
    }

}
