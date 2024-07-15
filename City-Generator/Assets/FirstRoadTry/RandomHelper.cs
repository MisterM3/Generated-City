using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomHelper
{
    
    public static bool CoinToss() 
    {
        int randomInt = Random.Range(0, 2);

        if (randomInt == 0)
        {
            return true;
        }
        else return false;
    }

    public static bool Percentage(float percentage)
    {
        float randomInt = Random.Range(0f, 1f);

        if (randomInt < percentage)
        {
            return true;
        }
        else return false;
    }
}
