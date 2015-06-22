﻿Managing events like these is what Rx was built for:

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
