using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.ServiceProcess;
using System.Windows.Forms;


namespace Hotspot
{
    public partial class Main : Form
    {
        private string publicNetworkInterface;
        private string privateNetworkInterface;

        private static string messageTitle = String.Empty;
        private static string messageContent = String.Empty;
        private static MessageBoxButtons messageButton;

        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start hotspot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_start_Click(object sender, EventArgs e)
        {
            Util.ClearProfiles();
            Util.StartServices();

            publicNetworkInterface = Util.GetNameOfPublicNetworkInterface();
            if (publicNetworkInterface == null)
                return;

            string output = String.Empty;

            output = Util.CmdCommand("netsh wlan show drivers");
            if (!(output.Contains("Hosted network supported: Yes") || output.Contains("Hosted network supported  : Yes") || output.Contains("Hosted network supported : Yes")))
            {
                //  Make hosted network available
                output = Util.CmdCommand("netsh wlan set hostednetwork mode=allow");
                output = Util.CmdCommand("netsh wlan show drivers");

                if (!(output.Contains("Hosted network supported: Yes") || output.Contains("Hosted network supported  : Yes") || output.Contains("Hosted network supported : Yes")))
                {
                    messageTitle = "Your network driver is not compatible or is outdated";
                    messageContent = output;
                    messageButton = MessageBoxButtons.OK;
                    MessageBox.Show(messageContent, messageTitle, messageButton, MessageBoxIcon.Error);
                    return;
                }
            }

            //  If output has this text then show an error and return
            if (output.Contains("The hosted network couldn't be started."))
            {
                messageTitle = "Error";
                messageContent = output;
                messageButton = MessageBoxButtons.OK;
                MessageBox.Show(messageContent, messageTitle, messageButton, MessageBoxIcon.Error);
                return;
            }

            //  Stop the hosted network
            output = Util.CmdCommand("netsh wlan stop hostednetwork");
            if (string.IsNullOrEmpty(output)) return;

            label_status.Text = "Starting...";

            //  Check hotspot name and password if had enough characters
            if (string.IsNullOrEmpty(textbox_name.Text))
            {
                messageTitle = "Invalid name";
                messageContent = "Please enter a name for hotspot";
                messageButton = MessageBoxButtons.OK;
                MessageBox.Show(messageContent, messageTitle, messageButton, MessageBoxIcon.Warning);
                label_status.Text = "Idle...";
                return;
            }
            else if (string.IsNullOrEmpty(textbox_password.Text) || textbox_password.Text.Length < 8)
            {
                messageTitle = "Invalid password";
                messageContent = "Please enter a password with minimum 8 characters";
                messageButton = MessageBoxButtons.OK;
                MessageBox.Show(messageContent, messageTitle, messageButton, MessageBoxIcon.Warning);
                label_status.Text = "Idle...";
                return;
            }

            //  Set a name and a password for this hosted network 
            output = Util.CmdCommand("netsh wlan set hostednetwork mode=allow ssid=" + textbox_name.Text + " key=" + textbox_password.Text);
            if (string.IsNullOrEmpty(output)) return;

            //  Start hosted network
            output = Util.CmdCommand("netsh wlan start hostednetwork");
            if (string.IsNullOrEmpty(output)) return;

            label_status.Text = "Hotspot started";

            privateNetworkInterface = Util.GetNameOfPrivateNetworkInterface(publicNetworkInterface);
            if (privateNetworkInterface == null)
            {
                output = Util.CmdCommand("netsh wlan stop hostednetwork");
                label_status.Text = "Hotspot has stopped";
                return;
            }

            try
            {
                Util.EnableDisableICS(publicNetworkInterface, privateNetworkInterface, true);
            }

            catch (Exception ex)
            {
                messageTitle = "Error";
                messageContent = ex.Message;
                messageButton = MessageBoxButtons.OK;
                MessageBox.Show(messageContent, messageTitle, messageButton, MessageBoxIcon.Error);

                output = Util.CmdCommand("netsh wlan stop hostednetwork");
                label_status.Text = "Hotspot has stopped";
                return;
            }
        }

        /// <summary>
        /// Stop hotspot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_stop_Click(object sender, EventArgs e)
        {
            string output = string.Empty;
            output = Util.CmdCommand("netsh wlan stop hostednetwork");
            if (string.IsNullOrEmpty(output)) return;

            try
            {
                Util.EnableDisableICS(publicNetworkInterface, privateNetworkInterface, false);
            }
            catch (Exception ex)
            {
                messageTitle = "Error";
                messageContent = ex.Message;
                messageButton = MessageBoxButtons.OK;
                MessageBox.Show(messageContent, messageTitle, messageButton, MessageBoxIcon.Error);

                output = Util.CmdCommand("netsh wlan stop hostednetwork");
                label_status.Text = "Hotspot has stopped";
                return;
            }

            label_status.Text = "Hotspot has stopped";
        }

        //private void button_about_Click(object sender, EventArgs e)
        //{
        //    string messageTitle = string.Empty;
        //    string messageContent = string.Empty;
        //    MessageBoxButtons messageButton;
 
        //    messageTitle = " ";
        //    messageContent = "Program name: Easy Hotspot v1.0\nCreated by: Ghiga Alexandru\nDate: 21/03/2016\nE-mail: ghalexandru@outlook.com";
        //    messageButton = MessageBoxButtons.OK;
        //    MessageBox.Show(messageContent, messageTitle, messageButton, MessageBoxIcon.Information);
        //}

        private void textbox_name_MouseClick(object sender, MouseEventArgs e)
        {
            if (textbox_name.Text == "enter hotspot name")
            {
                textbox_name.Text = String.Empty;
                textbox_name.ForeColor = Color.Black;
                textbox_name.Font = new Font(textbox_name.Font, FontStyle.Bold);
            }
        }

        private void textbox_password_Click(object sender, EventArgs e)
        {
            if (textbox_password.Text == "enter password")
            {
                textbox_password.Text = String.Empty;
                textbox_password.PasswordChar = '*';
                textbox_password.ForeColor = Color.Black;
                textbox_password.Font = new Font(textbox_name.Font, FontStyle.Bold);
            }
        }

        /// <summary>
        /// If text from hotspot name is changed then change color and style for the font
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox_name_TextChanged(object sender, EventArgs e)
        {
            if (textbox_name.Text.Length <= 1)
            {
                textbox_name.ForeColor = Color.Black;
                textbox_name.Font = new Font(textbox_name.Font, FontStyle.Bold);
            }
        }

        /// <summary>
        /// If text from hotspot password is changed then change color and style for the font
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox_password_TextChanged(object sender, EventArgs e)
        {
            if (textbox_password.Text.Length <= 1)
            {
                textbox_password.PasswordChar = '*';
                textbox_password.ForeColor = Color.Black;
                textbox_password.Font = new Font(textbox_name.Font, FontStyle.Bold);
            }
        }
    }
}
