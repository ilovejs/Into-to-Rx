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
            /*
             In the example the window was specified as 150 milliseconds. Values are published 100 milliseconds apart. 
             * 
             * Once we have subscribed to the subject, 
             * 
             * the first value is 200ms old and as such has expired and been removed from the cache.
             */
            var window = TimeSpan.FromMilliseconds(150);
            var subject = new ReplaySubject<string>(window);
            
            subject.OnNext("w");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            
            subject.OnNext("x");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            
            subject.OnNext("y");
            subject.Subscribe(Console.WriteLine);
            subject.OnNext("z");

            //w--x--yz is time sequence, w is out of date when subscribe to handler
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
