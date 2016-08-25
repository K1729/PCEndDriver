namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.InfoBox = new System.Windows.Forms.TextBox();
            this.COMBox = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MessageBox = new System.Windows.Forms.TextBox();
            this.Send = new System.Windows.Forms.Button();
            this.cancelAsyncButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InfoBox
            // 
            this.InfoBox.Location = new System.Drawing.Point(12, 12);
            this.InfoBox.Multiline = true;
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(202, 208);
            this.InfoBox.TabIndex = 1;
            // 
            // COMBox
            // 
            this.COMBox.FormattingEnabled = true;
            this.COMBox.Location = new System.Drawing.Point(223, 28);
            this.COMBox.Name = "COMBox";
            this.COMBox.Size = new System.Drawing.Size(121, 21);
            this.COMBox.TabIndex = 2;
            this.COMBox.DropDown += new System.EventHandler(this.COMBox_DropDown);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(220, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "COM port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Message:";
            // 
            // MessageBox
            // 
            this.MessageBox.Location = new System.Drawing.Point(223, 68);
            this.MessageBox.Name = "MessageBox";
            this.MessageBox.Size = new System.Drawing.Size(121, 20);
            this.MessageBox.TabIndex = 5;
            // 
            // Send
            // 
            this.Send.Location = new System.Drawing.Point(223, 94);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(121, 23);
            this.Send.TabIndex = 6;
            this.Send.Text = "Send Message";
            this.Send.UseVisualStyleBackColor = true;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            // 
            // cancelAsyncButton
            // 
            this.cancelAsyncButton.Location = new System.Drawing.Point(223, 123);
            this.cancelAsyncButton.Name = "cancelAsyncButton";
            this.cancelAsyncButton.Size = new System.Drawing.Size(121, 23);
            this.cancelAsyncButton.TabIndex = 7;
            this.cancelAsyncButton.Text = "Stop Listening";
            this.cancelAsyncButton.UseVisualStyleBackColor = true;
            this.cancelAsyncButton.Click += new System.EventHandler(this.cancelAsyncButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 261);
            this.Controls.Add(this.cancelAsyncButton);
            this.Controls.Add(this.Send);
            this.Controls.Add(this.MessageBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.COMBox);
            this.Controls.Add(this.InfoBox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox InfoBox;
        private System.Windows.Forms.ComboBox COMBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MessageBox;
        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.Button cancelAsyncButton;
    }
}

