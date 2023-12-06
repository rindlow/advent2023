namespace Advent2023;
public class Scratchcard
{
    public readonly int Number;
    public readonly IEnumerable<int> Winning;
    public readonly IEnumerable<int> Having;
    public readonly int WinningNumbers;
    public Scratchcard(string card)
    {
        var colonSplit = card.Split(':');
        Number = Int32.Parse(colonSplit[0][5..]);
        var parts = colonSplit[1].Split('|');
        Winning = from num in parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                  select Int32.Parse(num);
        Having = from num in parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                 select Int32.Parse(num);
        WinningNumbers = Winning.Intersect(Having).Count();
    }

    public int Score()
    {
        return (int)Math.Pow(2, WinningNumbers - 1);
    }
}
public static class Day4_Scratchcards
{
    public static int SumPoints(string filename)
    {
        return (from line in File.ReadAllLines(filename)
                select new Scratchcard(line).Score()).Sum();
    }
    public static int NumberOfCards(string filename)
    {
        Dictionary<int, int> numOfCards = [];
        foreach (string line in File.ReadAllLines(filename))
        {
            Scratchcard card = new(line);
            if (numOfCards.TryGetValue(card.Number, out int value))
            {
                numOfCards[card.Number] = value + 1;
            }
            else
            {
                numOfCards[card.Number] = 1;
            }
            for (int i = card.Number + 1; i < card.Number + card.WinningNumbers + 1; i++)
            {
                if (numOfCards.TryGetValue(i, out int xvalue))
                {
                    numOfCards[i] = xvalue + numOfCards[card.Number];
                }
                else
                {
                    numOfCards[i] = numOfCards[card.Number];
                }
            }
        }
        return (from kv in numOfCards select kv.Value).Sum();
    }
}
