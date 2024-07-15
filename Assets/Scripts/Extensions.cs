using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    // Show all items in collection
    public static void ShowItems<T>(this IEnumerable<T> collection) {
        foreach (var item in collection) {
            Debug.Log(item);
        }
    }
}
