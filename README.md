# MassTransit.Host.RabbitMQ #

A service host process for MassTransit consumer services using RabbitMQ for transport.

**Apache License, Version 2.0:** [http://www.apache.org/licenses/LICENSE-2.0.html](http://www.apache.org/licenses/LICENSE-2.0.html)

**Usage:**

1. Create a new Visual Studio class library (.NET 4.5 or later).
2. Create MassTransit consumers that implement Consumes<T> (see: [http://masstransit.readthedocs.org/en/latest/learning/firstConsumer.html](http://masstransit.readthedocs.org/en/latest/learning/firstConsumer.html))
3. Install **MassTransit.Host.RabbitMQ** from NuGet (Install-Package MassTransit.Host.RabbitMQ)
4. Update *MassTransit.Host.RabbitMQ.exe.config* in your project to configure your RabbitMQ endpoint.
5. Run!

**To Install as a Windows Service:**

1. Build your project.
2. Copy your build output folder to the server you'd like to install to.
3. Open a command prompt on the server and navigate to the folder you copied.
4. From the command prompt, run *MassTransit.Host.RabbitMQ.exe -username "myUser" -password "myPassword" --delayed -servciename "MyMassTransitConsumerService" -description "My service description" -displayname "My MassTransit Consumer Service"*	
	- NOTE: For more command line options, review the TopShelf command line reference: [http://topshelf.readthedocs.org/en/latest/overview/commandline.html](http://topshelf.readthedocs.org/en/latest/overview/commandline.html)

See Also:

Mass Transit - [http://masstransit-project.com/](http://masstransit-project.com/)

Topshelf - [http://topshelf-project.com/](http://topshelf-project.com/)