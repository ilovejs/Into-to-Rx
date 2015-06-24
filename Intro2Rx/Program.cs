using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Timers;

namespace Intro2Rx
{
    class Program
    {
        static void Main(string[] args)
        {
            var range = Observable.Range(10, 15);
            range.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
        }

        //Example code only
        public static void NonBlocking_event_driven()
        {
            var ob = Observable.Create<string>(observer => {
                var timer = new System.Timers.Timer();
                timer.Interval = 1000;
                //2 action happen here.
                timer.Elapsed += (s, e) => observer.OnNext("tick"); 
                timer.Elapsed += OnTimerElapsed;
                timer.Start();
                
                //return timer as IDisposable token ??     
                //Now when a consumer disposes of their subscription, the underlying Timer will be disposed of too.

                /*
                 When we dispose of our subscription, we will stop seeing "tick" being written to the screen;
                 
                 * So output can be:
                 *  tick
                    01/01/2012 12:00:00
                    tick
                    01/01/2012 12:00:01
                    tick
                    01/01/2012 12:00:02
                    01/01/2012 12:00:03
                    01/01/2012 12:00:04
                    01/01/2012 12:00:05
                 */

                //Use an action to un-register the event handler, preventing a memory leak by retaining the reference to the timer.
                return () =>
                {
                    timer.Elapsed -= OnTimerElapsed;
                    timer.Dispose();
                };

//                return Disposable.Empty;
            });

            //simple subscription.
            var subscription = ob.Subscribe(Console.WriteLine);
            Console.ReadLine();
            
            subscription.Dispose();
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(e.SignalTime);
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
