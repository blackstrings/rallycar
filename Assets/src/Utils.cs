using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class Utils
{
    /// <summary>
	/// Shuffles a list of items
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="ts"></param>
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
