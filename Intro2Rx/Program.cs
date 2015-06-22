using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Intro2Rx
{
    class Program
    {
        /**
         * AsyncSubject<T> is similar to the Replay and Behavior subjects in the way that it caches values, 
         * however it will only store the last value, and only publish it when the sequence is completed. 
         * 
         * The general usage of the AsyncSubject<T> is to only ever publish one value then immediately complete. 
         * This means that is becomes quite comparable to Task<T>.
         */
        static void Main(string[] args)
        {
            var subject = new AsyncSubject<string>();
            subject.OnNext("a");
            WriteSequenceToConsole(subject);
            subject.OnNext("b");
            subject.OnNext("c");
            //This make a huge difference.
            subject.OnCompleted();

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
