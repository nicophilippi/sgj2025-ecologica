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
}