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
        public static System.Timers.Timer aTimer;
        public SerialPort ArduinoPort;
        public SerialPort[] SerialPorts;
        string message;
        string returnMessage = "";
        public bool PortFound;
        const int messageLength = 16;

        public ArduinoPrinter()
        {
            InitializeComponent();
            this.AllowDrop = true;
            
            this.MaximumSize = new Size(int.MaxValue, int.MaxValue);
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);

            // Timer settings
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 500;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;

            SetComPort();
            // ArduinoPort.BaudRate = 9600;
            // ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
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
                Identify.Enabled = false;
                string[] ports = SerialPort.GetPortNames();

                for(int i = 0; i < (ports.Count() - 1); i++)
                {
                    SerialPorts[i].PortName = ports[i];
                    message = "IDENTIFY\n";

                    SerialPorts[i].Open();
                    SerialPorts[i].DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    Sender(SerialPorts[i]);
                }
                aTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                InfoBox.Text += "\nError: " + ex.ToString();
                Identify.Enabled = true;
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

        // Closes all ports
        public void ClosePorts()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                SerialPort porto = new SerialPort(port, 9600);
                porto.Close();
            }
        }

        // Events:

        // Fires when aTimer elapsed event is called
        public void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            ClosePorts();
            aTimer.Enabled = false;
        }

        // This reads the data and saves it to returnMessage
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            returnMessage += sp.ReadExisting();
            if (returnMessage.Contains("CONNECTED"))
            {
                returnMessage = "";
                ArduinoPort = sp;
                ClosePorts();
                ArduinoPort.Open();
                aTimer.Enabled = true;
            }
            else if (returnMessage.Contains("RESEND"))
            {
                returnMessage = "";
                ArduinoPort = sp;
                message = "IDENTIFY\n";
                Sender(sp);
            }
            else if (returnMessage.Count() > 16)
            {
                returnMessage = "";
                message = "RESEND\n";
                Sender(sp);
            }
        }

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
    }
}
