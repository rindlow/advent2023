namespace Advent2023;

public class Day20PulsePropagation_Test
{
    [Theory]
    [InlineData("testinput/day20a.txt", 32000000L)]
    [InlineData("testinput/day20b.txt", 11687500L)]
    [InlineData("day20.txt", 812721756L)]
    public void TestPart1(string filename, long expected)
    {
        Assert.Equal(expected, Day20PulsePropagation.HighTimesLow(filename));
    }
    [Theory]
    [InlineData("day20.txt", 0)]
    public void TestPart2(string filename, long expected)
    {
        Assert.Equal(expected, Day20PulsePropagation.PressesUntilRxLow(filename));
    }
}
