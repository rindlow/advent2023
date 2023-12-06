#!/bin/bash

if [ $# -lt 2 ]; then
    echo "USAGE: $0 dayno name"
    exit 1
fi
Project="Advent2023"
Module="Day$1$2"
testModule="${Module}_Test"
csFile="$Module.cs"
testFile="$testModule.cs"

cat - > $Project/$csFile << EOF
namespace $Project;
public static class $Module
{
}
EOF

cat - > $Project.Tests/$testFile << EOF
namespace $Project;

public class $testModule
{
    [Theory]
    [InlineData("testinput/day$1.txt", 0)]
    //[InlineData("day$1.txt", 0)]
    public void TestPart1(string filename, int expected)
    {
        Assert.Equal(expected, $Module.Part1(filename));
    }

}
EOF
