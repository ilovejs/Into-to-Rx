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
            var subject = new Subject<string>();
            subject.Subscribe(Console.WriteLine);
            subject.OnNext("a");
            subject.OnNext("b");
            subject.OnCompleted();
            //once a sequence is completed, no more activity can happen on that sequence.
            //so 'c' won't be printed.
            subject.OnNext("c");
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
