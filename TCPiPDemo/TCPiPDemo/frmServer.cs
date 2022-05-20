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
        string nome = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13;
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            if(nome == "")
            {
                nome = e.MessageString;
                return;
            }

            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += string.Format("\r\n {0} disse: {1}", nome, e.MessageString);
                if (nome == nome)
                {
                    e.Reply(string.Format("\r\n Você disse: {0}", e.MessageString));
                    return;
                }
                e.Reply(string.Format("\r\n {0}: {1} \r\n", nome, e.MessageString));
                int linhas = txtStatus.Lines.Length - 3;

                if (linhas >= 9)
                {
                    txtStatus.ScrollBars = ScrollBars.Vertical;
                    txtStatus.WordWrap = true;
                }
            });
        }

        private void onClickPerigo(string msg, SimpleTCP.Message e)
        {
            MessageBox.Show(msg);
            e.ReplyLine(string.Format("\r\n Eu digitei sexo: {0} \r\n", msg));
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            try
            {
                btnIniciar.Enabled = false;
                btnParar.Enabled = true;
                txtHost.ReadOnly = true;
                txtPort.ReadOnly = true;
                txtStatus.Text += "Iniciando servidor...\r\n";

                System.Net.IPAddress ip = System.Net.IPAddress.Parse(txtHost.Text);
                server.Start(ip, Convert.ToInt32(txtPort.Text));
                txtStatus.Text += "Servidor iniciado\r\n";
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
    }
}