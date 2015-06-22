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
            //BehaviorSubject only remembers the last publication.
            //Need to provide a default value. This means that all subscribers will receive a value immediately (unless it is already completed).
            var subject = new BehaviorSubject<string>("a");
            subject.OnNext("b");
            subject.OnNext("c");
            
            subject.Subscribe(Console.WriteLine);

            subject.OnNext("d");

            subject.OnCompleted();

            /*
            * That note that there is a difference between a ReplaySubject<T> with a buffer size of one (commonly called a 'replay one subject') and a BehaviorSubject<T>. 
            * 
            * 1. A BehaviorSubject<T> requires an initial value. 
            *    With the assumption that neither subjects have completed, then you can be sure that the BehaviorSubject<T> will have a value. 
            *    You cannot be certain with the ReplaySubject<T> however
            * 
            * 2. Another difference is that a replay-one-subject will still cache its value once it has been completed. 
            *    So subscribing to a completed BehaviorSubject<T> we can be sure to not receive any values, 
            *    but with a ReplaySubject<T> it is possible.
            */
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
