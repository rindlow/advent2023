namespace Advent2023;

public class Day14ParabolicReflectorDish_Test
{
    [Theory]
    [InlineData("testinput/day14.txt", 136)]
    [InlineData("day14.txt", 109665)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day14ParabolicReflectorDish.NorthSupportLoad(filename));
    }
    [Theory]
    [InlineData("testinput/day14.txt", 64)]
    [InlineData("day14.txt", 96061)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day14ParabolicReflectorDish.BillionLoad(filename));
    }
}
