namespace Advent2023;

public class Day25Snowverload_Test
{
    [Theory]
    [InlineData("testinput/day25.txt", 54)]
    [InlineData("day25.txt", 600225)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, Day25Snowverload.MultiplyGroupSizes(filename));
    }

}
