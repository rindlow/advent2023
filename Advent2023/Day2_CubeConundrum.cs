using System.Runtime.Intrinsics.X86;

namespace Advent2023;

struct SubSet {
    public int redCubes;
    public int greenCubes;
    public int blueCubes;
}
class Game
{
    public int gameId;
    public List<SubSet> subSets;

    public Game(string line) {
        var colonSplit = line.Split(':');
        gameId = Int32.Parse(colonSplit[0][5..]);
        subSets = [];
        foreach (var subsetString in colonSplit[1].Split(';')) {
            SubSet subset = new();
            foreach (var cubes in subsetString.Split(',')) {
                if (cubes.EndsWith("red")) {
                    subset.redCubes = Int32.Parse(cubes[1..^4]);
                }
                if (cubes.EndsWith("green")) {
                    subset.greenCubes = Int32.Parse(cubes[1..^6]);
                }
                if (cubes.EndsWith("blue")) {
                    subset.blueCubes = Int32.Parse(cubes[1..^5]);
                }
            }
            subSets.Add(subset);
        }
    }

    public bool PossibleWith(int redCubes, int greenCubes, int blueCubes) {
        return subSets.All(subset => subset.redCubes <= redCubes && subset.greenCubes <= greenCubes && subset.blueCubes <= blueCubes);
    }
    public int Power() {
        return subSets.Select(x => x.redCubes).Max()
            * subSets.Select(x => x.greenCubes).Max()
            * subSets.Select(x => x.blueCubes).Max();
    }
}
public static class Day2_CubeConundrum
{
    private static IEnumerable<Game> ReadFile(string filename) => 
        from line in File.ReadAllLines(filename) select (new Game(line));
    public static int SumPossibleGames(string filename, int redCubes, int greenCubes, int blueCubes) => 
        (from game in ReadFile(filename)
            where game.PossibleWith(redCubes, greenCubes, blueCubes)
            select game.gameId).Sum();
    public static int SumPowers(string filename) => 
        (from game in ReadFile(filename)
            select game.Power()).Sum();
}
