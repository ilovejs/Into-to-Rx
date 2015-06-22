using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Intro2Rx
{
    class Program
    {
        static void Main(string[] args)
        {
            var values = new Subject<int>();
            
            var firstSubscription = values.Subscribe(value =>
            Console.WriteLine("1st subscription received {0}", value));

            var secondSubscription = values.Subscribe(value =>
            Console.WriteLine("2nd subscription received {0}", value));
            
            values.OnNext(0);
            values.OnNext(1);
            values.OnNext(2);
            values.OnNext(3);

            firstSubscription.Dispose();
            Console.WriteLine("Disposed of 1st subscription");
            
            values.OnNext(4);
            values.OnNext(5);
        }

        //Takes an IObservable<string> as its parameter. 
        //Subject<string> implements this interface.
        static void WriteSequenceToConsole(IObservable<string> sequence)
        {
            //The next two lines are equivalent.
            //sequence.Subscribe(value=>Console.WriteLine(value));
            //Extension Method doc: https://msdn.microsoft.com/en-us/library/system.observableextensions(v=VS.103).aspx
            sequence.Subscribe(Console.WriteLine);
        }
    }
}
