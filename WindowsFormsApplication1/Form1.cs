using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Comm ports driver
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public SerialPort ArduinoPort;
        string message;
        string returnMessage = "";
        const int messageLength = 16;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InfoBox.Text = "";
            // identyfy arduino
            thingy();
        }

        public void thingy()
        {
            ArduinoPort = new SerialPort("COM3");
            message = "IDENTIFY\n";

            ArduinoPort.Open();
            Sender(ArduinoPort);
            Thread.Sleep(500);

            returnMessage += ArduinoPort.ReadLine();
            if (returnMessage.Contains("CONNECTED"))
            {
                InfoBox.AppendText(returnMessage);
                returnMessage = "";
                // aTimer.Enabled = false;
            }
            else
            {
                InfoBox.AppendText("Arduino not found");
            }
        }

        public void Sender(SerialPort porto)
        {
            byte[] buffer = new byte[messageLength];
            int n = 0;
            try
            {
                foreach (char c in message)
                {
                    buffer[n] = Convert.ToByte(c);
                    n++;
                }
                porto.Write(buffer, 0, messageLength - 1);
            }
            catch (Exception ex)
            {
                InfoBox.Text += "Exception in conversion funktion:\r\n";
                InfoBox.Text += ex.ToString();
            }
        }
    }
}
