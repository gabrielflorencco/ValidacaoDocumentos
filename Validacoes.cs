using System;

internal class Validacoes
{
    public bool ValidarCC(string cartao)
    {
        int peso = 0;
        int soma = 0;

        #region Validação de Texto
        if (String.IsNullOrEmpty(cartao))
            return false;

        if (!long.TryParse(cartao, out long num))
            return false;

        if (cartao.Length < 16)
            return false;

        if (cartao == new string(cartao[0], 16))
            return false;
        #endregion

        #region Validação do Cartão

        for (int i = 0; i < 16; i++)
        {
            if (i % 2 == 0)
            {
                peso = 2;
            }
            else
            {
                peso = 1;
            }

            if (int.Parse(cartao[i].ToString()) * peso > 9)
            {
                soma += (int.Parse(cartao[i].ToString()) * peso) - 9;
            }
            else
            {
                soma += int.Parse(cartao[i].ToString()) * peso;
            }
        }

        if (soma % 10 == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        #endregion
    }

    public bool ValidarCNH(string CNH)
    {
        #region Validação de Texto
        if (String.IsNullOrEmpty(CNH))
            return false;

        if (!long.TryParse(CNH, out long num))
            return false;

        if (CNH.Length < 11)
            return false;

        if (CNH == new string(CNH[0], 11))
            return false;
        #endregion

        #region Validação da CNH
        bool isValid = false;
        int digito1 = 0;
        int digito2 = 0;
        int dsc = 0;

        for (int c = 0; c < 2; c++)
        {
            int soma = 0;
            int peso = c == 0 ? 9 : 1;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(CNH[i].ToString()) * peso;
                peso = c == 0 ? peso - 1 : peso + 1;
            }

            int resto = soma % 11;

            if (c == 0)
            {
                if (resto >= 10)
                {
                    digito1 = 0;
                    dsc = 2;
                }
                else
                {
                    digito1 = resto;
                }
            }
            else
            {
                digito2 = resto >= 10 ? 0 : resto - dsc;
            }
        }

        isValid = digito1.ToString() + digito2.ToString() == CNH.Substring(CNH.Length - 2, 2);

        return isValid;
        #endregion
    }

    public bool ValidarCNPJ(string CNPJ)
    {
        CNPJ = CNPJ.Trim().Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "");

        #region Validação de Texto
        if (string.IsNullOrEmpty(CNPJ))
            return false;

        if (!long.TryParse(CNPJ, out long num))
            return false;

        if (CNPJ.Length != 14)
            return false;

        if (CNPJ == new string(CNPJ[0], 16))
            return false;
        #endregion

        #region Validação do CNPJ
        int peso = 5, soma = 0, digito1 = 0, digito2 = 0;
        soma = 0;

        for (int j = 0; j < 12; j++)
        {
            if (j != 0 && j != 4)
                peso--;

            soma += int.Parse(CNPJ[j].ToString()) * peso;

            if (peso == 2)
                peso = 9;
        }

        if (soma % 11 < 2)
        {
            digito1 = 0;
        }
        else
        {
            digito1 = 11 - (soma % 11);
        }

        soma = 0;
        peso = 6;

        for (int j = 0; j < 13; j++)
        {
            if (j != 0 && j != 5)
                peso--;

            soma += int.Parse(CNPJ[j].ToString()) * peso;

            if (peso == 2)
                peso = 9;
        }

        if (soma % 11 < 2)
        {
            digito2 = 0;
        }
        else
        {
            digito2 = 11 - (soma % 11);
        }

