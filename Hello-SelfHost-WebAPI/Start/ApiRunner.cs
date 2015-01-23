using System;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace Hello_SelfHost_WebAPI.Start
{
    internal class ApiRunner
    {
        private ManualResetEvent _stopManualResetEvent;
        private Thread _workerThread;

        public void Start()
        {
            if (_stopManualResetEvent != null)
            {
                throw new InvalidOperationException("Api Runner is already running");
            }
            Console.WriteLine("Starting self hosted server at http://localhost:9000/");
            _stopManualResetEvent = new ManualResetEvent(false);
            _workerThread = new Thread(() =>
            {
                const string baseUrl = "http://localhost:9000/";
                using (WebApp.Start<Startup>(baseUrl))
                {
                    _stopManualResetEvent.WaitOne();
                }
            });

            _workerThread.Start();
            Console.WriteLine("Started!");
        }

        public void Stop()
        {
            Console.WriteLine("Stopping self hosted server.");
            if (_stopManualResetEvent == null)
            {
                throw new InvalidOperationException("Api Runner is not running");
            }

            _stopManualResetEvent.Set();
            _stopManualResetEvent = null;
            _workerThread = null;
            Console.WriteLine("Stopped!");
        }
    }
}