using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Flat4Me.ImageService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public static string EVENT_LOG_NAME = "Flat4Me Image Services";
        
        public ProjectInstaller()
        {
            InitializeComponent();

            foreach (Installer i in this.serviceInstaller.Installers)
            {
                EventLogInstaller eventLogInstall = i as EventLogInstaller;
                if (eventLogInstall != null)
                {
                    eventLogInstall.Log = EVENT_LOG_NAME;
                    eventLogInstall.UninstallAction = UninstallAction.NoAction;
                    break;
                }
            }
        }
    }
}
