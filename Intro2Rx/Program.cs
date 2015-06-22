using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace Intro2Rx
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Tips:
             * 
             * Subject class implement IObserver and IObeservable
             * 
               public sealed class Subject<T> : ISubject<T>, ISubject<T, T>, IObserver<T>, IObservable<T>, IDisposable
             */
            var subject = new Subject<string>();
            WriteSequenceToConsole(subject);

            subject.OnNext("a");
            subject.OnNext("b");
            subject.OnNext("c");
            Console.ReadKey();
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
