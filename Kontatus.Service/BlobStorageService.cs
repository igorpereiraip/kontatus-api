using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface IBlobStorageService
    {
        Task CheckContainer(string nomeContainer);
        Task<CloudBlockBlob> UploadBlockBlob(string nomeContainer, string nomeBlob, Stream stream);
        Task<(byte[], string)> DownloadBlockBlob(string nomeContainer, string nomeBlob, string diretorio);
        Task DeleteBlockBlob(string nomeContainer, string nomeBlob, string diretorio);
        Task<List<Uri>> ListBlockBlobs(string nomeContainer);
        Task<string> GetNewSignature(string nomeContainer, string nomeBlob, bool infinity = false);
        Task<double> GetSizeMb(string nomeContainer, string nomeBlob);
    }
    public class BlobStorageService : IBlobStorageService
    {
        public BlobStorageService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private IConfiguration Configuration { get; set; }

        private async Task<CloudBlobContainer> ObterReferenciaContainer(string nomeContainer)
        {
            string StrConnecting = RetornaUrlConection();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StrConnecting);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(nomeContainer);
        }
        private async Task<CloudBlockBlob> ObterReferenciaBlockBlob(string nomeContainer, string nomeBlob)
        {
            CloudBlobContainer container = await ObterReferenciaContainer(nomeContainer);

            CloudBlockBlob blob = container.GetBlockBlobReference(nomeBlob);

            return blob;
        }
        public async Task CheckContainer(string nomeContainer)
        {
            CloudBlobContainer container = await ObterReferenciaContainer(nomeContainer);
            await container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Container, null, null);
        }
        public async Task<CloudBlockBlob> UploadBlockBlob(string nomeContainer, string nomeBlob, Stream stream)
        {
            CloudBlockBlob blob = await ObterReferenciaBlockBlob(nomeContainer, nomeBlob);
            AccessCondition accessCondition = null;

            blob.UploadFromStreamAsync(stream, accessCondition, null, null).Wait();
            var url = blob.Uri.AbsoluteUri.ToString();
            return blob;

        }
        public async Task<(byte[], string)> DownloadBlockBlob(string nomeContainer, string nomeBlob, string diretorio)
        {
            var caminho = string.Format("{0}", diretorio);
            CloudBlockBlob blob = await ObterReferenciaBlockBlob(nomeContainer, caminho);
            blob.FetchAttributesAsync().Wait();
            long fileByteLength = blob.Properties.Length;
            var nome = blob.Name;
            byte[] fileContent = new byte[fileByteLength];

            await blob.DownloadToByteArrayAsync(fileContent, 0);

            return (fileContent, nomeBlob);

        }
        public async Task DeleteBlockBlob(string nomeContainer, string nomeBlob, string diretorio)
        {
            var caminho = string.Format("{0}", diretorio);
            CloudBlockBlob blob = await ObterReferenciaBlockBlob(nomeContainer, caminho);
            bool existe = await blob.ExistsAsync();
            if (existe)
                await blob.DeleteAsync();
        }
        public async Task<List<Uri>> ListBlockBlobs(string nomeContainer)
        {
            var blobs = new List<Uri>();
            BlobContinuationToken blobContinuationToken = null;

            CloudBlobContainer container = await ObterReferenciaContainer(nomeContainer);
            var resultSegment = await container.ListBlobsSegmentedAsync(null, blobContinuationToken);
            blobContinuationToken = resultSegment.ContinuationToken;
            foreach (var blob in resultSegment.Results)
            {
                blobs.Add(blob.Uri);
            }

            return blobs;
        }

        public async Task<string> GetNewSignature(string nomeContainer, string nomeBlob, bool infinity = false)
        {
            CloudBlockBlob blob = await ObterReferenciaBlockBlob(nomeContainer, nomeBlob);

            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy();

            var systemConfigurations = Configuration.GetSection("SystemConfigurations");
            var minutosValidade = systemConfigurations["MinutosValidadeCaminhoImagem"];

            var validadeCaminhoImagem = DateTime.UtcNow;

            if (minutosValidade == null)
            {
                minutosValidade = "1440";
            }

            validadeCaminhoImagem = validadeCaminhoImagem.AddMinutes(int.Parse(minutosValidade));
            policy.SharedAccessExpiryTime = infinity ? DateTime.UtcNow.AddYears(100) : validadeCaminhoImagem;
            policy.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15);
            policy.Permissions = SharedAccessBlobPermissions.Read;
            var assinatura = blob.GetSharedAccessSignature(policy);

            return assinatura;
        }

        private string RetornaUrlConection()
        {
            var conexao = Configuration.GetSection("GeralConfiguration").GetSection("BlobStorageConnection").Value;
            return conexao;
        }

        public async Task<double> GetSizeMb(string nomeContainer, string nomeBlob)
        {
            try
            {
                CloudBlockBlob blob = await ObterReferenciaBlockBlob(nomeContainer, nomeBlob);
                var sizeInMb = Math.Round(blob.Properties.Length / 1048576.0, 2);
                await blob.FetchAttributesAsync();

                return sizeInMb == 0 ? 0.01 : sizeInMb;
            }
            catch (Exception)
            {
                return 0.01;
            }
        }
    }
}
