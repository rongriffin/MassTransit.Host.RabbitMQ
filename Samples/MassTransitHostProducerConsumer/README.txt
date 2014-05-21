Simple Producer Consumer example where the Consumer is hosted using MassTransit.Host.RabbitMQ.

Messaging.csproj
----------------

Shared assembly containing messages passed between producer and consumer.

Producer
---------

Console application that publishes messages to the consumers.

Consumer
--------

Class library with a single consumer.  MassTransit.Host.RabbitMQ is referenced from NuGet and set as the external startup project.

To Run: 
--------

  1. Open MassTransitHostProducerConsumer.sln in Visual Studio 2013 or later.
  2. Start an instance of the Producer project.
  3. Start an instance of the Consumer project.
  4. In the Producer console window, press enter to publish a message.  The Consumer console window should output the message details.