using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternetTester.BusinessLogic.InternetAccessChecker
{
    public class InternetAccessCheckerAgent : IDisposable
    {
        IInternetAccessChecker _internetAccessChecker;
        int _checkDelay;
        Task _task;
        CancellationTokenSource _taskCancelationTokenSource;

        public bool? IsInternetAvailable { get; private set; }

        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public event ChangedEventHandler Changed;

        protected void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        public InternetAccessCheckerAgent(IInternetAccessChecker internetAccessChecker, int checkDelay)
        {
            _internetAccessChecker = internetAccessChecker;
            _checkDelay = checkDelay;
        }

        public void Start()
        {
            _taskCancelationTokenSource = new CancellationTokenSource();
            _task = new Task(InternetCheckerTaskFunction, _taskCancelationTokenSource.Token);

            _task.Start();
        }

        public void Stop()
        {
            CancelInternetCheckerTask();
            IsInternetAvailable = null;
        }

        public void Dispose()
        {
            if(_task.Status == TaskStatus.Running)
            {
                CancelInternetCheckerTask();
            }
        }

        private void CancelInternetCheckerTask()
        {
            if (_task == null || _taskCancelationTokenSource == null)
                throw new InvalidOperationException("Agent wasn't started.");
            if (_taskCancelationTokenSource.IsCancellationRequested)
                throw new InvalidOperationException("Agent stop already triggered.");

            _taskCancelationTokenSource.Cancel();
        }

        private void InternetCheckerTaskFunction()
        {
            while (!_taskCancelationTokenSource.Token.IsCancellationRequested)
            {
                var isInternetAvailable = _internetAccessChecker.IsInternetAvailable();

                if (IsInternetAvailable != isInternetAvailable)
                {
                    IsInternetAvailable = isInternetAvailable;
                    OnChanged(EventArgs.Empty);
                }

                Thread.Sleep(_checkDelay);
            }
        }
    }
}
