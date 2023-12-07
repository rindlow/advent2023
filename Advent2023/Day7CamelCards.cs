namespace Advent2023;

internal sealed class Hand
{
    public string Cards { get; }
    public int Bid { get; }

    public Hand(string line)
    {
        var split = line.Split(' ');
        Cards = split[0];
        Bid = Int32.Parse(split[1]);
    }
    public int Strength()
    {
        Dictionary<char, int> numCards = [];
        foreach (char card in Cards)
        {
            if (numCards.TryGetValue(card, out int value))
            {
                numCards[card]++;
            }
            else
            {
                numCards[card] = 1;
            }
        }
        return String.Concat((from kv in numCards
                              select $"{kv.Value}").OrderDescending()) switch
        {
            "5" => 6,
            "41" => 5,
            "32" => 4,
            "311" => 3,
            "221" => 2,
            "2111" => 1,
            _ => 0
        };
    }
    public int StrengthWithJokers()
    {
        Dictionary<char, int> numCards = [];
        foreach (char card in Cards)
        {
            if (numCards.TryGetValue(card, out int value))
            {
                numCards[card]++;
            }
            else
            {
                numCards[card] = 1;
            }
        }
        numCards.Remove('J');
        return String.Concat((from kv in numCards
                              select $"{kv.Value}").OrderDescending()) switch
        {
            "5" or "4" or "3" or "2" or "1" or "" => 6,
            "41" or "31" or "21" or "11" => 5,
            "32" or "22" => 4,
            "311" or "211" or "111" => 3,
            "221" => 2,
            "2111" or "1111" => 1,
            _ => 0
        };
    }
    private static int CardValue(char c)
    {
        return c switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            _ => c - '0',
        };
    }
    private static int CardValueWithJokers(char c)
    {
        return c switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 1,
            'T' => 10,
            _ => c - '0',
        };
    }
    public int SortValue()
    {
        return Cards.Aggregate(0, (value, c) => 100 * value + CardValue(c));
    }
    public int SortValueWithJokers()
    {
        return Cards.Aggregate(0, (value, c) => 100 * value + CardValueWithJokers(c));
    }
}
public static class Day7CamelCards
{
    public static int TotalWinnings(string filename)
    {
        var hands = (from line in File.ReadAllLines(filename)
                     select new Hand(line))
                    .OrderBy(h => h.Strength())
                    .ThenBy(h => h.SortValue()).ToArray();
        return (from tuple in hands.Zip(Enumerable.Range(1, hands.Length))
                select tuple.First.Bid * tuple.Second).Sum();
    }
    public static int TotalWinningsWithJokers(string filename)
    {
        var hands = (from line in File.ReadAllLines(filename)
                     select new Hand(line))
                    .OrderBy(h => h.StrengthWithJokers())
                    .ThenBy(h => h.SortValueWithJokers()).ToArray();
        return (from tuple in hands.Zip(Enumerable.Range(1, hands.Length))
                select tuple.First.Bid * tuple.Second).Sum();
    }
}
