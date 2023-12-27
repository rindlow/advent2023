namespace AnsiColor;
public static class AnsiColor
{
    public static string Bold(this string str)
    {
        return $"\x1b[1m{str}\x1b[0m";
    }
    public static string Black(this string str)
    {
        return $"\x1b[30m{str}\x1b[0m";
    }
    public static string Red(this string str)
    {
        return $"\x1b[31m{str}\x1b[0m";
    }
    public static string Green(this string str)
    {
        return $"\x1b[32m{str}\x1b[0m";
    }
    public static string Yellow(this string str)
    {
        return $"\x1b[33m{str}\x1b[0m";
    }
    public static string Blue(this string str)
    {
        return $"\x1b[34m{str}\x1b[0m";
    }
    public static string Magenta(this string str)
    {
        return $"\x1b[35m{str}\x1b[0m";
    }
    public static string Cyan(this string str)
    {
        return $"\x1b[36m{str}\x1b[0m";
    }
    public static string White(this string str)
    {
        return $"\x1b[37m{str}\x1b[0m";
    }
}
