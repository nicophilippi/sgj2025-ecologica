using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public static class Utils
{
    public static IEnumerable<(T el, int i)> WithIndex<T>(this IEnumerable<T> e)
    {
        var i = 0;
        foreach (var el in e) yield return (el, i++);
    }


    public static int? IndexOf<T>(this IEnumerable<T> e, T what)
    {
        foreach (var (el, i) in e.WithIndex())
        {
            if (el.Equals(what)) return i;
        }

        return null;
    }
    
    
    /// <summary>Disposes source afterwards</summary>
    public static IEnumerable<T> ToEnumerable<T>([NotNull] this IEnumerator<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        while (source.MoveNext()) yield return source.Current;
        source.Dispose();
    }
    
    
    public static T MinBy<T, C>([NotNull] this IEnumerable<T> source, C cSeed, T tSeed,
        [NotNull] Func<T, C> toComparable, out C resultsComparable) where C : IComparable<C>
     {
         if (source == null) throw new ArgumentNullException(nameof(source));
         if (toComparable == null) throw new ArgumentNullException(nameof(toComparable));
     
         var max = cSeed;
         var result = source.Aggregate(tSeed, (accu, n) =>
         {
             var nVal = toComparable(n);
             if (max.CompareTo(nVal) <= 0) return accu;
             max = nVal;
             return n;
         });
         resultsComparable = max;
         return result;
     }
 
 
     public static T MinBy<T, C>([NotNull] this IEnumerable<T> source, C cSeed, T tSeed, [NotNull] Func<T, C> toComparable)
         where C : IComparable<C> =>
         source.MinBy(cSeed, tSeed, toComparable, out _);
 
 
     public static T MinBy<T, C>([NotNull] this IEnumerable<T> source, [NotNull] Func<T, C> toComparable)
         where C : IComparable<C>
     {
         // Getting disposed by ToEnumerable()
         // ReSharper disable once GenericEnumeratorNotDisposed
         var etor = source.GetEnumerator();
         if (!etor.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");
         return etor.ToEnumerable().MinBy(toComparable(etor.Current), etor.Current, toComparable);
     }


     public static string NoBitches =>
         "\u2800\u28de\u28bd\u28aa\u28a3\u28a3\u28a3\u28ab\u287a\u2875\u28dd\u286e\u28d7\u28b7\u28bd\u28bd\u28bd\u28ee\u2877\u287d\u28dc\u28dc\u28ae\u28ba\u28dc\u28b7\u28bd\u289d\u287d\u28dd\n\u2838\u2878\u281c\u2815\u2815\u2801\u2881\u2887\u288f\u28bd\u28ba\u28ea\u2873\u285d\u28ce\u28cf\u28af\u289e\u287f\u28df\u28f7\u28f3\u28af\u2877\u28fd\u28bd\u28af\u28f3\u28eb\u2807\n\u2800\u2800\u2880\u2880\u2884\u28ac\u28aa\u286a\u284e\u28c6\u2848\u281a\u281c\u2815\u2807\u2817\u281d\u2895\u28af\u28ab\u28de\u28ef\u28ff\u28fb\u287d\u28cf\u2897\u28d7\u280f\u2800\n\u2800\u282a\u286a\u286a\u28ea\u28aa\u28ba\u28b8\u28a2\u2893\u2886\u28a4\u2880\u2800\u2800\u2800\u2800\u2808\u288a\u289e\u287e\u28ff\u286f\u28cf\u28ae\u2837\u2801\u2800\u2800\n\u2800\u2800\u2800\u2808\u280a\u2806\u2843\u2815\u2895\u2887\u2887\u2887\u2887\u2887\u288f\u288e\u288e\u2886\u2884\u2800\u2891\u28fd\u28ff\u289d\u2832\u2809\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u287f\u2802\u2820\u2800\u2847\u2887\u2815\u2888\u28c0\u2800\u2801\u2821\u2823\u2863\u286b\u28c2\u28ff\u282f\u28aa\u2830\u2802\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2866\u2859\u2842\u2880\u28a4\u28a3\u2823\u2848\u28fe\u2843\u2820\u2804\u2800\u2844\u28b1\u28cc\u28f6\u288f\u288a\u2802\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u289d\u2872\u28dc\u286e\u284f\u288e\u288c\u2882\u2819\u2822\u2810\u2880\u2898\u28b5\u28fd\u28ff\u287f\u2801\u2801\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2828\u28fa\u287a\u2855\u2855\u2871\u2851\u2846\u2855\u2845\u2855\u285c\u287c\u28bd\u287b\u280f\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u28fc\u28f3\u28eb\u28fe\u28f5\u28d7\u2875\u2871\u2861\u28a3\u2891\u2895\u289c\u2895\u285d\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u28f4\u28ff\u28fe\u28ff\u28ff\u28ff\u287f\u287d\u2851\u288c\u282a\u2862\u2863\u28e3\u285f\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u285f\u287e\u28ff\u28bf\u28bf\u28b5\u28fd\u28fe\u28fc\u28d8\u28b8\u28b8\u28de\u285f\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2801\u2807\u2821\u2829\u286b\u28bf\u28dd\u287b\u286e\u28d2\u28bd\u280b\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800";
}