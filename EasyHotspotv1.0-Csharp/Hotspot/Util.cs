using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Windows.Forms;

namespace Hotspot
{
    class Util
    {
        private const long minimumSpeed = 81920;    //  10 kilobytes
        private static string messageTitle = string.Empty;
        private static string messageContent = string.Empty;
        private static MessageBoxButtons messageButton;

        /// <summary>
        /// Default message for no internet 
        /// </summary>
        public static void MsgNoInternet()
        {
            messageTitle = "Error";
            messageContent = "No internet connection";
            messageButton = MessageBoxButtons.OK;
            MessageBox.Show(messageContent, messageTitle, messageButton, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Check network connection and return the name of public network interface
        /// </summary>
        /// <param name="minimumSpeed">minimum speed in bits per second for testing connection</param>
        /// <param name="publicNetworkInterface">store name of public network interface</param>
        /// <returns></returns>
        public static string GetNameOfPublicNetworkInterface()
        {
            string publicNetworkInterface = String.Empty;

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MsgNoInternet();
                return null;   //  No internet connection
            }

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Discard because of standard reasons
                if ((ni.OperationalStatus != OperationalStatus.Up) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                    continue;

                // This allow to filter modems, serial, etc.
                if (ni.Speed < minimumSpeed)
                    continue;

                // Discard virtual cards (virtual box, virtual pc, etc.)
                if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;

                // Discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                    continue;

                //  This is active network interface
                publicNetworkInterface = ni.Name;

                if (String.IsNullOrEmpty(publicNetworkInterface))
                {
                    messageTitle = "Error";
                    messageContent = "Public network interface is null";
                    messageButton = MessageBoxButtons.OK;

                    MessageBox.Show(messageContent, messageTitle, messageButton, MessageBoxIcon.Error);
                    return null;
                }
                else
                    return publicNetworkInterface;
            }

            MsgNoInternet();
            return null;
        }

        /// <summary>
        /// Get name of created network interface
        /// </summary>
        /// <param name="publicNetworkInterface">name of public network interface</param>
        /// <returns></returns>
        public static string GetNameOfPrivateNetworkInterface(string publicNetworkInterface)
        {
            string title = string.Empty;
            string message = string.Empty;
            MessageBoxButtons buttons;

            string privateNetworkInterface = String.Empty;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel && ni.Name != publicNetworkInterface)
                {
                    privateNetworkInterface = ni.Name;

                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            Console.WriteLine(ip.Address.ToString());
                        }
                    }

                }
            }

            if (String.IsNullOrEmpty(privateNetworkInterface))
            {
                title = "Error";
                message = "Private network interface is null";
                buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                return null;
            }
            else
                return privateNetworkInterface;

        }

        /// <summary>
        /// A function that execute a cmd command and return the output from console
        /// </summary>
        /// <param name="command">cmd command</param>
        /// <returns></returns>
        public static string CmdCommand(string command)
        {
            string output = string.Empty;
            string error = string.Empty;

            #region Create a new procees and open cmd with window hidden
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/c " + command);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.UseShellExecute = false;
            #endregion

            #region Read the output from console 
            Process process = Process.Start(processStartInfo);
            using (StreamReader streamReader = process.StandardOutput)
            {
                output = streamReader.ReadToEnd();
            }
            #endregion

            #region Read possible error from console
            using (StreamReader streamReader = process.StandardError)
            {
                error = streamReader.ReadToEnd();
            }
            #endregion

            #region If is an error then show an error window and return 
            if (!string.IsNullOrEmpty(error))
            {
                messageTitle = "Error";
                messageContent = error;
                messageButton = MessageBoxButtons.OK;
                MessageBox.Show(error, messageTitle, messageButton, MessageBoxIcon.Error);
                return null;
            }
            #endregion

            if(string.IsNullOrEmpty(output))
            {
                messageTitle = "Error";
                messageContent = "Cannot access command prompt";
                messageButton = MessageBoxButtons.OK;
                MessageBox.Show(error, messageTitle, messageButton, MessageBoxIcon.Error);
                return null;
            }

            return output;
        }

        /// <summary>
        /// Enable or disable internet connection sharing 
        /// </summary>
        /// <param name="publicNetworkInterface">name of public network interface</param>
        /// <param name="privateNetworkInterface">name of private network interface</param>
        /// <param name="enable">enable or disable</param>
        /// <returns></returns>
        public static void EnableDisableICS(string publicNetworkInterface, string privateNetworkInterface, bool enable)
        {
            bool found;
            dynamic sharingManager = default(dynamic);
            object collection = null;
            object item = null;
            dynamic entryConnection = default(dynamic);
            dynamic objNCProps;

            Type t = Type.GetTypeFromProgID("HNetCfg.HNetShare.1");
            sharingManager = System.Activator.CreateInstance(t);
            collection = sharingManager.EnumEveryConnection;

            foreach (object obj in (IEnumerable)collection)
            {
                item = obj;
                entryConnection = sharingManager.INetSharingConfigurationForINetConnection(item);
                objNCProps = sharingManager.NetConnectionProps(item);

                if (objNCProps.name == privateNetworkInterface)
                {
                    found = true;
                    if (enable)
                    {
                        entryConnection.EnableSharing(1);
                    }
                    else
                    {
                        entryConnection.DisableSharing();
                    }
                }
            }

            collection = sharingManager.EnumEveryConnection;

            foreach (object obj in (IEnumerable)collection)
            {
                item = obj;
                entryConnection = sharingManager.INetSharingConfigurationForINetConnection(item);
                objNCProps = sharingManager.NetConnectionProps(item);

                if (objNCProps.name == publicNetworkInterface)
                {
                    found = true;
                    if (enable)
                    {
                        try
                        {
                            entryConnection.EnableSharing(0);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                        }
                    }
                    else
                    {
                        try
                        {
                            entryConnection.DisableSharing();
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Check if a specific service exist
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool DoesServiceExist(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in services)
                if (service.ServiceName.Equals(serviceName))
                    return true;

            return false;
        }

        /// <summary>
        /// Start a list of services that are required for hotspot to work
        /// </summary>
        public static void StartServices()
        {
            HashSet<string> allServiceNames = new HashSet<string> { "Netman", "PlugPlay", "TapiSrv", "RpcSs", "RasMan", "ALG", "NlaSvc" };

            foreach (string serviceName in allServiceNames)
            {
                if (!DoesServiceExist(serviceName))
                {
                    Console.WriteLine("Service " + serviceName + " doesn't exist!");
                    continue;
                }

                var service = new ServiceController(serviceName);
                Console.WriteLine("Service name " + service.DisplayName + " Status: " + service.Status);
                if (service.Status.ToString() != "Running")
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                }
                Console.WriteLine("Service name " + service.DisplayName + " Status: " + service.Status);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Clear registers for existing hotspot profiles
        /// </summary>
        public static void ClearProfiles()
        {
            Console.WriteLine(CmdCommand("net stop wlansvc"));

            using (RegistryKey delKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\WlanSvc\Parameters\", true))
            {
                if (delKey != null)
                {
                    string[] keyNames = delKey.GetSubKeyNames();
                    foreach (string name in keyNames)
                    {
                        if (name == "HostedNetworkSettings")
                            delKey.DeleteSubKeyTree("HostedNetworkSettings");
                    }

                }
                delKey.Close();

            }

            Console.WriteLine(CmdCommand("net start wlansvc"));
        }

    }
}
