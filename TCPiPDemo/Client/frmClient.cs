using SimpleTCP;
using System;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class frmClient : Form
    {
        public frmClient()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;

        private void btnConectar_Click(object sender, EventArgs e)
        {
            btnConectar.Enabled = false;
            btnDesconectar.Enabled = true;
            txtMensagem.Enabled = true;
            txtHost.ReadOnly = true;
            txtPort.ReadOnly = true;
            client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
            txtStatus.Text += "Conectado ao servidor.\r\n";
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            client.Disconnect();
            txtStatus.Text += "Desconectado do servidor com sucesso.\r\n";
            btnConectar.Enabled = true;
            btnDesconectar.Enabled = false;
            txtMensagem.Enabled = false;
            txtHost.ReadOnly = false;
            txtPort.ReadOnly = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;
            });
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply(txtMensagem.Text, TimeSpan.FromSeconds(3));
        }

        private void txtMensagem_TextChanged(object sender, EventArgs e)
        {

        }

    }
}