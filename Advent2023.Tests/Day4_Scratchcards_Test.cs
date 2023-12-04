namespace Advent2023;

public class Day4_Scratchcards_Test
{
    [Theory]
    [InlineData("testinput/day4.txt", 13)]
    [InlineData("day4.txt", 26443)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day4_Scratchcards.SumPoints(filename));
    }

    [Theory]
    [InlineData("testinput/day4.txt", 30)]
    [InlineData("day4.txt", 6284877)]
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day4_Scratchcards.NumberOfCards(filename));
    }

}
