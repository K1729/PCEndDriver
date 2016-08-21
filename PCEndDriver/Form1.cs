using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Comm ports driver
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace ArduinoDriver
{
    public partial class  ArduinoPrinter : Form
    {
        public SerialPort ArduinoPort;
        string message;
        string returnMessage = "";
        string[] ports;
        public bool PortFound;
        const int messageLength = 16;

        public ArduinoPrinter()
        {
            InitializeComponent();
            this.AllowDrop = true;

            this.MaximumSize = new Size(int.MaxValue, int.MaxValue);
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);

            SetComPort();
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
                ArduinoPort = new SerialPort(COMBox.SelectedValue.ToString());
                message = "IDENTIFY\n";
                try
                {
                    ArduinoPort.Open();
                }
                catch (UnauthorizedAccessException ex)
                {
                    InfoBox.AppendText("Unauthorized Access Exception! Cannot open port " + COMBox.SelectedValue.ToString());
                    InfoBox.AppendText(Environment.NewLine);
                    InfoBox.AppendText(ex.ToString());
                }
                catch (Exception ex)
                {
                    InfoBox.AppendText("Unknown error: ");
                    InfoBox.AppendText(Environment.NewLine);
                    InfoBox.AppendText(ex.ToString());
                }
                Sender(ArduinoPort);
                Thread.Sleep(500);

                returnMessage += ArduinoPort.ReadLine();
                if (returnMessage.Contains("CONNECTED"))
                {
                    InfoBox.AppendText(returnMessage);
                    returnMessage = "";
                }
            }
            catch (Exception ex)
            {
                InfoBox.Text += "\nError: " + ex.ToString();
            }
        }

        // Send message to port
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

        // Events:

        // This event occurs when the user drags over the form with
        // the mouse during a drag and drop operation
        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Dataformat of the data can be accepted
            // (we only accept file drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy; // Okay
            else
                e.Effect = DragDropEffects.None;
        }

        // Occurs when the user releases the mouse over the drop target
        public void Form_DragDrop(object sender, DragEventArgs e)
        {
            // Extract the data from the DataObject-Container into a string list
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            // Tästä ekasta saadaan tuo osoite. Voitaisiin käyttää sitä tuon
            // tiedoston lukemiseen...

            InfoBox.Text = FileList[0];
            message = FileList[0];

            // Do something with the data...
            // lineReader(FileList[0]);
        }

        // This reads the text file line by line
        public void lineReader(string address)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(address);
            while((line = file.ReadLine()) != null)
            {
                try
                {
                    if (line.Length > 0)
                    {
                        if (line[0] != ';')
                        {
                            InfoBox.AppendText(counter + " " + line);
                            InfoBox.AppendText(Environment.NewLine);
                            counter++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    InfoBox.Text = ex.ToString();
                }
            }

            file.Close();
        }

        private void COMBox_DropDown(object sender, EventArgs e)
        {
            COMBox.Items.Clear();
            ports = SerialPort.GetPortNames();
            COMBox.Items.AddRange(ports);
        }
    }
}
