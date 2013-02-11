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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


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


    
    }

    
}
