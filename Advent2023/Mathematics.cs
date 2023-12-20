namespace Advent2023;

class Mathematics
{
    public static long Gcd(long a, long b)
    {
        if (b == 0)
        {
            return a;
        }
        return Gcd(b, a % b);
    }
    public static long Lcm(long a, long b)
    {
        return a * (b / Gcd(a, b));
    }
}