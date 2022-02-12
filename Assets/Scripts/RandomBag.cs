using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBag
{
    public static T RandomChoice<T>(List<T> bag)
    {
        return bag[Random.Range(0, bag.Count)];
    }

    public static T RandomChoice<T>(T[] bag)
    {
        return bag[Random.Range(0, bag.Length)];
    }

    public static void ShuffleBag<T>(ref List<T> bag)
    {
        for (int i = 0; i < bag.Count; i++)
        {
            int j = Random.Range(0, bag.Count);
            var tmp = bag[i];
            bag[i] = bag[j];
            bag[j] = tmp;
        }
    }

    public  static T[] SubArray<T>(T[] inputArray, int startIndex, int endIndex)
    {
        if (endIndex > inputArray.Length)
        {
            throw new System.Exception("SubArray Error");
        }

        int lengh = endIndex - startIndex + 1;

        T[] newArray = new T[lengh];

        for (int i = 0; i < lengh; i++)
        {
            newArray[i] = inputArray[i + startIndex];
        }
        return newArray;
    }

}
