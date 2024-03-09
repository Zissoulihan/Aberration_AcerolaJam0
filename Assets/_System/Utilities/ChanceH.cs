using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Chance helper utility script
/// </summary>
public static class ChanceH
{
    public static bool ChanceRoll(float chance, float outOf = 100f)
    {
        return Random.Range(0f, outOf) <= chance;
    }

    public static T Choose<T>(List<T> possibleChoices)
    {
        if (possibleChoices is null || possibleChoices.Count == 0) {
            Debug.LogError("Tried to choose from an invalid list of choices!");
            return default(T);
        }
        return possibleChoices[Random.Range(0, possibleChoices.Count)];

    }
}
