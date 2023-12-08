namespace Advent2023;

public class Day8HauntedWasteland_Test
{
    [Theory]
    [InlineData("testinput/day8.txt", 6)]
    [InlineData("day8.txt", 0)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day8HauntedWasteland.NumberOfSteps(filename));
    }

}
