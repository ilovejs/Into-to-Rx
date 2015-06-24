This repo holds practicing code for tutorial: Intro to Rx
http://www.introtorx.com/


Managing events like these is what Rx was built for:

UI events like mouse move, button click

Domain events like property changed, collection updated, "Order Filled", "Registration accepted" etc.

Infrastructure events like from file watcher, system and WMI events

Integration events like a broadcast from a message bus or a push event from WebSockets API or other low latency middleware like Nirvana.
	url: http://www.my-channels.com/

Integration with a CEP engine like StreamInsight or StreamBase.
	url: http://www.microsoft.com/sqlserver/en/us/solutions-technologies/business-intelligence/complex-event-processing.aspx
	url: http://www.streambase.com/
	
Interestingly Microsoft's CEP product StreamInsight, which is part of the SQL Server family, 
also uses LINQ to build queries over streaming events of data.


microsoft concurrent page: http://msdn.microsoft.com/en-us/concurrency

read until with Diigo annotation.
http://www.introtorx.com/Content/v1.0.10621.0/04_CreatingObservableSequences.html

ref
https://www.youtube.com/watch?v=iVhYCCrUHw8

his github and blog
http://leecampbell.blogspot.com.au/2010/08/reactive-extensions-for-net.html

Jeffery Richter's brilliant book <CLR via C#> or Joe Duffy's comprehensive <Concurrent Programming on Windows>. 
Most stuff on the internet is blatant plagiary of Richter's examples from his book. 
An in-depth examination of APM is outside of the scope of this book.

Transite from APM model:
To utilize the Asynchronous Programming Model but avoid its awkward API, 
we can use the Observable.FromAsyncPattern method. 
Jeffery van Gogh gives a brilliant walk through of the Observable.FromAsyncPattern in Part 1 of his Rx on the Server blog 
series.  http://blogs.msdn.com/b/jeffva/archive/2010/07/23/rx-on-the-server-part-1-of-n-asynchronous-system-io-stream-reading.aspx
While the theory backing the Rx on the Server series is sound, it was written in mid 2010 and targets an old version of Rx.

