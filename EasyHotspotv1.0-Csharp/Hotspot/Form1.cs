using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Hotspot
{
    public partial class form_main : Form
    {
        public form_main()
        {
            InitializeComponent();
        }

        //  If text from hotspot name has changed then change color and style for the font
        private void textbox_name_TextChanged(object sender, EventArgs e)
        {
            if (textbox_name.Text.Length <= 1)
            {
                textbox_name.ForeColor = Color.Black;
                textbox_name.Font = new Font(textbox_name.Font, FontStyle.Bold);
            }
        }

        //  If text from password has changed then change color and style for the font
        private void textbox_password_TextChanged(object sender, EventArgs e)
        {
            if (textbox_password.Text.Length <= 1)
            {
                textbox_password.PasswordChar = '*';
                textbox_password.ForeColor = Color.Black;
                textbox_password.Font = new Font(textbox_name.Font, FontStyle.Bold);
            }
        }

        //  A function that execute a cmd command and return the output from console
        private string CmdCommand(string command)
        {
            string output = string.Empty;
            string error = string.Empty;

            string title;
            string message;
            MessageBoxButtons buttons;

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
                title = "Error";
                message = error;
                buttons = MessageBoxButtons.OK;
                MessageBox.Show(error, title, buttons, MessageBoxIcon.Error);
                return string.Empty;
            }
            #endregion

            return output;
        }

        //  A function that check if hosted network is supported
        private void button_test_Click(object sender, EventArgs e)
        {
            string title = string.Empty;
            string message = string.Empty;
            MessageBoxButtons buttons;

            string output = CmdCommand("netsh wlan show drivers");
            if (string.IsNullOrEmpty(output))
                return;

            /*  If output has this text then hosted network is supported and it will  
              show a window with that information */
            if (output.Contains("Hosted network supported: Yes"))
            {
                title = "Device Mode";
                message = "Wi-Fi device available";
                buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
            }
        }

        //  A function that start the hotspot
        private void button_start_Click(object sender, EventArgs e)
        {
            string title = string.Empty;
            string message = string.Empty;
            MessageBoxButtons buttons;

            //  Make hosted network available
            string output = CmdCommand("netsh wlan set hostednetwork mode=allow");


            //  Start the hosted newtork
            output = CmdCommand("netsh wlan start hostednetwork");
            if (string.IsNullOrEmpty(output))
                return;

            //  If output has this text then show an error and return
            if (output.Contains("The hosted network couldn't be started."))
            {
                title = "Error";
                message = "Wi-Fi Device is off";
                buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                return;
            }

            //  Stop the hosted network
            output = CmdCommand("netsh wlan stop hostednetwork");
            if (string.IsNullOrEmpty(output)) return;

            label_status.Text = "Starting...";

            //  Check hotspot name and password if had enough characters
            if (string.IsNullOrEmpty(textbox_name.Text))
            {
                title = "Invalid name";
                message = "Please enter a name for hotspot";
                buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                label_status.Text = "Idle...";
                return;
            }
            else if (string.IsNullOrEmpty(textbox_password.Text) || textbox_password.Text.Length < 8)
            {
                title = "Invalid password";
                message = "Please enter a password with minimum 8 characters";
                buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                label_status.Text = "Idle...";
                return;
            }

            //  Set a name and a password for this hosted network 
            output = CmdCommand("netsh wlan set hostednetwork mode=allow ssid=" + textbox_name.Text + " key=" + textbox_password.Text);
            if (string.IsNullOrEmpty(output)) return;

            //  Start hosted network
            output = CmdCommand("netsh wlan start hostednetwork");
            if (string.IsNullOrEmpty(output)) return;

            label_status.Text = "Hotspot started";


            title = "Last Steps";
            message = "1. Go to \"Network and Sharing Center\"\n2. Click on \"Change adapter settings\"\n3. Right click on the adapter from which you use internet\n4. Click \"Properties\"\n5. Select the \"Sharing\" tab\n5. Tick on the \"Allow other network users to connect ...\"\n6. Choose the wi-fi adapter that was created";
            buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
        }

        //  A function that stop the hotspot
        private void button_stop_Click(object sender, EventArgs e)
        {
            string output = string.Empty;
            output = CmdCommand("netsh wlan stop hostednetwork");
            if (string.IsNullOrEmpty(output)) return;

            label_status.Text = "Hotspot has stopped";
        }

        private void button_about_Click(object sender, EventArgs e)
        {
            string title = string.Empty;
            string message = string.Empty;
            MessageBoxButtons buttons;


            title = " ";
            message = "Program name: Easy Hotspot v1.0\nCreated by: Ghiga Alexandru\nDate: 21/03/2016\nE-mail: ghalexandru@outlook.com";
            buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
        }
    }
}
