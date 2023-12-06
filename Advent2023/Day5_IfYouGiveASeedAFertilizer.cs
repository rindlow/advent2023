namespace Advent2023;
public class Range {
    public Int64 Start;
    public Int64 Length;
    public Range Overlap(Range other) {
        if (Start <= other.Start && other.Start < Start + Length) {
            return new Range() { Start = other.Start, Length = Math.Min(Start + Length, other.Start + other.Length) - other.Start };
        }
        if (other.Start <= Start && Start < other.Start + other.Length) {
            return new Range() { Start = Start, Length = Math.Min(Start + Length, other.Start + other.Length) - Start };
        }
        return new Range();
    }
    public List<Range> Subtract(Range other) {
        if (Start < other.Start && other.Start < Start + Length && Start + Length < other.Start + other.Length) {
            return [new Range() { Start = Start, Length = other.Start - Start } ];
        }
        if (Start < other.Start && other.Start + other.Length < Start + Length) {
            return [
                new Range() { Start = Start, Length = other.Start - Start },
                new Range() { Start = other.Start + other.Length, Length = Start + Length - (other.Start + other.Length)} ];
        }
        if (other.Start <= Start && Start < other.Start + other.Length && other.Start + other.Length < Start + Length) {
            return [new Range() { Start = other.Start + other.Length, Length = Start + Length - (other.Start + other.Length)}];
        }
        return [];
    }
    public Range Translate(Int64 diff) {
        Start += diff;
        return this;
    }
    public override string ToString() {
        return $"<Range {Start} +{Length} ({Start} - {Start + Length})>";
    }
}
public class SeedMapEntry {
    public readonly Int64 DestinationStart;
    public readonly Range Source;
    public SeedMapEntry(string line) {
        var numbers = line.Split(' ');
        DestinationStart = Int64.Parse(numbers[0]);
        Source = new Range() { Start = Int64.Parse(numbers[1]), Length = Int64.Parse(numbers[2]) };
    }
}
public class SeedMap {
    public readonly string Source;
    public readonly string Destination;
    public readonly IEnumerable<SeedMapEntry> Entries;
    public SeedMap(string[] lines) {
        string[] header = lines[0][..^5].Split('-');
        Source = header[0];
        Destination = header[2];
        Entries = from line in lines[1..] select new SeedMapEntry(line);
    }
    private Ranges MapEntries(Range inRange) {
        Ranges mapped = new ([]);
        Ranges ranges = new ([]);
        ranges.Add(inRange);
        foreach (SeedMapEntry entry in Entries) {
            Ranges unmapped = new ([]);
            foreach (Range range in ranges.ranges) {
                Range overlap = range.Overlap(entry.Source);
                if (overlap.Length > 0) {
                    mapped.Add(overlap.Translate(entry.DestinationStart - entry.Source.Start));
                    foreach (Range rest in range.Subtract(entry.Source)) {
                        unmapped.Add(rest);
                    }
                }
                else {
                    unmapped.Add(range);
                }
            }
            ranges = unmapped;
        }
        foreach (Range range in mapped.ranges) {
            ranges.Add(range);
        }
        return ranges;
    }
    public Ranges Map(Ranges ranges) {
        Ranges ret = new ([]);
        foreach(Range range in ranges.ranges) {
            foreach(Range r in MapEntries(range).ranges) {
                ret.Add(r);
            }
        }
        return ret;
    }
}

public class Ranges {
    public List<Range> ranges;
    public Ranges(IEnumerable<Range> initial) {
        ranges = initial.ToList();
    }
    public void Add(Range range) {
        ranges.Add(range);
    }
    public Int64 Lowest() {
        return (from range in ranges select range.Start).Min();
    }
    public override string ToString() {
        return $"<Ranges {String.Join(' ', ranges)}>";
    }
}
struct Having {
    public Ranges Number;
    public string Kind;
}
class Almanac {
    public readonly Int64[] Seeds;
    readonly List<SeedMap> Maps;

    public Almanac(string filename) {
        Maps = [];
        string[] lines = File.ReadAllLines(filename);
        Seeds = (from seed in lines[0].Split(": ")[1].Split(' ') select Int64.Parse(seed)).ToArray();
        int start = 0;
        for (int i = 2; i < lines.Length; i++) {
            if (lines[i].Contains(':')) {
                start = i;
            } else if (lines[i] == "") {
                Maps.Add(new SeedMap(lines[start..i]));
            }
        }
        Maps.Add(new SeedMap(lines[start..]));
    }
    public Having Corresponds(Having having) {
        foreach (SeedMap map in Maps) {
            if (map.Source == having.Kind) {
                return new Having() { Number = map.Map(having.Number), Kind = map.Destination };
            }
        }
        throw new Exception($"Unknown map source '{having.Kind}'");
    }
    public Ranges SeedsToLocations(Having current) {
        while (current.Kind != "location") {
            current = Corresponds(current);
        }
        return current.Number;
    }
    public Having SingleSeeds() {
        return new() { 
            Number = new Ranges(from seed in Seeds select new Range() { Start = seed, Length = 1 }), 
            Kind = "seed" 
        };
    }
    public Having RangeSeeds() {
        Ranges ranges = new ([]);
        for (int i = 0; i < Seeds.Length; i += 2) {
            ranges.Add(new Range() { Start = Seeds[i], Length = Seeds[i + 1] });
        }
        return new() { Number = ranges, Kind = "seed" };
    }
}
public static class Day5_IfYouGiveASeedAFertilizer
{
    public static Int64 LowestLocationNumber(string filename) {
        Almanac almanac = new(filename);
        return almanac.SeedsToLocations(almanac.SingleSeeds()).Lowest();
    }
    public static Int64 LowestLocationNumberRange(string filename) {
        Almanac almanac = new(filename);
        return almanac.SeedsToLocations(almanac.RangeSeeds()).Lowest();
    }
}
