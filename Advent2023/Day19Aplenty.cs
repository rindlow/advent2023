namespace Advent2023;
using CatDict = Dictionary<Category, int>;
enum Category
{
    X, M, A, S
}
enum Operator
{
    LessThan, GreaterThan
}
sealed class RatingRange
{
    public CatDict Start { get; } = [];
    public CatDict End { get; } = [];

    public RatingRange()
    {
        Start = new() { { Category.X, 1 }, { Category.M, 1 }, { Category.A, 1 }, { Category.S, 1 } };
        End = new() { { Category.X, 4001 }, { Category.M, 4001 }, { Category.A, 4001 }, { Category.S, 4001 } };
    }
    public RatingRange(CatDict start, CatDict end)
    {
        Start = start;
        End = end;
    }
    public RatingRange SplitOff(Category category, int value)
    {
        CatDict newStart = new(Start);
        CatDict newEnd = new(End);
        newStart[category] = value;
        End[category] = value;
        return new(newStart, newEnd);
    }
}
struct Split
{
    public RatingRange Action;
    public RatingRange? NoAction;
}
sealed class Comparasion(string comp)
{
    readonly Category _category = comp[0] switch
    {
        'x' => Category.X,
        'm' => Category.M,
        'a' => Category.A,
        's' => Category.S,
        _ => throw new NotSupportedException(),
    };
    readonly Operator _operator = comp[1] switch
    {
        '<' => Operator.LessThan,
        '>' => Operator.GreaterThan,
        _ => throw new NotSupportedException(),
    };
    readonly int _value = Int32.Parse(comp[2..]);
    public bool Matches(Rating rating)
    {
        int ratingValue = _category switch
        {
            Category.X => rating.X,
            Category.M => rating.M,
            Category.A => rating.A,
            Category.S => rating.S,
            _ => throw new NotSupportedException(),
        };
        return _operator switch
        {
            Operator.LessThan => ratingValue < _value,
            Operator.GreaterThan => ratingValue > _value,
            _ => throw new NotSupportedException(),
        };
    }
    public Split Split(RatingRange range)
    {
        RatingRange? splitoff = null;
        if (range.Start[_category] < _value && _value < range.End[_category])
        {
            splitoff = range.SplitOff(_category, _value);
            if (_operator == Operator.GreaterThan)
            {
                (range, splitoff) = (splitoff, range);
            }
        }
        return new() { Action = range, NoAction = splitoff };
    }
}
sealed class RangeAccept
{
    public string Action { get; }
    public List<Split> splits;
}
sealed class Rule
{
    Comparasion? _comparaison;
    string _action;
    public Rule(string rule)
    {
        if (rule.Contains(':'))
        {
            var split = rule.Split(':');
            _comparaison = new(split[0]);
            _action = split[1];
        }
        else
        {
            _action = rule;
        }
    }
    public string? Accept(Rating rating)
    {
        if (_comparaison is Comparasion comp)
        {
            if (!comp.Matches(rating))
            {
                return null;
            }
        }
        return _action;
    }
    public IEnumerable<RatingRange> SplitRanges(IEnumerable<RatingRange> ranges)
    {
        if (_comparaison is Comparasion comp)
        {
            foreach (RatingRange range in ranges)
            {

            }
        }
        return ranges;
    }
}
sealed class Workflow
{
    public string Name { get; }
    List<Rule> _rules = [];
    public Workflow(string workflow)
    {
        var split = workflow.Split('{');
        Name = split[0];
        foreach (string rule in split[1][..^1].Split(','))
        {
            _rules.Add(new Rule(rule));
        }
    }
    public string Process(Rating rating)
    {
        foreach (Rule rule in _rules)
        {
            if (rule.Accept(rating) is string action)
            {
                return action;
            }
        }
        return "R";
    }
    public IEnumerable<RatingRange> ProcessRange(RatingRange range)
    {
        IEnumerable<RatingRange> ranges = [range];
        foreach (Rule rule in _rules)
        {
            ranges = rule.SplitRanges(ranges);
        }
        return ranges;
    }
}
sealed class Rating
{
    public int X { get; }
    public int M { get; }
    public int A { get; }
    public int S { get; }
    public Rating(string rating)
    {
        var split = rating[1..^1].Split(',');
        X = Int32.Parse(split[0][2..]);
        M = Int32.Parse(split[1][2..]);
        A = Int32.Parse(split[2][2..]);
        S = Int32.Parse(split[3][2..]);
    }
    public int Sum()
    {
        return X + M + A + S;
    }
}

sealed class PartRatings
{
    public Dictionary<string, Workflow> Workflows { get; } = [];
    public List<Rating> Ratings { get; } = [];
    public PartRatings(string filename)
    {
        bool read_workflow = true;
        foreach (string line in File.ReadAllLines(filename))
        {
            if (line.Length == 0)
            {
                read_workflow = false;
                continue;
            }
            if (read_workflow)
            {
                Workflow workflow = new(line);
                Workflows[workflow.Name] = workflow;
            }
            else
            {
                Ratings.Add(new(line));
            }
        }
    }
}
public static class Day19Aplenty
{
    public static int SumRating(string filename)
    {
        PartRatings partRatings = new(filename);
        int accepted = 0;
        foreach (Rating rating in partRatings.Ratings)
        {
            string state = "in";
            while (state != "A" && state != "R")
            {
                state = partRatings.Workflows[state].Process(rating);
            }
            if (state == "A")
            {
                accepted += rating.Sum();
            }
        }
        return accepted;
    }
    public static long DistinctCombinations(string filename)
    {
        PartRatings partRatings = new(filename);
        RatingRange range = new();
        IEnumerable<RatingRange> result = partRatings.Workflows["in"].ProcessRange(range);
        return 0;
    }
}
