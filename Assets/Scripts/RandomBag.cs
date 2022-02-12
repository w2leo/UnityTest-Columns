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
}