        if (digito1.ToString() == CNPJ.Substring(12, 1) && digito2.ToString() == CNPJ.Substring(13, 1))
        {
            return true;
        }
        else
        {
            return false;
        }
        #endregion
    }

    public bool ValidarCPF(string CPF)
    {
        int c = 0;
        int digito1 = 0;
        int digito2 = 0;
        bool cpfCompleto = false;

        #region Validação de Texto
        if (String.IsNullOrEmpty(CPF))
            return false;

        if (!long.TryParse(CPF, out long num))
            return false;

        if (CPF.Length < 11)
            return false;

        if (CPF == new string(CPF[0], 11))
            return false;
        #endregion

        #region Validação do CPF
        if (CPF.Length != 11)
        {
            cpfCompleto = false;
        }
        else
        {
            cpfCompleto = true;
        }

        for (int i = 10; i > 1; i--)
        {
            digito1 += int.Parse(CPF.Substring(c, 1)) * i;
            c++;
        }

        digito1 = digito1 % 11;

        if (digito1 > 1)
        {
            digito1 = 11 - digito1;
        }
        else
        {
            digito1 = 0;
        }

        if (cpfCompleto == false)
        {
            c = 0;
            CPF = CPF + digito1.ToString();

            for (int i = 11; i > 1; i--)
            {
                digito2 += int.Parse(CPF.Substring(c, 1)) * i;
                c++;
            }

            digito2 = digito2 % 11;

            if (digito2 > 1)
            {
                digito2 = 11 - digito2;
            }
            else
            {
                digito2 = 0;
            }

            return false;
        }

        if (digito1 != int.Parse(CPF.Substring(9, 1)))
        {
            return false;
        }

        c = 0;

        for (int i = 11; i > 1; i--)
        {
            digito2 += int.Parse(CPF.Substring(c, 1)) * i;
            c++;
        }

        digito2 = digito2 % 11;

        if (digito2 > 1)
        {
            digito2 = 11 - digito2;
        }
        else
        {
            digito2 = 0;
        }

        if (digito2 != int.Parse(CPF.Substring(10, 1)))
        {
            return false;
        }

        return true;
        #endregion
    }

    public bool ValidarPISPASEP(string pispasep)
    {
        pispasep = pispasep.Trim().Replace(".", "").Replace(",", "").Replace("-", "");

        #region Validação de Texto
        if (String.IsNullOrEmpty(pispasep))
            return false;

        if (!long.TryParse(pispasep, out long num))
            return false;

        if (pispasep.Length < 11)
            return false;

        if (pispasep == new string(pispasep[0], 11))
            return false;
        #endregion

        #region Validação do PISPASEP
        int peso = 3;
        int soma = 0;

        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(pispasep[i].ToString()) * peso;
            peso = peso == 2 ? 9 : peso - 1;
        }

        int digito;

        if (11 - (soma % 11) == 10 || 11 - (soma % 11) == 11)
            digito = 0;
        else
            digito = 11 - (soma % 11);

        if (digito.ToString() == pispasep[10].ToString())
            return true;
        else
            return false;
        #endregion
    }

    public bool ValidarRG(string RG)
    {
        RG = RG.Trim().Replace(".", "").Replace(",", "").Replace("-", "");
        int peso = 9;
        int soma = 0;
        int digito;

        #region Validação de Texto
        if (String.IsNullOrEmpty(RG))
            return false;

        if (!long.TryParse(RG, out long num))
            return false;

        if (RG.Length < 9)
            return false;

        if (RG == new string(RG[0], 9))
            return false;
        #endregion

        #region Validação do RG
        for (int i = 0; i < 8; i++)
        {
            soma += RG[i] * peso;
            peso--;
        }

        digito = soma % 11;

        if (RG[8].ToString() == digito.ToString() || (RG[8] == 'x' || RG[8] == 'X' && digito == 10))
        {
            return true;
        }
        else
        {
            return false;
        }
        #endregion
    }

    public bool ValidarTE(string titulo)
    {
        titulo = titulo.Trim().Replace("-", "").Replace(" ", "");

        #region Validação de Texto
        if (String.IsNullOrEmpty(titulo))
            return false;

        if (titulo.Length < 12)
            return false;

        if (titulo == new string(titulo[0], 12) || titulo == new string(titulo[0], 13))
            return false;

        if (titulo.Length == 12)
            titulo = titulo.PadLeft(13, '0');
        #endregion

        #region Validação do TE
        int peso = 2;
        int soma = 0;
        int dv1;
        int dv2;

        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(titulo[i].ToString()) * peso;
            peso = peso == 2 ? 9 : peso - 1;
        }

        if (soma % 11 == 1)
        {
            dv1 = 0;
        }
        else
        {
            if ((soma % 11) == 0)
            {
                if (titulo[9].ToString() + titulo[10].ToString() == "01" || titulo[9].ToString() + titulo[10].ToString() == "02")
                {
                    dv1 = 1;
                }
                else
                {
                    dv1 = 0;
                }
            }
            else
            {
                dv1 = 11 - (soma % 11);
            }
        }

        soma = 0;
        peso = 4;

        for (int i = 0; i < 3; i++)
        {
            soma += int.Parse(titulo[9 + i].ToString()) * peso;
            peso--;
        }

        if ((soma % 11) == 1)
        {
            dv2 = 0;
        }
        else
        {
            if ((soma % 11) == 0)
            {
                if (titulo.Substring(8, 2) == "01" || titulo.Substring(8, 2) == "02")
                {
                    dv2 = 1;
                }
                else
                {
                    dv2 = 0;
                }
            }
            else
            {
                dv2 = 11 - (soma % 11);
            }
        }

        string digitos = dv1.ToString() + dv2.ToString();
        string digitosV = titulo.Substring(11, 2).ToString();

        if (digitosV == digitos)
        {
            return true;
        }
        else
        {
            return false;
        }
        #endregion
    }
}