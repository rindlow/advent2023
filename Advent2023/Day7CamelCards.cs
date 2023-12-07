using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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
        if (numCards.ContainsValue(5))
        {
            return 6;
        }
        if (numCards.ContainsValue(4))
        {
            return 5;
        }
        if (numCards.ContainsValue(3))
        {
            if (numCards.ContainsValue(2))
            {
                return 4;
            }
            {
                return 3;
            }
        }
        return (from kv in numCards where kv.Value == 2 select kv.Value).Count();
    }
    private static int CardValue(char c)
    {
        switch (c)
        {
            case 'A': return 14;
            case 'K': return 13;
            case 'Q': return 12;
            case 'J': return 11;
            case 'T': return 10;
            default: return c - '0';
        }
    }
    public int SortValue()
    {
        int value = 0;
        foreach (char c in Cards)
        {
            value *= 100;
            value += CardValue(c);
        }
        return value;
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
        int numJokers = 0;
        if (numCards.TryGetValue('J', out int numJ))
        {
            numJokers = numJ;
            numCards.Remove('J');
        }

        if (numCards.ContainsValue(5) || numJokers >= 4)
        {
            return 6;
        }
        if (numCards.ContainsValue(4))
        {
            return 5 + numJokers;
        }
        if (numCards.ContainsValue(3))
        {
            if (numCards.ContainsValue(2))
            {
                return 4;
            }
            else if (numJokers > 0)
            {
                return 4 + numJokers;
            }
            else
            {
                return 3;
            }
        }
        int pairs = (from kv in numCards where kv.Value == 2 select kv.Value).Count();
        if (pairs == 2)
        {
            if (numJokers == 1)
            {
                return 4;
            }
            else
            {
                return 2;
            }
        }
        if (pairs == 1)
        {
            if (numJokers == 3)
            {
                return 6;
            }
            if (numJokers == 2)
            {
                return 5;
            }
            if (numJokers == 1)
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }
        if (numJokers == 3)
        {
            return 5;
        }
        if (numJokers == 2)
        {
            return 3;
        }
        if (numJokers == 1)
        {
            return 1;
        }
        else
        {
            return 0;
        }

    }
    public int SortValueWithJokers()
    {
        int value = 0;
        foreach (char c in Cards)
        {
            value *= 100;
            value += CardValueWithJokers(c);
        }
        return value;
    }
    private static int CardValueWithJokers(char c)
    {
        switch (c)
        {
            case 'A': return 14;
            case 'K': return 13;
            case 'Q': return 12;
            case 'J': return 1;
            case 'T': return 10;
            default: return c - '0';
        }
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
        int winnings = 0;
        for (int i = 0; i < hands.Length; i++)
        {
            winnings += hands[i].Bid * (i + 1);
        }
        return winnings;
    }
    public static int TotalWinningsWithJokers(string filename)
    {
        var hands = (from line in File.ReadAllLines(filename)
                     select new Hand(line))
                    .OrderBy(h => h.StrengthWithJokers())
                    .ThenBy(h => h.SortValueWithJokers()).ToArray();
        int winnings = 0;
        for (int i = 0; i < hands.Length; i++)
        {
            winnings += hands[i].Bid * (i + 1);
        }
        return winnings;
    }
}
