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
//            var subject = new Subject<int>();
//         
//            subject.Subscribe(
//                Console.WriteLine,
//                () => Console.WriteLine("Completed"));
//
//            subject.OnCompleted();
//            subject.OnNext(2);

            Console.WriteLine("Normal color");
            using (new ConsoleColor(System.ConsoleColor.Red))
            {
                Console.WriteLine("Now I am Red");
                using (new ConsoleColor(System.ConsoleColor.Green))
                {
                    Console.WriteLine("Now I am Green");
                }
                Console.WriteLine("and back to Red");
            }
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
