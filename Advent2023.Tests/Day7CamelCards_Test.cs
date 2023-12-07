namespace Advent2023;

public class Day7CamelCards_Test
{
    [Theory]
    [InlineData("testinput/day7.txt", 6440)]
    [InlineData("day7.txt", 251216224)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day7CamelCards.TotalWinnings(filename));
    }
    [Theory]
    [InlineData("testinput/day7.txt", 5905)]
    [InlineData("day7.txt", 250825971)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day7CamelCards.TotalWinningsWithJokers(filename));
    }
}
