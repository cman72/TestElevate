using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Win32;

namespace TestElevate
{
    class Program
    {        
        public static void ExecuteAsAdmin(string path)
        {
            try

            {
                Process proc = new Process();               
                proc.StartInfo.FileName = path;
                proc.StartInfo.Arguments = "conf";
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "runas";
                proc.Start();
            }

            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);   
            }
        }

        public static void SetRegistry(string un, string pw)

        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\AutoAdminLogon");
            key.SetValue("AutoAdminLogon", "1");
            key.Close();
            
        }

        public static string ReadRegistry()

        {
           
            string isConfigured;
            RegistryKey key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
            key = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon");
            isConfigured= key.GetValue("AutoAdminLogon").ToString();          
            key.Close();
            return isConfigured;      
          
        }


        static void Main(string[] args)
        {
            string username = "";
            string password = "";
            string answer = "";
            string exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            if (args.Length == 1)
            {
                Console.WriteLine("Username:");
                username = Console.ReadLine();
                Console.WriteLine("Password:");
                password = Console.ReadLine();

                SetRegistry(username, password);
                Console.WriteLine("Registry is now configured for autologon with username: " + username);
            }

            else

            {
                ReadRegistry();
                Console.WriteLine(ReadRegistry());
                Console.WriteLine("Press C to configure autologon");
                answer = Console.ReadLine();
                

                if (answer == "c" || answer == "C")

                {  
                    ExecuteAsAdmin(exeName);
                }
            }
        }
    }
}
