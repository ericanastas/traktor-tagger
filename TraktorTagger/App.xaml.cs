using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TraktorTagger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;


            var traktorProcesses = System.Diagnostics.Process.GetProcessesByName(TraktorTagger.Properties.Resources.TraktorEXE);

            var processes = System.Diagnostics.Process.GetProcesses();

            foreach(var p in processes)
            {
                System.Diagnostics.ProcessModule module;

                try
                {
                    module = p.MainModule;

                }
                catch
                {
                    continue;
                }

                

                if(String.Compare(module.ModuleName, TraktorTagger.Properties.Resources.TraktorEXE,true) == 0)
                {
                    System.Windows.MessageBox.Show("Traktor.exe has been detected! Traktor Tagger can not be started while Traktor is running.\n\nTraktor Tagger will now close. ", "Traktor Tagger", MessageBoxButton.OK, MessageBoxImage.Stop);

                    this.Shutdown();
                }
            }
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            log.Error("An Unhandled exception was thrown", e.Exception);
        }


    
    }

    
}
