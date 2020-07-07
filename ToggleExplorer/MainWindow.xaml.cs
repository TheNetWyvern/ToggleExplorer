using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToggleExplorer.Enums;

namespace ToggleExplorer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RegistryKey ourKey;
        public MainWindow()
        {
            InitializeComponent();
            ourKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            ourKey = ourKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", RegistryKeyPermissionCheck.ReadWriteSubTree);
            ourKey.SetValue("AutoRestartShell", 0, RegistryValueKind.DWord);
            ApplicationActivationManager appActiveManager = new ApplicationActivationManager();
            uint pid;
            //            Microsoft.SunriseBaseGame_8wekyb3d8bbwe!SunriseReleaseFinal
            appActiveManager.ActivateApplication("Microsoft.SunriseBaseGame_8wekyb3d8bbwe!SunriseReleaseFinal", null, ActivateOptions.None, out pid);
            //MessageBox.Show(pid.ToString());
            ToggleExplorer();
        }

        private void ToggleExplorerProcess_Click(object sender, RoutedEventArgs e)
        {
            ToggleExplorer();
        }

        private void ToggleExplorer() 
        {
            Process[] processes = Process.GetProcessesByName("explorer");
            if (processes.Count() > 0)
            {
                Thread.Sleep(30000);
                foreach (Process process in processes)
                {
                    // process.Close();
                    process.Kill();
                }
            }
            else
            {
                Process.Start($"{System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System))}/explorer.exe");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ourKey.SetValue("AutoRestartShell", 1, RegistryValueKind.DWord);
            Process[] processes = Process.GetProcessesByName("explorer");
            if (processes.Count() < 1) 
            { 
                Process.Start($"{System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System))}/explorer.exe");
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Topmost = false; // set topmost false first
            Topmost = true; // then set topmost true again.
        }
    }
}
