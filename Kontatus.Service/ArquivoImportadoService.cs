using ExcelDataReader;
using Kontatus.Data.Repository;
using Kontatus.Domain.Entity;
using Kontatus.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface IArquivoImportadoService
    {
        Task<ArquivoImportado> CreateArquivoImportado(string competencia, string descricao);
        Task<ArquivoImportado> UpdateArquivoImportado(ArquivoImportado arquivoImportado);
        Task<ArquivoImportado> ImportarXLSLote(List<string[]> reader, ArquivoImportado arquivoImportado);
    }

    public class ArquivoImportadoService : IArquivoImportadoService
    {
        private readonly IArquivoImportadoRepository repository;
        private readonly IConfiguration configuration;
        private readonly IPessoaService pessoaService;
        private readonly ITelefoneService telefoneService;
        private readonly IEnderecoService enderecoService;
        private readonly IEmailService emailService;
        private ArquivoImportado arquivoImportado;
        public ArquivoImportadoService(IArquivoImportadoRepository repository, 
            IConfiguration configuration,
            IPessoaService pessoaService,
            ITelefoneService telefoneService,
            IEnderecoService enderecoService,
            IEmailService emailService
            )
            : base()
        {
            this.repository = repository;
            this.configuration = configuration;
            this.pessoaService = pessoaService;
            this.telefoneService = telefoneService;
            this.enderecoService = enderecoService;
            this.emailService = emailService;
            this.arquivoImportado = null;
        }

        public async Task<ArquivoImportado> UpdateArquivoImportado(ArquivoImportado arquivoImportado)
        {
            return await repository.Update(arquivoImportado);
        }

        public async Task<ArquivoImportado> CreateArquivoImportado(string competencia, string descricao)
        {
            var arquivoImportado = new ArquivoImportado()
            {
                Competencia = competencia,
                Descricao = descricao,
                StatusProcessamento = StatusProcessamentoEnum.EmProcessamento,
                PessoasAdicionadas = 0,
                EnderecosCriados = 0,
                TelefonesCriados = 0,
                EmailsCriados = 0,
            };
            var newArquivoImportado = await repository.Create(arquivoImportado);
            return newArquivoImportado;
        }

        public async Task<ArquivoImportado> ImportarXLSLote(List<string[]> reader, ArquivoImportado arquivoImportado)
        {
            this.arquivoImportado = arquivoImportado;

            await ProcessPessoa(reader);

            await ProcessTelefone(reader);

            await ProcessEndereco(reader);

            await ProcessEmail(reader);

            return this.arquivoImportado;
        }


        private async Task ProcessPessoa(List<string[]> reader)
        {
            var count = 0;
            var countPessoa = 0;
            var listPessoa = new List<Pessoa>();
            foreach (var line in reader)
            {
                try
                {
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }

                    //PESSOA
                    var cpf = line[1].ToString().PadLeft(11, '0');
                    var nome = line[2].ToString();
                    var sexo = line[3].ToString();
                    var dataNascimento = line[4].ToString();
                    var aposentado = line[30].ToString() == "APOSENTADO" ? true : false;

                    var pessoaDB = await pessoaService.GetByCpf(cpf);
                    if (pessoaDB != null)
                    {
                        continue;
                    }


                    var pessoa = new Pessoa()
                    {
                        Nome = nome,
                        CPF = cpf,
                        DataNascimento = dataNascimento,
                        Sexo = sexo,
                        Aposentado = aposentado
                    };

                    if (await pessoaService.ValidatePessoaAsync(pessoa, listPessoa))
                        listPessoa.Add(pessoa);

                    if (countPessoa == 1000 && listPessoa.Count > 0)
                    {
                        await pessoaService.CreateRange(listPessoa);
                        this.arquivoImportado.PessoasAdicionadas += listPessoa.Count;
                        listPessoa.Clear();
                        countPessoa = 0;
                    }

                    countPessoa++;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (listPessoa.Count > 0)
            {
                await pessoaService.CreateRange(listPessoa);
                this.arquivoImportado.PessoasAdicionadas += listPessoa.Count;
            }

            await UpdateArquivoImportado();
        }
   

        private async Task ProcessTelefone(List<string[]> reader)
        {
            var count = 0;
            var countTelefone = 0;
            var listTelefone = new List<Telefone>();
            var listNumeros = new List<string>();
            foreach (var line in reader)
            {
                try
                {
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }

               
                    var cpf = line[1].ToString().PadLeft(11, '0');
                    var pessoa = await pessoaService.GetByCpf(cpf);
                    if (pessoa == null)
                    {
                        continue;
                    }

                    
                    //TELEFONE
                    var whatsappPrimeiroTelefone = line[54].ToString() == "SIM" ? true : false;
                    var primeiroTelefone = line[55].ToString();
                    listNumeros.Add(line[56].ToString());
                    listNumeros.Add(line[57].ToString());
                    listNumeros.Add(line[58].ToString());
                    listNumeros.Add(line[59].ToString());
                    listNumeros.Add(line[60].ToString());
                    listNumeros.Add(line[61].ToString());
                    listNumeros.Add(line[62].ToString());
                    listNumeros.Add(line[63].ToString());
                    listNumeros.Add(line[64].ToString());


                    var telefone = new Telefone()
                    {
                        NumeroTelefone = primeiroTelefone,
                        ArquivoImportadoId = arquivoImportado.ID,
                        Whatsapp = whatsappPrimeiroTelefone,
                        PessoaId = pessoa.ID
                    };

                    if (await telefoneService.ValidateTelefoneFullAsync(telefone, listTelefone))
                        listTelefone.Add(telefone);

                    foreach (var tel in listNumeros)
                    {
                        var telefoneInsert = new Telefone()
                        {
                            NumeroTelefone = tel,
                            ArquivoImportadoId = arquivoImportado.ID,
                            PessoaId = pessoa.ID
                        };

                        if (await telefoneService.ValidateTelefoneFullAsync(telefoneInsert, listTelefone))
                            listTelefone.Add(telefoneInsert);
                    }


                    if (countTelefone == 1000 && listTelefone.Count > 0)
                    {
                        await telefoneService.CreateRange(listTelefone);
                        this.arquivoImportado.TelefonesCriados += listTelefone.Count;
                        listTelefone.Clear();
                        countTelefone = 0;
                    }

                    listNumeros.Clear();

                    countTelefone++;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (listTelefone.Count > 0)
            {
                await telefoneService.CreateRange(listTelefone);
                this.arquivoImportado.TelefonesCriados += listTelefone.Count;
            }

            await UpdateArquivoImportado();
        }

        private async Task ProcessEndereco(List<string[]> reader)
        {
            var count = 0;
            var countEndereco = 0;
            var listEndereco = new List<Endereco>();
            foreach (var line in reader)
            {
                try
                {
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }


                    var cpf = line[1].ToString().PadLeft(11, '0');
                    var pessoa = await pessoaService.GetByCpf(cpf);
                    if (pessoa == null)
                    {
                        continue;
                    }


                    //ENDERECO
                    var descricaoEndereco = line[45].ToString();
                    var bairro = line[46].ToString();
                    var cidade = line[47].ToString();
                    var cep = line[48].ToString();
                    var uf = line[49].ToString();


                    var endereco = new Endereco()
                    {
                        Cidade = cidade,
                        Bairro = bairro,
                        DescricaoEndereco = descricaoEndereco,
                        Uf = uf,
                        Cep = cep,
                        ArquivoImportadoId = this.arquivoImportado.ID,
                        PessoaId = pessoa.ID
                    };

                    if (await enderecoService.ValidateEnderecoAsync(endereco, listEndereco))
                        listEndereco.Add(endereco);


                    if (countEndereco == 1000 && listEndereco.Count > 0)
                    {
                        await enderecoService.CreateRange(listEndereco);
                        this.arquivoImportado.EnderecosCriados += listEndereco.Count;
                        listEndereco.Clear();
                        countEndereco = 0;
                    }

                    countEndereco++;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (listEndereco.Count > 0)
            {
                await enderecoService.CreateRange(listEndereco);
                this.arquivoImportado.EnderecosCriados += listEndereco.Count;
            }

            await UpdateArquivoImportado();
        }

        private async Task ProcessEmail(List<string[]> reader)
        {
            var count = 0;
            var countEmail = 0;
            var listEmail = new List<Email>();
            foreach (var line in reader)
            {
                try
                {
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }


                    var cpf = line[1].ToString().PadLeft(11, '0');
                    var pessoa = await pessoaService.GetByCpf(cpf);
                    if (pessoa == null)
                    {
                        continue;
                    }


                    //EMAIL
                    var enderecoEmail = line[50].ToString();

                    var email = new Email()
                    {
                        EnderecoEmail = enderecoEmail,
                        ArquivoImportadoId = arquivoImportado.ID,
                        PessoaId = pessoa.ID
                    };


                    if (await emailService.ValidateEmailAsync(email, listEmail))
                        listEmail.Add(email);


                    if (countEmail == 1000 && listEmail.Count > 0)
                    {
                        await emailService.CreateRange(listEmail);
                        this.arquivoImportado.EmailsCriados += listEmail.Count;
                        listEmail.Clear();
                        countEmail = 0;
                    }

                    countEmail++;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (listEmail.Count > 0)
            {
                await emailService.CreateRange(listEmail);
                this.arquivoImportado.EmailsCriados += listEmail.Count;
            }

            await UpdateArquivoImportado();
        }

        private async Task UpdateArquivoImportado()
        {
            var arquivoImportadoDB = await repository.Get(this.arquivoImportado.ID);
            arquivoImportadoDB.PessoasAdicionadas = this.arquivoImportado.PessoasAdicionadas;
            arquivoImportadoDB.TelefonesCriados = this.arquivoImportado.TelefonesCriados;
            arquivoImportadoDB.EnderecosCriados = this.arquivoImportado.EnderecosCriados;
            arquivoImportadoDB.EmailsCriados = this.arquivoImportado.EmailsCriados;
            arquivoImportadoDB.StatusProcessamento = this.arquivoImportado.StatusProcessamento;

            await UpdateArquivoImportado(arquivoImportadoDB);
        }


    }
}
