using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;
using RestMocker.Core;

namespace RestMocker.WorkerRole
{
    /// <summary>
    /// Main class
    /// </summary>
    public class WorkerRole : RoleEntryPoint
    {

        #region Properties

        private IDisposable app = null;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        #endregion Properties


        #region Methods

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public override void Run()
        {

            IocFactory.Instance.Logger.Info("Running worker");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        /// <summary>
        /// Called when [start].
        /// </summary>
        /// <returns>Result of base.OnStart()</returns>
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["WebEndpoint"];
            var baseUri = String.Format("{0}://{1}", endpoint.Protocol, endpoint.IPEndpoint);

            IocFactory.Instance.Logger.Info("Starting OWIN at {0}", baseUri);


            this.app = WebApp.Start<Startup>(new StartOptions(url: baseUri));
            var result = base.OnStart();
            return result;
        }

        /// <summary>
        /// Called when [stop].
        /// </summary>
        public override void OnStop()
        {
            IocFactory.Instance.Logger.Info("RestMockerWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();


            if (this.app != null)
            {
                this.app.Dispose();
            }

            base.OnStop();


            IocFactory.Instance.Logger.Info("RestMockerWorkerRole has stopped");
        }

        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }
        }

        #endregion Methods
    }
}
