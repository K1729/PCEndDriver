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
        public delegate void passString( string myString );
        public passString myDelegate;
        string message;
        string returnMessage = "";
        const int messageLength = 16;
        bool done;

        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerSupportsCancellation = true;
            done = false;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            myDelegate = new passString(infoBoxUpdater);
        }

        private void infoBoxUpdater(string myString)
        {
            InfoBox.AppendText(myString + "\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InfoBox.Text = "";
            // identify arduino
            thingy();
        }

        public void thingy()
        {
            ArduinoPort = new SerialPort(COMBox.SelectedItem.ToString());
            message = "IDENTIFY";

            Sender(ArduinoPort);
            Thread.Sleep(200);

            returnMessage += ArduinoPort.ReadLine();
            if (returnMessage.Contains("CONNECTED"))
            {
                InfoBox.AppendText(returnMessage);
                returnMessage = "";
                // Watcher.ArduinoPort = ArduinoPort;
                // tWatcher = new Thread(new ThreadStart(Watcher.Listener));
            }
            else
            {
                InfoBox.AppendText("Arduino not found");
            }
            ArduinoPort.Close();
        }

        // Sender opens the port. So it must be closed afterwards.
        public void Sender(SerialPort porto)
        {
            byte[] buffer = new byte[messageLength];
            int n = 0;
            message = message + '\n';
            try
            {
                foreach (char c in message)
                {
                    buffer[n] = Convert.ToByte(c);
                    n++;
                }
                try
                {
                    porto.Open();
                }
                catch { }
                Thread.Sleep(200);
                porto.Write(buffer, 0, messageLength - 1);
            }
            catch (Exception ex)
            {
                if (InfoBox.InvokeRequired)
                {
                    InfoBox.Invoke(myDelegate, new object[] { ("Error in sender: " + ex.ToString()) });
                }
                else
                {
                    InfoBox.Text += "Exception in conversion funktion:\r\n";
                    InfoBox.Text += ex.ToString();
                }
            }
        }

        // Updates COMBox dropdown list when invoked
        private void COMBox_DropDown(object sender, EventArgs e)
        {
            COMBox.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            COMBox.Items.AddRange(ports);
        }

        private void Send_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                message = MessageBox.Text;
                backgroundWorker1.RunWorkerAsync(COMBox.SelectedItem.ToString());
            }
            else
            {
                InfoBox.AppendText("Worker busy\r\n");
            }
        }

        // Stops background worker
        private void cancelAsyncButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                backgroundWorker1.CancelAsync();
            }
        }

        // This is background worker. It does the loop and keeps port alive.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            SerialPort sp = new SerialPort((string)e.Argument);

            string messageArduino = "";
            Sender(sp);
            Thread.Sleep(200);

            while (!done)
            {
                if (worker.CancellationPending == true)
                {
                    done = true;
                    if (InfoBox.InvokeRequired)
                    {
                        InfoBox.Invoke(myDelegate, new object[] { ("Return message: " + messageArduino) });
                    }
                    else
                    {
                        InfoBox.Text += "Return message: " + messageArduino + "\r\n";
                    }
                    e.Result = messageArduino;
                    sp.Close();
                    e.Cancel = true;
                    break;
                }
                else
                {
                    messageArduino += sp.ReadExisting();
                    Thread.Sleep(1000);
                    if (InfoBox.InvokeRequired)
                    {
                        InfoBox.Invoke(myDelegate, new object[] { ("Return message: " + messageArduino) });
                    }
                    else
                    {
                        InfoBox.Text += "Return message: \r\n";
                        InfoBox.Text += messageArduino;
                    }
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            returnMessage = (string)e.Result;
            InfoBox.AppendText("Background worker successfully shut down.\r\n");
            InfoBox.AppendText("Return message: " + returnMessage + "\r\n");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            InfoBox.AppendText("Return message: " + e.ProgressPercentage.ToString());
        }
    }
}
