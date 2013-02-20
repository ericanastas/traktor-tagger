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
            

            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            log.Info("Traktor Tagger " + version+" starting up...");



            log.Debug("Subscribing to App_DispatcherUnhandledException");
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;


            //var traktorProcesses = System.Diagnostics.Process.GetProcessesByName(TraktorTagger.Properties.Resources.TraktorEXE);


            log.Info("Checking for running traktor processs...");
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
                    log.Info("Running traktor processs found! Shutting down...");

                    System.Windows.MessageBox.Show("Traktor.exe has been detected! Traktor Tagger can not be started while Traktor is running.\n\nTraktor Tagger will now close. ", "Traktor Tagger", MessageBoxButton.OK, MessageBoxImage.Stop);

                    this.Shutdown();
                }
            }
            log.Info("No running traktor processs found.");

        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            log.Fatal("An Unhandled exception was thrown", e.Exception);
        }


    
    }

    
}
