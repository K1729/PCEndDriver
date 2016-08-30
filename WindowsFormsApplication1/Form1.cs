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

using WindowsFormsApplication1.Model;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private readonly object _lock = new object();
        private readonly Queue<Instruction> _instructions = new Queue<Instruction>();
        private readonly AutoResetEvent _signal = new AutoResetEvent(false);

        public SerialPort ArduinoPort;
        public delegate void passString( string myString );
        public passString myDelegate;
        string message;
        string returnMessage = "";
        const int messageLength = 16;

        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            myDelegate = new passString(infoBoxUpdater);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InfoBox.Text = "";
            // identify arduino
            thingy();
        }

        private void Send_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                _instructions.Clear();
                message = MessageBox.Text;
                backgroundWorker1.RunWorkerAsync(COMBox.SelectedItem.ToString());
            }
            else
            {
                InfoBox.AppendText("Worker is busy...\r\n");
                InfoBox.AppendText("Leaving instructions in queue.\r\n");
                Instruction newInstruction = new Instruction(true, message);

                // you need to make sure only
                // one thread can access the list
                // at a time
                lock (_lock)
                {
                    _instructions.Enqueue(newInstruction);
                }

                Thread.Sleep(100);
                // notify the waiting thread
                _signal.Set();
            }
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

        private void infoBoxUpdater(string myString)
        {
            InfoBox.AppendText(myString + "\r\n");
        }

        // This is background worker. It does the loop and keeps port alive.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            SerialPort sp = new SerialPort((string)e.Argument);
            Instruction command = null;

            string messageArduino = "";
            bool done = false;
            Sender(sp);

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
                    if (sp.BytesToRead > 0)
                    {
                        Thread.Sleep(200);
                        messageArduino += sp.ReadExisting();
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

                    if (command.newCommand)
                    {
                        message = command.ToString();
                        Sender(sp);
                        command.newCommand = false;
                    }

                    else if (_instructions.Count > 0)
                    {
                        _signal.WaitOne();
                        command = null;
                        lock (_lock)
                        {
                            command = _instructions.Dequeue();
                        }
                    }

                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
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
