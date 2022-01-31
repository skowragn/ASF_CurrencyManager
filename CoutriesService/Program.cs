using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Threading;

namespace CountriesService
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {

                ServiceRuntime.RegisterServiceAsync("CountriesServiceType",
                    context => new CountriesService(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(CountriesService).Name);

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
