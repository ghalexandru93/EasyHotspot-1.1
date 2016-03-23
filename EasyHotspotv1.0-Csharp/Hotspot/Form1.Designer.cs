namespace Hotspot
{
    partial class form_main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_name = new System.Windows.Forms.Label();
            this.label_password = new System.Windows.Forms.Label();
            this.textbox_name = new System.Windows.Forms.TextBox();
            this.textbox_password = new System.Windows.Forms.TextBox();
            this.button_test = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.label_status = new System.Windows.Forms.Label();
            this.button_about = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_name.Location = new System.Drawing.Point(13, 13);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(87, 13);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "Hotspot Name";
            // 
            // label_password
            // 
            this.label_password.AutoSize = true;
            this.label_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_password.Location = new System.Drawing.Point(13, 40);
            this.label_password.Name = "label_password";
            this.label_password.Size = new System.Drawing.Size(61, 13);
            this.label_password.TabIndex = 1;
            this.label_password.Text = "Password";
            // 
            // textbox_name
            // 
            this.textbox_name.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textbox_name.Location = new System.Drawing.Point(128, 5);
            this.textbox_name.Name = "textbox_name";
            this.textbox_name.Size = new System.Drawing.Size(147, 20);
            this.textbox_name.TabIndex = 2;
            this.textbox_name.Text = "enter hotspot name";
            this.textbox_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textbox_name.TextChanged += new System.EventHandler(this.textbox_name_TextChanged);
            // 
            // textbox_password
            // 
            this.textbox_password.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textbox_password.Location = new System.Drawing.Point(128, 40);
            this.textbox_password.Name = "textbox_password";
            this.textbox_password.Size = new System.Drawing.Size(147, 20);
            this.textbox_password.TabIndex = 3;
            this.textbox_password.Text = "enter password";
            this.textbox_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textbox_password.TextChanged += new System.EventHandler(this.textbox_password_TextChanged);
            // 
            // button_test
            // 
            this.button_test.Location = new System.Drawing.Point(10, 73);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(75, 23);
            this.button_test.TabIndex = 4;
            this.button_test.Text = "Test";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(91, 73);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 5;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(172, 73);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 23);
            this.button_stop.TabIndex = 6;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // label_status
            // 
            this.label_status.AccessibleName = "label_status";
            this.label_status.AutoSize = true;
            this.label_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_status.Location = new System.Drawing.Point(154, 106);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(33, 13);
            this.label_status.TabIndex = 8;
            this.label_status.Text = "Idle...";
            this.label_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_about
            // 
            this.button_about.Location = new System.Drawing.Point(253, 73);
            this.button_about.Name = "button_about";
            this.button_about.Size = new System.Drawing.Size(75, 23);
            this.button_about.TabIndex = 9;
            this.button_about.Text = "About";
            this.button_about.UseVisualStyleBackColor = true;
            this.button_about.Click += new System.EventHandler(this.button_about_Click);
            // 
            // form_main
            // 
            this.AccessibleName = "label_status";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 128);
            this.Controls.Add(this.button_about);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.button_test);
            this.Controls.Add(this.textbox_password);
            this.Controls.Add(this.textbox_name);
            this.Controls.Add(this.label_password);
            this.Controls.Add(this.label_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "form_main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Easy Hotspot v1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_password;
        private System.Windows.Forms.TextBox textbox_name;
        private System.Windows.Forms.TextBox textbox_password;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Button button_about;
    }
}

