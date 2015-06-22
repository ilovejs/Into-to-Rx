using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Intro2Rx
{
    class Program
    {
        static void Main(string[] args)
        {
//            BlockingMethod().Subscribe(x => Console.WriteLine("value is {0}", x));

            NonBlocking().Subscribe(x => Console.WriteLine("value is {0}", x));
        }

        private static IObservable<string> BlockingMethod()
        {
            var subject = new ReplaySubject<string>();
            subject.OnNext("a");
            subject.OnNext("b");
            subject.OnCompleted();

            Thread.Sleep(3000);
            return subject;
        }

        private static IObservable<string> NonBlocking()
        {
            //Type info can be avoided.
            return Observable.Create<string>((IObserver<string> observer) => {
                observer.OnNext("a");
                observer.OnNext("b");
                observer.OnCompleted();

                Thread.Sleep(3000);
                return Disposable.Create(() => Console.WriteLine("Observer has unsubscribed"));
                //or can return an Action like 
                //return () => Console.WriteLine("Observer has unsubscribed"); 
            });
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
