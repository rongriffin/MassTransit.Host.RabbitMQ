# MassTransit.Host.RabbitMQ #

A service host process for MassTransit consumer services using RabbitMQ for transport.  **Currently not compatible with MassTransit 3.0 and later**

**Apache License, Version 2.0:** [http://www.apache.org/licenses/LICENSE-2.0.html](http://www.apache.org/licenses/LICENSE-2.0.html)

**Usage:**

1. Create a new Visual Studio class library (.NET 4.5 or later).
2. Install **MassTransit.Host.RabbitMQ** from NuGet (Install-Package MassTransit.Host.RabbitMQ)
3. Create MassTransit consumers that implement Consumes<T> (see: [http://masstransit.readthedocs.org/en/latest/learning/firstConsumer.html](http://masstransit.readthedocs.org/en/latest/learning/firstConsumer.html))
4. Update *MassTransit.Host.RabbitMQ.exe.config* in your project to configure your RabbitMQ endpoint address and credentials.
5. Run!

**Enable Logging:**

The host uses NLog for error logging and for logging diagnostics (e.g. Listing the subscribers that the host loaded).  To configure logging, install Nlog.Config from [https://www.nuget.org/packages/NLog.Config/](https://www.nuget.org/packages/NLog.Config/ "NuGet") and configure your logging provider.  

For example, to log output to the Console, edit NLog.config as follows:

	<?xml version="1.0" encoding="utf-8" ?>
	<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
	      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	  <!-- 
	  See https://github.com/nlog/nlog/wiki/Configuration-file 
	  for information on customizing logging rules and outputs.
	   -->
	  <targets>
	    <!-- add your targets here -->
	    <target xsi:type="Console" name="c" 
	            layout="${longdate} ${uppercase:${level}} ${message}" />
	  </targets>
	  <rules>
	    <!-- add your logging rules here -->
	    <logger name="*" minlevel="Trace" writeTo="c" />
	  </rules>
	</nlog>

**To Install as a Windows Service:**

1. Build your project.
2. Copy your build output folder to the server you'd like to install to.
3. Open a command prompt on the server and navigate to the folder you copied.
4. From the command prompt, run *MassTransit.Host.RabbitMQ.exe install -username "myUser" -password "myPassword" --delayed -servicename "MyMassTransitConsumerService" -description "My service description" -displayname "My MassTransit Consumer Service"*	
	- NOTE: For more command line options, review the TopShelf command line reference: [http://topshelf.readthedocs.org/en/latest/overview/commandline.html](http://topshelf.readthedocs.org/en/latest/overview/commandline.html)

See Also:

Mass Transit - [http://masstransit-project.com/](http://masstransit-project.com/)

Topshelf - [http://topshelf-project.com/](http://topshelf-project.com/)