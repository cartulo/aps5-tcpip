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
        string nome = "";
        string[] mensagens = new string[] 
        {
            "Olá, estamos acionando a equipe de ajuda, fique calmo e mantenha contato.",
            "Tudo bem",
            "com você?"
        };

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                btnConectar.Enabled = false;
                client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
                btnDesconectar.Enabled = true;        
                txtMensagem.Enabled = true;
                txtHost.ReadOnly = true;
                txtPort.ReadOnly = true;
                btnAlerta.Enabled = true;
                btnPerigo.Enabled = true;
                txtNome.Enabled = true;

                txtStatus.Text += "Conectado ao servidor.\r\n";
            } catch
            {
                btnConectar.Enabled = true;
                btnDesconectar.Enabled = false;
                MessageBox.Show("Chat inexistente");
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            client.Disconnect();
            txtStatus.Text += "Desconectado do servidor com sucesso.\r\n";
            btnConectar.Enabled = true;
            btnDesconectar.Enabled = false;
            btnEnviar.Enabled = false;
            txtMensagem.Enabled = false;
            txtHost.Enabled = false;
            txtPort.Enabled = false;
            txtNome.Enabled = false;
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

                for (int i = 0; i < mensagens.Length; i++)
                {
                    if (txtStatus.Text.IndexOf(mensagens[i].ToString()) != -1)
                    {
                        client.WriteLineAndGetReply($"Cliente: {mensagens[i]}", TimeSpan.FromMilliseconds(350));
                        return;
                    }
                }
            });
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                if (nome == "")
                {
                    txtNome.Enabled = false;
                    nome = txtNome.Text;
                    client.WriteLine(nome);
                }

                client.WriteLineAndGetReply(txtMensagem.Text, TimeSpan.FromMilliseconds(350));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void btnPerigo_Click(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply("Cuidade!", TimeSpan.FromMilliseconds(350));
        }

        private void btnAlerta_Click(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply("Alerte", TimeSpan.FromMilliseconds(350));
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            
            if(txtNome.Text.Length > 3) btnEnviar.Enabled = true; else btnEnviar.Enabled = false;
            
        }
    }
}