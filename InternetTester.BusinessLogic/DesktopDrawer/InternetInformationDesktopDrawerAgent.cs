using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetTester.BusinessLogic.DesktopDrawer
{
    public class InternetInformationDesktopDrawerAgent
    {
        Task _task;
        CancellationTokenSource _taskCancelationTokenSource;
        bool _isInternetAvailable;

        public void Start()
        {
            _taskCancelationTokenSource = new CancellationTokenSource();
            _task = new Task(DrawingFunction, _taskCancelationTokenSource.Token);

            _task.Start();
        }

        public void Stop()
        {
            CancelDrawingTask();
        }

        private void DrawingFunction(object obj)
        {
            while (true)
            {
                using (var contextWrapper = new DesktopContextWraper())
                {
                    var context = contextWrapper.Context;
                    var validBrush = new SolidBrush(Color.Green);
                    var invalidBrush = new SolidBrush(Color.Red);

                    if(_isInternetAvailable)
                    {
                        context.FillEllipse(validBrush, new Rectangle(0, 0, 20, 20));
                    }
                    else
                    {
                        context.FillEllipse(invalidBrush, new Rectangle(0, 0, 20, 20));
                        context.DrawString("NO INTERNET", new Font(FontFamily.GenericMonospace, 30, FontStyle.Bold), invalidBrush, Screen.PrimaryScreen.Bounds.Width / 2, 0, new StringFormat() { Alignment = StringAlignment.Center });
                    }
                }
                Thread.Sleep(300);
            }
        }
        
        private void CancelDrawingTask()
        {
            if (_task == null || _taskCancelationTokenSource == null)
                throw new InvalidOperationException("Agent wasn't started.");
            if (_taskCancelationTokenSource.IsCancellationRequested)
                throw new InvalidOperationException("Agent stop already triggered.");

            _taskCancelationTokenSource.Cancel();
        }

        public void SetInternetAvailability(bool isInternetAvailable)
        {
            _isInternetAvailable = isInternetAvailable;
        }
    }
}
