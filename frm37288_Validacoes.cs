using System;
using System.Collections.Generic;
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
            if (cmbDocumento.SelectedItem == null || String.IsNullOrEmpty(cmbDocumento.SelectedItem.ToString()))
            {
                MessageBox.Show("Selecione um tipo de documento antes de tentar a validação!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tipoDocumento = cmbDocumento.SelectedItem.ToString();
            string numeroDocumento = txtNumero.Text
                .Replace(",", "").Replace(".", "").Replace("-", "")
                .Replace(" ", "").Replace("/", "").Replace("_", "").Replace(" ", "");

            if (String.IsNullOrEmpty(numeroDocumento))
            {
                MessageBox.Show($"Digite um número de {tipoDocumento} na entrada!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Validacoes validacoes = new Validacoes();

            var validadores = new Dictionary<string, Func<string, bool>>()
            {
                { "Cartão de Crédito (CC)", validacoes.ValidarCC },
                { "Cadastro Pessoa Física (CPF)", validacoes.ValidarCPF },
                { "Registro Geral (RG)", validacoes.ValidarRG },
                { "Carteira Nacional de Habilitação (CNH)", validacoes.ValidarCNH },
                { "PIS-PASEP", validacoes.ValidarPISPASEP },
                { "Cadastro Nacional Pessoa Jurídica (CNPJ)", validacoes.ValidarCNPJ },
                { "Título de Eleitor (TE)", num =>
                    {
                        if (num.Length == 11)
                            num = num.PadLeft(12, '0');
                        return validacoes.ValidarTE(num);
                    }
                }
            };

            bool validez = validadores.TryGetValue(tipoDocumento, out Func<string, bool> validar) ? validar(numeroDocumento) : false;

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

            if (cmbDocumento.SelectedItem == null)
                return;

            string tipoDocumento = cmbDocumento.SelectedItem.ToString();

            var formatos = new Dictionary<string, (int maxLength, string mask)>
            {
                { "Cartão de Crédito (CC)", (16, "0000 0000 0000 0000") },
                { "Cadastro Pessoa Física (CPF)", (11, "000,000,000-00") },
                { "Registro Geral (RG)", (9, "00,000,000-0") },
                { "Carteira Nacional de Habilitação (CNH)", (11, "00000000000") },
                { "PIS-PASEP", (11, "000,00000,00-0") },
                { "Cadastro Nacional Pessoa Jurídica (CNPJ)", (14, "00,000,000/0000-00") },
                { "Título de Eleitor (TE)", (12, "0000 0000 0000") }
            };

            if (formatos.TryGetValue(tipoDocumento, out var config))
            {
                txtNumero.MaxLength = config.maxLength;
                txtNumero.Mask = config.mask;
            }
            else
            {
                txtNumero.MaxLength = 0;
                txtNumero.Mask = "";
            }
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
