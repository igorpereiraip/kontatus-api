using Kontatus.Data.Repository;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using Kontatus.Service;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;   
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface IArquivoService
    {
        Task SalvarArquivo(ArquivoDTO arquivoDTO);
        Task<(byte[], string)> GetArquivo(int usuarioID, string beneficio);
        Task<bool> ImportarXLS(Pessoa pessoa, Telefone telefone);
    }

    public class ArquivoService : IArquivoService
    {
        private readonly IArquivoRepository repository;
        private readonly IBlobStorageService blobStorageService;
        private readonly IConfiguration configuration;
        private readonly IPessoaRepository pessoaRepository;
        private readonly ITelefoneRepository telefoneRepository;

        public ArquivoService(IArquivoRepository repository, 
            IBlobStorageService blobStorageService, 
            IConfiguration configuration,
            IPessoaRepository pessoaRepository,
            ITelefoneRepository telefoneRepository
            )
            : base()
        {
            this.repository = repository;
            this.blobStorageService = blobStorageService;
            this.configuration = configuration;
            this.pessoaRepository = pessoaRepository;
            this.telefoneRepository = telefoneRepository;

        }

        public async Task<bool> ImportarXLS(Pessoa pessoa, Telefone telefone)
        {
            var listPessoa = await pessoaRepository.Find(x => x.CPF == pessoa.CPF).ToListAsync();

            if (listPessoa.Count > 0)
            {
                var telefoneExistente = await telefoneRepository.Find(x => x.NumeroTelefone == telefone.NumeroTelefone && x.PessoaId == listPessoa[0].ID).FirstOrDefaultAsync();
                if (telefoneExistente == null)
                {
                    telefone.PessoaId = listPessoa[0].ID;
                    var telefoneNovo = await telefoneRepository.Create(telefone);
                }

            }
            else
            {
                var pessoaNova = await pessoaRepository.Create(pessoa);
                telefone.PessoaId = pessoaNova.ID;
                var telefoneNovo = await telefoneRepository.Create(telefone);
            }

            return true;

        }

        public async Task SalvarArquivo(ArquivoDTO arquivoDTO)
        {
            foreach (var arquivo in arquivoDTO.Arquivos)
            {
                var nomeDiretorio = DiretorioAzure();
                var nomeOriginal = arquivo.FileName;
                var nome = NormalizarNome(nomeOriginal);

                var destino = string.Format("{0}/{1}", nomeDiretorio, nome);

                var fileStream = arquivo.OpenReadStream();
                CloudBlockBlob blob = await blobStorageService.UploadBlockBlob("rfarquivos", destino, fileStream);
                SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy();


                var tamanhoMb = Math.Round(blob.Properties.Length / 1048576.0, 2);

                //if (!string.IsNullOrEmpty(blob.SnapshotQualifiedStorageUri.PrimaryUri.AbsoluteUri))
                //{
                //    await repository.SalvarArquivo(
                //        arquivoDTO.
                //        Beneficio,
                //        arquivoDTO.Mes,
                //        DateTime.Now.Year.ToString(),
                //        DateTime.UtcNow.Day.ToString("d2"),
                //        arquivoDTO.UsuarioID,
                //        blob.SnapshotQualifiedStorageUri.PrimaryUri.AbsoluteUri,
                //        nome,
                //        tamanhoMb);
                //}
            }
        }


        public async Task<(byte[], string)> GetArquivo(int usuarioID, string beneficio)
        {
            var arquivo = await repository.GetArquivo(usuarioID, beneficio);

            if (arquivo == null)
                throw new Exception("Arquivo expirado ou inválido");
                

            byte[] dt;
            using (var archiveStream = new MemoryStream())
            {

              var (dados, nome) = await blobStorageService.DownloadBlockBlob("rfarquivos", arquivo.NomeCompleto, arquivo.NomeCompleto);
              dt = dados;
            }
            return (dt, $"{beneficio}.pdf");

        }

        public string DiretorioAzure()
        {
            var mes = DateTime.UtcNow.Month.ToString().PadLeft(2, '0');
            var ano = DateTime.UtcNow.Year.ToString();
            var dia = DateTime.UtcNow.Day.ToString("d2");
            return string.Format("{0}/{1}/{2}", ano, mes, dia);
        }

        private string NormalizarNome(string nome)
        {
            return $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}{new Random().Next(1000, 9999)}{Path.GetExtension(nome)}";
        }

    }
}
