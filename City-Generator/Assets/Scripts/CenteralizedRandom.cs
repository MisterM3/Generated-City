using UnityEngine;
using System.Collections.Generic;
public static class CenteralizedRandom
{
    private static int staticSeed = 0;

    public static void Init()
    {
        staticSeed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(staticSeed);
    }

    public static void Init(int seed)
    {
        staticSeed = seed;
        Random.InitState(seed);
    }

    public static float Range(float minInclusive, float maxInclusive)
    {
        return Random.Range(minInclusive, maxInclusive);
    }

    public static int Range(int minInclusive, int maxExclusive)
    {
        return Random.Range(minInclusive, maxExclusive);
    }

    public static bool CoinToss()
    {
        int coinSide = Random.Range(0, 2);
        return coinSide == 0;
    }

    public static T RandomizeEnum<T>() where T : struct, System.Enum
    {

        var enumValues = System.Enum.GetValues(typeof(T));

        int resultIntEnum = Random.Range(0, enumValues.Length);

        var test = System.Enum.GetName(typeof(T), resultIntEnum);
        var enumType = System.Enum.Parse(typeof(T), test);
        return (T)enumType;

    }

    public static T RandomItem<T>(this IList<T> list)
    {
        int index = Range(0, list.Count);
        return list[index];
    }

}
