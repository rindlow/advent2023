namespace Advent2023;

public class Day22SandSlabs_Test
{
    [Theory]
    [InlineData("testinput/day22.txt", 5)]
    [InlineData("day22.txt", 413)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day22SandSlabs.NumberOfDisintegratableBricks(filename));
    }
    [Theory]
    [InlineData("testinput/day22.txt", 7)]
    [InlineData("day22.txt", 413)] // > 2388
    public void TestPart2(string filename, int expected)
    {
        Assert.Equal(expected, Day22SandSlabs.SumOtherFalling(filename));
    }
}
