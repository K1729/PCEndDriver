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
        }

        // Sets the default port
        public void SetComPort()
        {
            try
            {
                DebugBox.AppendText(COMBox.SelectedItem.ToString());
                DebugBox.AppendText(Environment.NewLine);
                ArduinoPort = new SerialPort(COMBox.SelectedItem.ToString(), 9600, Parity.None, 8, StopBits.One);
                ArduinoPort.Handshake = Handshake.None;
                ArduinoPort.RtsEnable = true;
                message = "IDENTIFY";
                try
                {
                    ArduinoPort.Open();
                    Thread.Sleep(200);
                }
                catch (UnauthorizedAccessException ex)
                {
                    DebugBox.AppendText("Unauthorized Access Exception! Cannot open port " + COMBox.SelectedValue.ToString());
                    DebugBox.AppendText(Environment.NewLine);
                    DebugBox.AppendText(ex.ToString());
                }
                catch (Exception ex)
                {
                    DebugBox.AppendText("Unknown error: ");
                    DebugBox.AppendText(Environment.NewLine);
                    DebugBox.AppendText(ex.ToString());
                }
                Sender(ArduinoPort);

                returnMessage += ArduinoPort.ReadLine();
                if (returnMessage.Contains("CONNECTED"))
                {
                    InfoBox.AppendText(returnMessage);
                    returnMessage = "";
                    try
                    {
                        using (ArduinoPort)
                        {
                            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReveiced);
                            DebugBox.AppendText("Created serial event handler");
                            DebugBox.AppendText(Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugBox.AppendText("Could not create serial event handler");
                        DebugBox.AppendText(Environment.NewLine);
                        DebugBox.AppendText(ex.ToString());
                        DebugBox.AppendText(Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugBox.Text += "\nError: " + ex.ToString();
            }
        }

        // Send message to port
        public void Sender(SerialPort porto)
        {
            byte[] buffer = new byte[messageLength];
            int n = 0;
            message = message + '\n';
            try
            {
                DebugBox.AppendText("BreakState: " + ArduinoPort.BreakState + "\r\n");
                DebugBox.AppendText("Clear-to-Send line: " + ArduinoPort.CtsHolding + "\r\n");
                DebugBox.AppendText("CanRaiseEvent: " + CanRaiseEvents + "\r\n");
                foreach (char c in message)
                {
                    buffer[n] = Convert.ToByte(c);
                    n++;
                }
                porto.Write(buffer, 0, messageLength - 1);
                Thread.Sleep(500);
                if (ArduinoPort.BytesToWrite == 0)
                {
                    DebugBox.AppendText("Message sent");
                    DebugBox.AppendText(Environment.NewLine);
                }
                else
                {
                    DebugBox.AppendText("Message has not yet been sent\r\n");
                }
            }
            catch (Exception ex)
            {
                DebugBox.AppendText("Exception in conversion funktion:");
                DebugBox.AppendText(Environment.NewLine);
                DebugBox.AppendText(ex.ToString());
                DebugBox.AppendText(Environment.NewLine);
            }
        }

        // This reads the text file line by line
        public void lineReader(string address)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(address);
            while ((line = file.ReadLine()) != null)
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
                    DebugBox.Text = ex.ToString();
                }
            }

            file.Close();
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

            DebugBox.Text = FileList[0];

            // Do something with the data...
            lineReader(FileList[0]);
        }

        // Reads the buffer if the event doesn't fire
        private void Read_Click(object sender, EventArgs e)
        {
            // Got error here. "Port is closed!"
            // ArduinoPort.Open();
            // This means the port closed before the arduino could respond!
            returnMessage += (string)ArduinoPort.ReadExisting();

            DebugBox.AppendText("Message received.");
            DebugBox.AppendText(Environment.NewLine);

            InfoBox.AppendText(returnMessage);
            InfoBox.AppendText(Environment.NewLine);
            // lineReader(message);
        }

        public void OnDataReveiced(object sender, SerialDataReceivedEventArgs e)
        {
            using (ArduinoPort)
            {
                Thread.Sleep(1);

                SerialPort sp = (SerialPort)sender;
                returnMessage += (string)sp.ReadExisting();

                if (DebugBox.InvokeRequired)
                {
                    DebugBox.Invoke(new Action(() =>
                    {
                        DebugBox.AppendText("Message received.");
                        DebugBox.AppendText(Environment.NewLine);
                    }));
                }

                if (InfoBox.InvokeRequired)
                {
                    InfoBox.Invoke(new Action(() => {
                        InfoBox.AppendText(returnMessage);
                        InfoBox.AppendText(Environment.NewLine);
                    }));
                }
            }
        }

        // Identify button
        private void button5_Click(object sender, EventArgs e)
        {
            InfoBox.Text = "";
            // identyfy arduino
            SetComPort();
        }

        // This updates the dropdown list
        private void COMBox_DropDown(object sender, EventArgs e)
        {
            COMBox.Items.Clear();
            ports = SerialPort.GetPortNames();
            COMBox.Items.AddRange(ports);
        }

        // This sends message recorded in MessageBox.
        private void Send_Click(object sender, EventArgs e)
        {
            DebugBox.AppendText("Trying to send message:");
            DebugBox.AppendText(Environment.NewLine);
            message = MessageBox.Text;
            DebugBox.AppendText(message);
            DebugBox.AppendText(Environment.NewLine);
            ArduinoPort.Close();
            using (ArduinoPort)
            {
                try
                {
                    ArduinoPort.Open();
                }
                catch (Exception ex)
                {
                    DebugBox.AppendText("Error in Send event:");
                    DebugBox.AppendText(Environment.NewLine);
                    DebugBox.AppendText(ex.ToString());
                    DebugBox.AppendText(Environment.NewLine);
                }
                Sender(ArduinoPort);
            }
        }
    }
}
