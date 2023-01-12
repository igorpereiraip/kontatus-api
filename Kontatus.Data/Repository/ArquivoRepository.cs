using ConsigIntegra.Data.Context;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsigIntegra.Data.Repository
{
    public interface IArquivoRepository
    {
        Task<int> SalvarArquivo(string beneficio, string mes, string ano, string dia, int usuarioID, string caminhoStorageArquivo, string nomeBlob, double tamanho);
        Task<IEnumerable<Arquivo>> ListarArquivos();
        Task<Arquivo> GetArquivo(int usuarioID, string beneficio);
    }
        public class ArquivoRepository : Repository<Arquivo>, IArquivoRepository
        {
            public ArquivoRepository(ConsigIntegraContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
            {

            }

            public async Task<int> SalvarArquivo(string beneficio, string mes, string ano, string dia, int usuarioID, string caminhoStorageArquivo, string nomeBlob, double tamanho)
            {
                int arquivoID;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    { 
                        var periodo = string.Format("{0}/{1}", mes, ano);
                       
                        var arquivo = new Arquivo()
                        {
                            Ativo = true,
                            Tamanho = tamanho,
                            CaminhoImagem = caminhoStorageArquivo,
                            DataCadastro = DateTime.UtcNow,
                            Beneficio = beneficio,
                            NomeBlob = nomeBlob,
                            DataUpload = DateTime.UtcNow,
                            UsuarioID = usuarioID,
                            NomeCompleto = string.Format("{0}/{1}/{2}/{3}", ano, mes, dia, nomeBlob)
                };

                        context.Arquivos.Add(arquivo);

                        await context.SaveChangesAsync();

                        trans.Commit();
                        arquivoID = arquivo.ID;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }

                return arquivoID;
            }


            public async Task<IEnumerable<Arquivo>> ListarArquivos()
            {
                return await context.Arquivos.Where(x => x.Ativo).ToListAsync();
            }

            public async Task<Arquivo> GetArquivo(int usuarioID, string beneficio)
            {
                return await context.Arquivos.Where(x => x.Ativo && x.Beneficio == beneficio && x.UsuarioID == usuarioID).OrderByDescending(x => x.DataUpload).FirstOrDefaultAsync();
            }


    }
}
