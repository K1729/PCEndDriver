namespace ArduinoDriver
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
            this.InfoBox = new System.Windows.Forms.TextBox();
            this.Indentify = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InfoBox
            // 
            this.InfoBox.Location = new System.Drawing.Point(12, 25);
            this.InfoBox.Multiline = true;
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(260, 166);
            this.InfoBox.TabIndex = 5;
            // 
            // Indentify
            // 
            this.Indentify.Location = new System.Drawing.Point(12, 197);
            this.Indentify.Name = "Indentify";
            this.Indentify.Size = new System.Drawing.Size(75, 23);
            this.Indentify.TabIndex = 8;
            this.Indentify.Text = "Identify";
            this.Indentify.UseVisualStyleBackColor = true;
            this.Indentify.Click += new System.EventHandler(this.button5_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Debug info";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Indentify);
            this.Controls.Add(this.InfoBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox InfoBox;
        private System.Windows.Forms.Button Indentify;
        private System.Windows.Forms.Label label3;
    }
}

