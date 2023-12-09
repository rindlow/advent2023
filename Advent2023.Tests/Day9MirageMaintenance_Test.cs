namespace Advent2023;

public class Day9MirageMaintenance_Test
{
    [Theory]
    [InlineData("testinput/day9.txt", 114L)]
    [InlineData("day9.txt", 2175229206L)]
    public void TestPart1(string filename, long expected)
    {
        Assert.Equal(expected, Day9MirageMaintenance.SumExtrapolateNext(filename));
    }
    [Theory]
    [InlineData("testinput/day9.txt", 2L)]
    [InlineData("day9.txt", 942L)]
    public void TestPart2(string filename, long expected)
    {
        Assert.Equal(expected, Day9MirageMaintenance.SumExtrapolatePrevious(filename));
    }
}
