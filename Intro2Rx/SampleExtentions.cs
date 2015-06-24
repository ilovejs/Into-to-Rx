using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Intro2Rx
{
    public static class SampleExtentions
    {
        public static void Dump<T>(this IObservable<T> source, string name)
        {
            source.Subscribe(
            i => Console.WriteLine("{0}-->{1}", name, i),
            ex => Console.WriteLine("{0} failed-->{1}", name, ex.Message),
            () => Console.WriteLine("{0} completed", name));
        }

        /*
            var sum = source.Aggregate(0, (acc, currentValue) => acc + currentValue);
            var count = source.Aggregate(0, (acc, currentValue) => acc + 1);
    
            //or using '_' to signify that the value is not used.
            var count = source.Aggregate(0, (acc, _) => acc + 1);
         */
//        public static IObservable<TSource> Aggregate<TSource>(this IObservable<TSource> source,
//            Func<TSource, TSource, TSource> accumulator)
//        {
//
//        }

        public static IObservable<T> MyMin<T>(this IObservable<T> source)
        {
            return source.Aggregate(
                (min, current) => Comparer<T>
                                    .Default
                                    .Compare(min, current) > 0 ? current : min);
        }

        public static IObservable<T> MyMax<T>(this IObservable<T> source)
        {
            var comparer = Comparer<T>.Default;
            Func<T, T, T> max =
                (x, y) =>
                {
                    if (comparer.Compare(x, y) < 0)
                    {
                        return y;
                    }
                    return x;
                };
            return source.Aggregate(max);
        }
    }


}