using SimpleTCP;
using System;
using System.Text;
using System.Windows.Forms;

namespace TCPIPDemo
{
    public partial class frmServer : Form
    {
        public frmServer()
        {
            InitializeComponent();
        }

        SimpleTcpServer server;
        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13;
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += "\r\nEu digitei:" + e.MessageString;
                e.ReplyLine(string.Format("Você disse: {0}\r\n", e.MessageString));
            });
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            btnIniciar.Enabled = false;
            btnParar.Enabled = true;
            txtHost.ReadOnly = true;
            txtPort.ReadOnly = true;
            txtStatus.Text += "Iniciando servidor...\r\n";

            System.Net.IPAddress ip = System.Net.IPAddress.Parse(txtHost.Text);
            server.Start(ip, Convert.ToInt32(txtPort.Text));
            txtStatus.Text += "Servidor iniciado\r\n";
        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            if (server.IsStarted) {  
                server.Stop();
                txtStatus.Text += "Servidor encerrado\r\n";
                btnIniciar.Enabled = true;
                btnParar.Enabled = false;
                txtHost.ReadOnly = false;
                txtPort.ReadOnly = false;
            }
        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {

        }
    }
}