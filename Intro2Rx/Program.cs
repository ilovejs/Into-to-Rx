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
            var bufferSize = 2;
            var subject = new ReplaySubject<string>(bufferSize);
            subject.OnNext("a");

            subject.OnNext("b");
            subject.OnNext("c");
            subject.Subscribe(Console.WriteLine);
            subject.OnNext("d");
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
