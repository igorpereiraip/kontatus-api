using AutoMapper;
using Kontatus.Helper.Utilitarios;
using Kontatus.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Kontatus.API.Configurations;
using System.Security.Claims;
using ExcelDataReader;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using System.Data;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace Kontatus.API.Controllers
{
    [Route("api/[controller]")]
    [ClaimRequirement(ClaimTypes.Name, "Authorization")]
    [ApiController]
    public class ArquivoController: ControllerBase
    {
        private readonly IArquivoService service;
        private readonly IArquivoImportadoService arquivoImportadoService;
        private readonly IMapper mapper;

        public ArquivoController(IArquivoService service, IArquivoImportadoService arquivoImportadoService, IMapper mapper)
        {
            this.service = service;
            this.arquivoImportadoService = arquivoImportadoService;
            this.mapper = mapper;
        }


        [HttpGet("getArquivo/{usuarioID}/{beneficio}")]
        public async Task<FileContentResult> GetArquivo(int usuarioID, string beneficio)
        {
            try
            {
                var (conteudo, nome) = await service.GetArquivo(usuarioID, beneficio);
                var file = File(conteudo, MimeMapping.GetMapping(nome), nome);

                return file;
            }
            catch(Exception ex)
            {
                var z = ex.Message;
                return null;
            }
        }
        [HttpPost("ImportXLS")]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        public async Task<Result<bool>> ImportExcelAsync([FromForm] ArquivoDTO arquivoDTO)
        {
            try
            {
                //using (var stream = arquivoDTO.Arquivos[0].OpenReadStream())
                //{
                //    using (var reader = ExcelReaderFactory.CreateReader(stream))
                //    {
                //var arquivoImportado = await arquivoImportadoService.CreateArquivoImportado(arquivoDTO.Competencia, arquivoDTO.Descricao);
                //var arquivoImportadoManipular = new ArquivoImportado()
                //{
                //    ID = arquivoImportado.ID,
                //    Competencia = arquivoImportado.Competencia,
                //    Descricao = arquivoImportado.Descricao,
                //    StatusProcessamento = arquivoImportado.StatusProcessamento,
                //    EmailsCriados = 0,
                //    EnderecosCriados = 0,
                //    PessoasAdicionadas = 0,
                //    TelefonesCriados = 0
                //};

                //arquivoImportadoManipular = await arquivoImportadoService.ImportarXLSLote(reader, arquivoImportadoManipular);

                //        await arquivoImportadoService.UpdateArquivoImportado(arquivoImportadoManipular);
                //    }
                //}

                using (var stream = arquivoDTO.Arquivos[0].OpenReadStream())
                {
                    var data = new List<string[]>();
                    using (TextFieldParser parser = new TextFieldParser(stream))
                    {               
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");

                        while (!parser.EndOfData)
                        {
                            // Parse each line of the CSV file
                            string[] fields = parser.ReadFields();
                            data.Add(fields);
                        }

                        var arquivoImportado = await arquivoImportadoService.CreateArquivoImportado(arquivoDTO.Competencia, arquivoDTO.Descricao);
                        var arquivoImportadoManipular = new ArquivoImportado()
                        {
                            ID = arquivoImportado.ID,
                            Competencia = arquivoImportado.Competencia,
                            Descricao = arquivoImportado.Descricao,
                            StatusProcessamento = arquivoImportado.StatusProcessamento,
                            EmailsCriados = 0,
                            EnderecosCriados = 0,
                            PessoasAdicionadas = 0,
                            TelefonesCriados = 0
                        };

                        arquivoImportadoManipular = await arquivoImportadoService.ImportarXLSLote(data, arquivoImportadoManipular);
                    }




                }
                return Result<bool>.Ok(true);



            }
            catch(Exception e)
            {
                return Result<bool>.Err(e.Message);
            }

        }

    }
}
