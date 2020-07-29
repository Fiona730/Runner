using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void Add<T>(ref T[] array, T element)
    {
        Expand(ref array, 1);
        array[array.Length - 1] = element;
    }

    public static void Add<T>(ref T[] array, T[] elements)
    {
        int oriLength = array.Length;
        Expand(ref array, elements.Length);
        Array.Copy(elements, 0, array, oriLength, elements.Length);
    }

    public static void Expand<T>(ref T[] array, int length)
    {
        Array.Resize(ref array, array.Length + length);
    }

    public static void Sort(ref Scene.hexNum[] array)
    {
        Scene.hexNum tmp;
        for (int i=1; i<=array.Length-1; i++)
            for (int j=0; j<array.Length-i; j++)
        {
            if(array[j]>array[j+1])
            {
                tmp = array[j];
                array[j] = array[j+1];
                array[j+1] = tmp;
            }
        }
    }
}
