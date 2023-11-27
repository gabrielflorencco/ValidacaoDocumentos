using System;
using System.Drawing;
using System.Windows.Forms;

namespace prj37288_Validacoes
{
    public partial class frm37288_Validacoes : Form
    {
        public frm37288_Validacoes()
        {
            InitializeComponent();
        }

        private void frm37288_Validacoes_Load(object sender, EventArgs e)
        {
            cmbDocumento.SelectedIndex = 0;
        }

        #region Botão Validar
        private void btnValidar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cmbDocumento.SelectedItem.ToString()))
            {
                MessageBox.Show($"Selecione um tipo de documento antes de tentar a validação!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tipoDocumento = cmbDocumento.SelectedItem.ToString();
            string numeroDocumento =
                txtNumero.Text.Replace(",", "").Replace(".", "").Replace("-", "").Replace(" ", "").Replace("/", "").Replace("_", "").Replace(" ", "");

            if (String.IsNullOrEmpty(numeroDocumento))
            {
                MessageBox.Show($"Digite um número de {cmbDocumento.SelectedItem.ToString()} na entrada!", "ERRO", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            bool validez;
            cls37288_Validacoes validacoes = new cls37288_Validacoes();

            if (tipoDocumento == "Cartão de Crédito (CC)")
            {
                validez = validacoes.ValidarCC(numeroDocumento);
            }
            else if (tipoDocumento == "Cadastro Pessoa Física (CPF)")
            {
                validez = validacoes.ValidarCPF(numeroDocumento);
            }
            else if (tipoDocumento == "Registro Geral (RG)")
            {
                validez = validacoes.ValidarRG(numeroDocumento);
            }
            else if (tipoDocumento == "Carteira Nacional de Habilitação (CNH)")
            {
                validez = validacoes.ValidarCNH(numeroDocumento);
            }
            else if (tipoDocumento == "PIS-PASEP")
            {
                validez = validacoes.ValidarPISPASEP(numeroDocumento);
            }
            else if (tipoDocumento == "Cadastro Nacional Pessoa Jurídica (CNPJ)")
            {
                validez = validacoes.ValidarCNPJ(numeroDocumento);
            }
            else if (tipoDocumento == "Título de Eleitor (TE)")
            {
                if (numeroDocumento.Length == 11)
                    txtNumero.Text = txtNumero.Text.Replace(" ", "").PadLeft(12, '0');

                validez = validacoes.ValidarTE(txtNumero.Text);
            }
            else
            {
                validez = false;
            }

            lblSituacao.Text = validez ? "Válido" : "Inválido";
            lblSituacao.ForeColor = validez ? Color.Green : Color.Red;
        }
        #endregion

        #region Mudanças na TextBox ao mudar o documento selecionado
        private void cmbDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNumero.Clear();
            lblSituacao.Text = "";
            txtNumero.Focus();

            int maxLength;
            string tipoDocumento = cmbDocumento.SelectedItem.ToString();

            string mask = "";

            if (tipoDocumento == "Cartão de Crédito (CC)")
            {
                maxLength = 16;
                mask = "0000 0000 0000 0000";
            }
            else if (tipoDocumento == "Cadastro Pessoa Física (CPF)")
            {
                maxLength = 11;
                mask = "000,000,000-00";
            }
            else if (tipoDocumento == "Registro Geral (RG)")
            {
                maxLength = 9;
                mask = "00,000,000-0";
            }
            else if (tipoDocumento == "Carteira Nacional de Habilitação (CNH)")
            {
                maxLength = 11;
                mask = "00000000000";
            }
            else if (tipoDocumento == "PIS-PASEP")
            {
                maxLength = 11;
                mask = "000,00000,00-0";
            }
            else if (tipoDocumento == "Cadastro Nacional Pessoa Jurídica (CNPJ)")
            {
                maxLength = 14;
                mask = "00,000,000/0000-00";
            }
            else if (tipoDocumento == "Título de Eleitor (TE)")
            {
                maxLength = 12;
                mask = "0000 0000 0000";
            }
            else
            {
                maxLength = 0;
            }

            txtNumero.Mask = mask;
            txtNumero.MaxLength = maxLength;
        }
        #endregion

        #region Form Closing
        private void frm37288_Validacoes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Deseja mesmo sair?", "Sair", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                e.Cancel = true;
        }
        #endregion
    }
}
