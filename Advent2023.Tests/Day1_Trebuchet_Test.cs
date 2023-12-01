namespace Advent2023;

public class Day1_Trebuchet_Test
{
    [Theory]
    [InlineData("testinput/day1.txt", 142)]
    [InlineData("day1.txt", 55621)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, actual: Day1_Trebuchet.SumCalibrationValues(filename));
    }
    [Theory]
    [InlineData("testinput/day1b.txt", 281)]
    [InlineData("day1.txt", 53592)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, actual: Day1_Trebuchet.SumCalibrationValuesWithText(filename));
    }
}
  