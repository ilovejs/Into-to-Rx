using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Intro2Rx
{
    class Program
    {
        static void Main(string[] args)
        {
            skipUntil_takeUntil();

            Console.ReadKey();
        }
#region Part II 
        
        #region Creating a sequence
        /*
        
        Factory Methods
            Observable.Return
            Observable.Empty
            Observable.Never
            Observable.Throw
            Observable.Create
         
        Unfold methods
            Observable.Range
            Observable.Interval
            Observable.Timer
            Observable.Generate
         
        Paradigm Transition
            Observable.Start
            Observable.FromEventPattern
            Task.ToObservable
            Task<T>.ToObservable
            IEnumerable<T>.ToObservable
            Observable.FromAsyncPattern
         
         */
        #region Work with events
        public static void work_with_time_events()
        {
            // Method 1: Observable.Interval function.

            IObservable<long> alsoOneNumberPerSecond = Observable.Interval(TimeSpan.FromMilliseconds(250));
            alsoOneNumberPerSecond.Subscribe(lowNum =>
            {
                Console.WriteLine(lowNum);
            });

            // Method 2: Observable.Timer
            var timer = Observable.Timer(TimeSpan.FromSeconds(1));
            timer.Subscribe(
                Console.WriteLine,
                () => Console.WriteLine("completed"));

            // Method 3: use Observable.Generate

        }

        public static void StartTimerAndFireOnce()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(5))
                .Subscribe(
                    x =>
                    {
                        // Do Stuff Here
                        Console.WriteLine(x);
                        // Console WriteLine Prints
                        // 0
                    });
        }

        #endregion

        #region Transitioning into IObservable<T>

        static void from_task()
        {
            var t = Task.Factory.StartNew(() => "Test");
            var source = t.ToObservable();

            source.Subscribe(
                Console.WriteLine,
                () => Console.WriteLine("completed"));
        }

        static void from_delegate()
        {
            var start = Observable.Start(() =>
            {
                Console.Write("Working away");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(100);
                    Console.Write(".");
                }
            });
            start.Subscribe(
            unit => Console.WriteLine("Unit published"),
            () => Console.WriteLine("Action completed"));
        }

        static void from_delegate_return_val()
        {
            var start = Observable.Start(() =>
            {
                Console.Write("Working away");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(100);
                    Console.Write(".");
                }
                return "Published value";
            });
            start.Subscribe(
            Console.WriteLine,
            () => Console.WriteLine("Action completed"));
        }
        #endregion

        #region From events

        //wrapper of events into ob seq:  http://Rxx.codeplex.com
        static void from_events()
        {
            //Activated delegate is EventHandler
            //            var appActivated = Observable.FromEventPattern(
            //            h => MediaTypeNames.Application.Current.Activated += h,
            //            h => MediaTypeNames.Application.Current.Activated -= h);
            //            
            //            //PropertyChanged is PropertyChangedEventHandler
            //            var propChanged = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
            //            handler => handler.Invoke,
            //            h => this.PropertyChanged += h,
            //            h => this.PropertyChanged -= h);

            //FirstChanceException is EventHandler<FirstChanceExceptionEventArgs>
            var firstChanceException = Observable.FromEventPattern<FirstChanceExceptionEventArgs>(
            h => AppDomain.CurrentDomain.FirstChanceException += h,
            h => AppDomain.CurrentDomain.FirstChanceException -= h);
        }
        #endregion
        #endregion

        #region Reducing a sequence

        static void distinct()
        {
            var subject = new Subject<int>();
            var distinct = subject.Distinct();

            subject.Subscribe(
                i => Console.WriteLine("{0}", i),
                () => Console.WriteLine("subject.OnCompleted()"));
            
            distinct.Subscribe(
                i => Console.WriteLine("distinct.OnNext({0})", i),
                () => Console.WriteLine("distinct.OnCompleted()"));
            
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(4);
            subject.OnCompleted();
        }

        static void distinctUntil()
        {
            //This method will surface values only if they are different from the previous value. 

            var subject = new Subject<int>();
            var distinct = subject.DistinctUntilChanged();
            subject.Subscribe(
                i => Console.WriteLine("{0}", i),
                () => Console.WriteLine("subject.OnCompleted()"));
            distinct.Subscribe(
                i => Console.WriteLine("distinct.OnNext({0})", i),
                () => Console.WriteLine("distinct.OnCompleted()"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(4);
            subject.OnCompleted();
        }

        static void ignore_elements()
        {
            var subject = new Subject<int>();
            
            //a quirky little tool that allows you to receive the OnCompleted or OnError notifications.
            var noElements = subject.Where(_ => false); //subject.IgnoreElements();   
            
            subject.Subscribe(
                i=>Console.WriteLine("subject.OnNext({0})", i),
                () => Console.WriteLine("subject.OnCompleted()"));
            
            noElements.Subscribe(
                i=>Console.WriteLine("noElements.OnNext({0})", i),
                () => Console.WriteLine("noElements.OnCompleted()"));

            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnCompleted();

            //subject.IgnoreElements() equivalent to subject.Where(value => false);
            //Or functional style that implies that the value is ignored.   subject.Where(_ => false);
        }

        static void skipLast_takeLast()
        {
            var subject = new Subject<int>();
            subject
                .TakeLast(2)//.SkipLast(2)
                .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
            
            Console.WriteLine("Pushing 1");
            subject.OnNext(1);
            Console.WriteLine("Pushing 2");
            subject.OnNext(2);
            Console.WriteLine("Pushing 3");
            subject.OnNext(3);
            Console.WriteLine("Pushing 4");
            subject.OnNext(4);
            subject.OnCompleted();
        }

        static void skipUntil_takeUntil()
        {
//            SkipUntil will skip all values until any value is produced by a secondary observable sequence.
            var subject = new Subject<int>();
            var otherSubject = new Subject<Unit>();
            subject
//                .SkipUntil(otherSubject)    //Any value value is validate to kick off element in first queue.
                .TakeUntil(otherSubject)
                .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
            
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);

            otherSubject.OnNext(Unit.Default);  //
            
            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnNext(6);
            subject.OnNext(7);
            subject.OnNext(8);
            subject.OnCompleted();
        }
        #endregion

        #region Inspection



        #endregion
#endregion

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
