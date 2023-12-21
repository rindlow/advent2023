namespace Advent2023;

public class Day21StepCounter_Test
{
    [Theory]
    [InlineData("testinput/day21.txt", 6, 16)]
    //[InlineData("day21.txt", 64, 0)]
    public void TestPart1(string filename, int steps, int expected)
    {
        Assert.Equal(expected, Day21StepCounter.ReachedInSteps(filename, steps));
    }

}
