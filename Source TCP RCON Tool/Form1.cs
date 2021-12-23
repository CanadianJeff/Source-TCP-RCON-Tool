using System;
using System.Text;
using System.Windows.Forms;
using SimpleTCP;

namespace Source_TCP_RCON_Tool
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;

        private string StringToHex(string hexstring)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring)
            {
                //Note: X for upper, x for lower case letters
                sb.Append(Convert.ToInt32(t).ToString("x"));
            }
            return sb.ToString();
        }

        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
            txtMessage.Enabled = true;
            txtHost.Enabled = false;
            txtPort.Enabled = false;

            try
            {
                //Connect to server
                client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                txtMessage.Enabled = false;
                txtHost.Enabled = true;
                txtPort.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            //Update message to txtStatus
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;
                txtStatus.SelectionStart = txtStatus.Text.Length; txtStatus.ScrollToCaret();
            });
        }

        private void btnSend_Click_1(object sender, EventArgs e)
        {
            var txt = txtMessage.Text;
            try
            {
                client.Write($"{txt}\r\n");
            }
            catch (Exception)
            {
                MessageBox.Show("Are you still connected?");
            }


            txtStatus.Text += "] ";
            txtStatus.Text += txtMessage.Text;
            txtStatus.Text += "\r\n";
            txtMessage.Text = "";
            }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            btnDisconnect.Enabled = false;
            btnConnect.Enabled = true;
            txtMessage.Enabled = false;
            txtHost.Enabled = true;
            txtPort.Enabled = true;
            client.Disconnect();
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            //var textboxSender = (TextBox)sender;
            //var cursorPosition = textboxSender.SelectionStart;
            //textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9a-zA-Z ]", "");
            //textboxSender.SelectionStart = cursorPosition;
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSend_Click_1(this, new EventArgs());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStatus.Text = "";
        }
    }
}