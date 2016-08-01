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
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace ArduinoDriver
{
    public partial class  Form1 : Form
    {
        public SerialPort CurrentPort;
        public bool PortFound;
        public int Bit2;
        public int Bit4;
        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            InfoBox.Text = "";
            // identyfy arduino
            SetComPort();
        }

        public void SetComPort()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    CurrentPort = new SerialPort(port, 9600);
                    if (DetectArduino())
                    {
                        PortFound = true;
                        break;
                    }
                    else
                    {
                        PortFound = false;
                    }
                }
            }
            catch (Exception ex)
            {
                InfoBox.Text += "\nError: " + ex.ToString();
            }
            if (PortFound == true)
            {
                InfoBox.Text += "\nArduino found";
            }
            else
            {
                InfoBox.Text += "\nArduino not found";
            }
        }

        public void Messenger(string message)
        {

        }

        public bool DetectArduino()
        {
            try
            {
                // The below settings are for the Hello handshake
                int IntReturnASCII = 0;
                string message = "IDENTIFY";
                int count = 0;
                string returnMessage = "";
                byte[] buffer = new byte[8];

                
                while (returnMessage != "RESEND")
                {
                    int n = 0;
                    try
                    {
                        foreach (char c in message.ToCharArray(0, 8))
                        {
                            buffer[n] = Convert.ToByte(message.ToCharArray(n, n + 1)[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        InfoBox.Text += "Exception in conversion funktion:\n";
                        InfoBox.Text += ex.ToString();
                    }
                }

                CurrentPort.Open();
                CurrentPort.Write(buffer, 0, 8);
                // Koita korvata tämä jollakin muulla
                Thread.Sleep(500);
                count = CurrentPort.BytesToRead;
                while (count > 0)
                {
                    IntReturnASCII = CurrentPort.ReadByte();
                    returnMessage += Convert.ToChar(IntReturnASCII);
                    count--;
                }
                InfoBox.Text += returnMessage;

                CurrentPort.Close();

                if (returnMessage.Contains("CONNECTED"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                InfoBox.Text += "Error in DetectAdruino():\n";
                InfoBox.Text += ex.ToString();
                return false;
            }
        }

        public void MotorControl(int bit1, int bit2, int bit3,  int bit4)
        {
            try
            {
                // The below settings are for the Hello handshake
                byte[] buffer = new byte[6];
                buffer[0] = Convert.ToByte(255);
                buffer[1] = Convert.ToByte(1);
                buffer[2] = Convert.ToByte(bit1);
                buffer[3] = Convert.ToByte(bit2);
                buffer[4] = Convert.ToByte(bit3);
                buffer[5] = Convert.ToByte(bit4);

                CurrentPort.Open();
                CurrentPort.Write(buffer, 0, 6);
                CurrentPort.Close();
            }
            catch (Exception ex)
            {
                InfoBox.Text = "Error" + ex.ToString();
            }
        }
    }
}
