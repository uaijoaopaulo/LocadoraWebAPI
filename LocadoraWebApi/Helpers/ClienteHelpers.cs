using LocadoraWebApi.Models;
using LocadoraWebApi.Repository;

namespace LocadoraWebApi.Helper
{
    public class ClienteHelper : ClienteRepository
    {
        // Verifica se os dados estão sendo informados
        public string verificaCamposCliente(tb_ClienteCF value)
        {
            if (string.IsNullOrEmpty(value.CPF))
                return "O campo CPF não pode estar vazio";
            if (string.IsNullOrEmpty(value.nomeCliente))
                return "O campo NOME não pode estar vazio";
            return null;
        }

        // Verifica a existencia do cliente
        public tb_ClienteCF VerificaCliente(string cpfCliente, string nomeCliente, int idCliente)
        {
            var cliente = new tb_ClienteCF();
            if (!string.IsNullOrEmpty(cpfCliente))
            {
                cpfCliente = CPFshorter(cpfCliente);
                cliente = GetCliente(cpfCliente);
            }
            if (cliente == null || !cliente.clienteAtivo)
            {
                cliente = GetCliente(nomeCliente);
                if (cliente == null || !cliente.clienteAtivo)
                {
                    cliente = GetCliente(idCliente);
                    if (cliente == null || !cliente.clienteAtivo)
                        return null;
                    else
                        return cliente;
                }
                else
                    return cliente;
            }
            else
                return cliente;
        }

        // Verifica se o CPF é valido
        public bool IsCpf(string cpf)
        {
            for (int i = 0; i <= 9; i++)
            {
                string comp = "";
                for (int j = 0; j < 11; j++)
                {
                    comp = comp + i;
                }
                if (cpf.Equals(comp))
                {
                    return false;
                }
            }
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = CPFshorter(cpf);
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        // Retorna somente os numeros do CPF
        public string CPFshorter(string value)
        {
            return value.Replace(".", "").Replace("-", "");
        }
    }
}