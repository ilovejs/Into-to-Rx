using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Intro2Rx
{
    class Program
    {
        static void Main(string[] args)
        {
//            var singleValue = Observable.Return<string>("Value");
            
            //Can be reduced to the following
            var singleValue = Observable.Return("Value");

            //Equivalent to, below:

            //which could have also been simulated with a replay subject
            var subject = new ReplaySubject<string>();
            subject.OnNext("Value");
            subject.OnCompleted();
        }

        // Takes an IObservable<string> as its parameter. 
        // Subject<string> implements this interface.
        static void WriteSequenceToConsole(IObservable<string> sequence)
        {
            //The next two lines are equivalent.
            //sequence.Subscribe(value=>Console.WriteLine(value));
            //Extension Method doc: https://msdn.microsoft.com/en-us/library/system.observableextensions(v=VS.103).aspx
            sequence.Subscribe(Console.WriteLine);
        }
    }
}
