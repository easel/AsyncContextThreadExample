using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx.Synchronous;


namespace AsyncContextThreadExample
{
    public partial class Service1 : ServiceBase
    {
        private readonly AsyncContextThread thread = null;
        private Boolean isRunning = false;
        public Service1()
        {
            InitializeComponent();
            thread = new AsyncContextThread();
        }

        private async Task AsyncMain()
        {
            EventLog.WriteEntry("AsyncMain Started");
            while (isRunning)
            {
                await Task.Delay(500);
                await Task.Run(() =>
                {
                    EventLog.WriteEntry("AsyncMain Re-Checking");
                });
            }
        }

        protected override void OnStart(string[] args)
        {
            isRunning = true;
            thread.Factory.Run(AsyncMain);
            EventLog.WriteEntry("Starting AsyncContextThreadExampleService");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Stopping Service");
            isRunning = false;
            thread.JoinAsync().WaitAndUnwrapException();
        }
    }
}
