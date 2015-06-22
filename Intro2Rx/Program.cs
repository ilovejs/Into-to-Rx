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
             * Subject's siblings:
             * ReplaySubject<T> provides the feature of caching values and then replaying them for any late subscriptions.
             */
            
//            var subject = new Subject<string>();
            var subject = new ReplaySubject<string>();

            subject.OnNext("a");  //If subject is of Subject type, then 'a' won't be played.
                                  //or of ReplaySubject type, then full subscription will be played.

            WriteSequenceToConsole(subject);
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
