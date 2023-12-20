namespace Advent2023;

public class Day19Aplenty_Test
{
    [Theory]
    [InlineData("testinput/day19.txt", 19114)]
    [InlineData("day19.txt", 495298)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day19Aplenty.SumRating(filename));
    }
    [Theory]
    [InlineData("testinput/day19.txt", 167409079868000L)]
    // [InlineData("day19.txt", 495298)]
    public void TestPart2(string filename, long expected)
    {
        Assert.Equal(expected, Day19Aplenty.DistinctCombinations(filename));
    }
}
