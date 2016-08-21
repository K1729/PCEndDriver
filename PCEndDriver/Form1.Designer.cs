namespace ArduinoDriver
{
    partial class ArduinoPrinter
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
            this.InfoBox = new System.Windows.Forms.TextBox();
            this.Identify = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Send = new System.Windows.Forms.Button();
            this.COMBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // InfoBox
            // 
            this.InfoBox.Location = new System.Drawing.Point(12, 25);
            this.InfoBox.Multiline = true;
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.InfoBox.Size = new System.Drawing.Size(846, 195);
            this.InfoBox.TabIndex = 5;
            this.InfoBox.WordWrap = false;
            // 
            // Identify
            // 
            this.Identify.Location = new System.Drawing.Point(139, 228);
            this.Identify.Name = "Identify";
            this.Identify.Size = new System.Drawing.Size(75, 23);
            this.Identify.TabIndex = 8;
            this.Identify.Text = "Identify";
            this.Identify.UseVisualStyleBackColor = true;
            this.Identify.Click += new System.EventHandler(this.button5_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Info Screen";
            // 
            // Send
            // 
            this.Send.Location = new System.Drawing.Point(783, 226);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(75, 23);
            this.Send.TabIndex = 12;
            this.Send.Text = "Send";
            this.Send.UseVisualStyleBackColor = true;
            // 
            // COMBox
            // 
            this.COMBox.FormattingEnabled = true;
            this.COMBox.Location = new System.Drawing.Point(12, 228);
            this.COMBox.Name = "COMBox";
            this.COMBox.Size = new System.Drawing.Size(121, 21);
            this.COMBox.TabIndex = 13;
            this.COMBox.DropDown += new System.EventHandler(this.COMBox_DropDown);
            // 
            // ArduinoPrinter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 261);
            this.Controls.Add(this.COMBox);
            this.Controls.Add(this.Send);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Identify);
            this.Controls.Add(this.InfoBox);
            this.MinimumSize = new System.Drawing.Size(273, 300);
            this.Name = "ArduinoPrinter";
            this.Text = "Arduino 3D Printer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox InfoBox;
        private System.Windows.Forms.Button Identify;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.ComboBox COMBox;
    }
}

