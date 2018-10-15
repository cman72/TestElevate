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

        static string defaultUserName = "";
        static string defaultDomain = "";
        static string isConfigured;

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
            try

            {
                //RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon");
                RegistryKey key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                key = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
                key.SetValue("AutoAdminLogon", "1");
                key.SetValue("ForceAutologon", "1");
                key.SetValue("DefaultUserName", un);
                key.SetValue("DefaultPassword", pw);
                key.SetValue("DefaultDomain", "enfogroup.com");

                key.Close();
            }

            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        public static string ReadRegistry()

        {
           
            //string isConfigured;
            RegistryKey key = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
            key = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon");
            isConfigured= key.GetValue("AutoAdminLogon").ToString();
            defaultUserName = key.GetValue("DefaultUserName").ToString();
            defaultDomain = key.GetValue("DefaultDomain").ToString();
            key.Close();
            return isConfigured;      
          
        }

        public static void Exit()

        {
            Environment.Exit(0);
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
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }

            else

            {
                ReadRegistry();
                if  (isConfigured == "1")


                    //Console.WriteLine(ReadRegistry());

                    Console.WriteLine($"This computer is configured for autologon for \nUsername: {defaultUserName} \nDomain: {defaultDomain}");

                else
                    Console.WriteLine($"This computer is not configured for autologon");


                Console.WriteLine("\nPress C if you want to configure autologon or any other key to exit");
                answer = Console.ReadLine();
                

                if (answer == "c" || answer == "C")

                {  
                    ExecuteAsAdmin(exeName);
                }

                else
                    Exit();
            }
        }
    }
}
