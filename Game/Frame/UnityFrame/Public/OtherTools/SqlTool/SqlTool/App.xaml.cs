using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace SqlTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private delegate void NextPrimeDelegate();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NextPrimeDelegate(Update));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        private void App_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("一个未被处理的异常:" + e.Exception.GetType().ToString());
            e.Handled = true;
        }

        private void Update()
        {
            Thread.Sleep(1);
            Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new NextPrimeDelegate(Update));
        }
    }
}
