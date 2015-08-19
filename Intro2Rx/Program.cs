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
            First();

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

        static void Any()
        {
            //subject.Any(i => i > 2);
            //Functionally equivalent to subject.Where(i => i > 2).Any();

            var subject = new Subject<int>();
            subject.Subscribe(Console.WriteLine, () => Console.WriteLine("Subject completed"));
            
            var any = subject.Any();
            any.Subscribe(b => Console.WriteLine("The subject has any values? {0}", b));

            subject.OnNext(1);  //If we now remove the OnNext(1), the output will change to the following
            subject.OnCompleted();
        }

        static void AnyWithError()
        {
            var subject = new Subject<int>();
            subject.Subscribe(Console.WriteLine,
                ex => Console.WriteLine("subject OnError : {0}", ex),
                () => Console.WriteLine("Subject completed"));
            var any = subject.Any();
            any.Subscribe(b => Console.WriteLine("The subject has any values? {0}", b),
                ex => Console.WriteLine(".Any() OnError : {0}", ex),
                () => Console.WriteLine(".Any() completed"));
            subject.OnError(new Exception());
        }

        static void All()
        {
            var subject = new Subject<int>();
            subject.Subscribe(Console.WriteLine, () => Console.WriteLine("Subject completed"));
            
            var all = subject.All(i => i < 5);
            all.Subscribe(b => Console.WriteLine("All values less than 5? {0}", b));
            
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(6);    //trigger to log "All values less than 5? False"
            subject.OnNext(2);
            subject.OnNext(1);
            subject.OnCompleted();
        }

        static void Contains()
        {
            var subject = new Subject<int>();
            subject.Subscribe(
                Console.WriteLine,
                () => Console.WriteLine("Subject completed"));

            //Terminate when 2 is found 
            var contains = subject.Contains(2);
                contains.Subscribe(
                b => Console.WriteLine("Contains the value 2? {0}", b),
                () => Console.WriteLine("contains completed"));

            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnCompleted();
        }

        static void DefaultIfEmpty()
        {
            var subject = new Subject<int>();
            subject.Subscribe(
                Console.WriteLine,
                () => Console.WriteLine("Subject completed"));
            
            //return a single value if the source sequence is empty
            var defaultIfEmpty = subject.DefaultIfEmpty();
            defaultIfEmpty.Subscribe(
                b => Console.WriteLine("defaultIfEmpty value: {0}", b),
                () => Console.WriteLine("defaultIfEmpty completed"));
            
//            subject.OnNext(1);
//            subject.OnNext(2);
//            subject.OnNext(3);
            subject.OnCompleted();
        }

        static void SequenceEqual()
        {
            var subject1 = new Subject<int>();
            subject1.Subscribe(
                i => Console.WriteLine("subject1.OnNext({0})", i),
                () => Console.WriteLine("subject1 completed"));

            var subject2 = new Subject<int>();
            subject2.Subscribe(
                i => Console.WriteLine("subject2.OnNext({0})", i),
                () => Console.WriteLine("subject2 completed"));
            
            var areEqual = subject1.SequenceEqual(subject2);
                areEqual.Subscribe(
                i => Console.WriteLine("areEqual.OnNext({0})", i),
                () => Console.WriteLine("areEqual completed"));
            
            subject1.OnNext(1);
            subject1.OnNext(2);
            subject2.OnNext(1);
            subject2.OnNext(2);
            
            subject2.OnNext(3);
            subject1.OnNext(3);
            
            subject1.OnCompleted();
            subject2.OnCompleted();
        }
        #endregion

        #region Aggregation

        static void Count()
        {
            var numbers = Observable.Range(0, 3);
            numbers.Dump("numbers");
            numbers.Count().Dump("count");
        }

        static void Min_Average()
        {
            var numbers = new Subject<int>();

            //extension methods
            numbers.Dump("numbers");
            numbers.Min().Dump("Min");
            numbers.Average().Dump("Average");

            numbers.OnNext(1);
            numbers.OnNext(2);
            numbers.OnNext(3);
            numbers.OnCompleted();
        }

        static void First()
        {
            var interval = Observable.Interval(TimeSpan.FromSeconds(3));
            // Will block for 3s before returning
            // If the source sequence does not have any values (i.e. is an empty sequence) then the First method will throw an exception
            /*  You can cater for this in three ways:
                Use a try/catch blocks around the First() call
                Use Take(1) instead. However, this will be asynchronous, not blocking.
                Use FirstOrDefault extension method instead, but it will still block until the source produces any notification
             */
            Console.WriteLine(interval.First());
        }

        static void MyAggregate()
        {
            // var sum = source.Aggregate((acc, currentValue) => acc + currentValue);

            // See MyMin in SampleExtentions.cs
        }

        // Scan is aggregation function with initial value.
        static void Scan()
        {
            /*
             Aggregate is also not a good fit for infinite sequences. 
             * The Scan extension method however meets this requirement perfectly. 
             * The signatures for both Scan and Aggregate are the same; 
             * the difference is that Scan will push the result from every call to the accumulator function. 
             * 
             * So instead of being an aggregator that reduces a sequence to a single value sequence, 
             * it is an accumulator that we return an accumulated value for each value of the source sequence.
             */
            var numbers = new Subject<int>();
            var scan = numbers.Scan(0, (acc, current) => acc + current);
            numbers.Dump("numbers");
            scan.Dump("scan");
            numbers.OnNext(1);
            numbers.OnNext(2);
            numbers.OnNext(3);
            numbers.OnCompleted();
        }

        // More sample haven't done:
        // http://www.introtorx.com/Content/v1.0.10621.0/07_Aggregation.html#Count
        
        static void GroupBy()
        {
            //it is not good practice to have these nested subscribe calls:
//            var source = Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(10);
//
//            var group = source.GroupBy(i => i % 3);
//
//            group.Subscribe(grp =>
//                grp.Min().Subscribe(
//                    minValue =>
//                        Console.WriteLine("{0} min value = {1}", grp.Key, minValue)),
//                        () => Console.WriteLine("Completed"));
    
            var source = Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(10);
            
            var group = source.GroupBy(i => i % 3);
            //project and merge into diff seq
            group.SelectMany(
                grp => grp.Max()
                          .Select(value => new { grp.Key, value }))
                          .Dump("group");
        }

        // Section: Transformation of sequences http://www.introtorx.com/Content/v1.0.10621.0/08_Transformation.html


        #endregion

        #region Transformation of sequences

        static void TrnasformValue()
        {
            var source = Observable.Range(0, 5);
            source.Select(i => i + 3)
                  .Dump("+3");      //All seq element +3 transformation
        }
        
        static void TransformType()
        {
            //TIPS: 'a' = 65
//            Observable.Range(1, 5)
//                      .Select(i =>(char)(i + 64))
//                      .Dump("char");
//            
            //Transform seq of int to anonymous types
            Observable.Range(1, 5)
                      .Select(i => new { Number = i, Character = (char)(i + 64) })
                      .Dump("anon");

            //Alternative to write in 'query comprehension syntax
//            var query = from i in Observable.Range(1, 5)
//                        select new { Number = i, Character = (char)(i + 64) };
//                        query.Dump("anon");

        }

        static void Cast()
        {
            var objects = new Subject<object>();
            //obj -> int
            objects.Cast<int>().Dump("cast");
            
            objects.OnNext(1);
            objects.OnNext(2);
            objects.OnNext(3);
            //put a trap here
            objects.OnNext("4");   //Thankfully, if this is not what we want, we could use the alternative extension method OfType<T>().
            objects.OnCompleted();
        }

        static void OfType()
        {
            var objects = new Subject<object>();
            //obj -> int
            objects.OfType<int>().Dump("OfType");

            objects.OnNext(1);
            objects.OnNext(2);
            
            //put a trap here
            objects.OnNext("4");   //Ignore the error ! :)

            objects.OnNext(3);

            objects.OnCompleted();
        }

        static void ImplicitCast()
        {
            // It is fair to say that while these are convenient methods to have, 
            // we could have created them with the operators we already know about.
            var objects = new Subject<object>();   //Allow object, rather than forcing to be 'int' or 'string'
            
//            objects.Select(i => i.ToString())
//                   .Dump("implicit cast mimic Cast<int>() for all elem");

            objects.Where(i => i is int)
                   .Select(i => (int) i)
                   .Dump("implicit cast mimic conditional cast");

            objects.OnNext("1");
            objects.OnNext(2);

            //put a trap here
            objects.OnNext("3");   //Ignore the error ! :)

            objects.OnNext(4);

            objects.OnCompleted();
        }

        static void Timestamp()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                      .Take(3)
                      .Timestamp()
                      .Dump("TimeStamp");
        }

        static void TimeInterval()
        {
            //Not exact. Need research the output
            Observable.Interval(TimeSpan.FromSeconds(1))
                      .Take(3)
                      .TimeInterval()
                      .Dump("TimeStamp");
        }

        //Hack: SelectMany is like flatMap in scala
        static void Test_SelectMany()
        {
//            Observable.Return(3)
//                      .SelectMany(i => Observable.Range(1, i))   //only print [1,2,3]
//                      .Dump("SelectMany");

            Observable.Range(1, 3)          //changed here
                      .SelectMany(i => Observable.Range(1, i))   //print flatten result [1, 1,2, 1,2,3]
                      .Dump("SelectMany");
        }

        static void Test_SelectMany2()
        {
            Func<int, char> letter = i => (char)(i + 64);
            
//            Observable.Return(1)
            Observable.Range(1,3)
                      .SelectMany(i => Observable.Return(letter(i)))   //apply a function
                      .Dump("SelectMany");
        }

        static void Test_SelectMany3()
        {
            Func<int, char> letter = i => (char)(i + 64);
            
            Observable.Range(1, 30)
            .SelectMany(i => {                 //WOOOOW ! This is super helpful
                if (0 < i && i < 27)
                {
                    return Observable.Return(letter(i));
                }
                else
                {
                    return Observable.Empty<char>();
                }
            })
            .Dump("SelectMany");
        }


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
