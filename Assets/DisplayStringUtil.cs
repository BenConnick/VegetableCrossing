using UnityEngine;

public static class DisplayStringUtil
{
    public static string ToPlayerName(this int id)
    {
        return $"Player {id+1}";
    }

    public static string ToPlayerNum(this int id)
    {
        return (id + 1).ToString();
    }

    public static string ToPlayerTag(this int id)
    {
        return $" (P{id+1})";
    }
}
