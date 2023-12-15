namespace Advent2023;

public class Day13PointOfIncidence_Test
{
    [Theory]
    [InlineData("testinput/day13.txt", 405)]
    [InlineData("day13.txt", 33780)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day13PointOfIncidence.SumPatternNotes(filename));
    }
    [Theory]
    [InlineData("testinput/day13.txt", 400)]
    [InlineData("day13.txt", 0)] // < 33913
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day13PointOfIncidence.SumPatternNotesSmudge(filename));
    }

}
