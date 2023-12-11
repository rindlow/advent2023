namespace Advent2023;
public readonly struct Position(int row, int col) : IEquatable<Position>
{
    public readonly int Row { get; } = row;
    public readonly int Col { get; } = col;
    public override string ToString()
    {
        return $"<Position {Row}, {Col}>";
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        Position other = (Position)obj;
        return Row == other.Row && Col == other.Col;
    }
    public bool Equals(Position other)
    {
        return Row == other.Row && Col == other.Col;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return 10000 * Row + Col;
    }

    public static bool operator ==(Position left, Position right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Position left, Position right)
    {
        return !(left == right);
    }
}