namespace Advent2023;

public class Day5_IfYouGiveASeedAFertilizer_Test
{
    [Theory]
    [InlineData("testinput/day5.txt", 35)]
    [InlineData("day5.txt", 993500720)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day5IfYouGiveASeedAFertilizer.LowestLocationNumber(filename));
    }
    [Theory]
    [InlineData("testinput/day5.txt", 46)]
    [InlineData("day5.txt", 4917124)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day5IfYouGiveASeedAFertilizer.LowestLocationNumberRange(filename));
    }

}
