// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices
{
	using System;
	using System.ServiceModel;
	using System.ServiceModel.Activation;

	public class CustomServiceHostFactory : ServiceHostFactory
	{
		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			ServiceHost host = base.CreateServiceHost(serviceType, baseAddresses);

			// Custom code here...
			// Get started: http://blogs.msdn.com/b/carlosfigueira/archive/2011/06/14/wcf-extensibility-servicehostfactory.aspx

			#region [ Register Events ]

			host.Opened += new EventHandler(OnHostOpened);
			host.Closed += new EventHandler(OnHostClosed);

			host.Faulted += new EventHandler(OnHostFaulted);

			#endregion

			#region [ ServiceThrottlingBehavior ]

			// Note: ServiceThrottlingBehavior can be set in the config file.
			// Example with 1 CPU:

			// <behaviors>
			//  <serviceBehaviors>
			//   <behavior>
			//    <serviceThrottling
			//      maxConcurrentCalls="16"
			//      maxConcurrentSessions="100"
			//      maxConcurrentInstances="116" />

			// MSDN:
			// MaxConcurrentCalls: the default is 16 times the processor count (MaxConcurrentCalls should be set to less than the SQL connection pool size in queued scenarios).
			// MaxConcurrentSessions: the default is 100 times the processor count.
			// MaxConcurrentInstances: the default is the sum of the value of MaxConcurrentSessions and the value of MaxConcurrentCalls.

			/*ServiceThrottlingBehavior throttlingBehavior = new ServiceThrottlingBehavior();

			throttlingBehavior.MaxConcurrentCalls = 16 * Environment.ProcessorCount;
			throttlingBehavior.MaxConcurrentSessions = 100 * Environment.ProcessorCount;
			throttlingBehavior.MaxConcurrentInstances = throttlingBehavior.MaxConcurrentCalls + throttlingBehavior.MaxConcurrentSessions;

			host.Description.Behaviors.Add(throttlingBehavior);*/

			#endregion

			return host;
		}

		#region [ Events ]

		private void OnHostOpened(object sender, EventArgs e)
		{
			// Custom code here...
		}

		private void OnHostClosed(object sender, EventArgs e)
		{
			// Custom code here...
		}

		private void OnHostFaulted(object sender, EventArgs e)
		{
			// Custom code here...
		}

		#endregion
	}
}