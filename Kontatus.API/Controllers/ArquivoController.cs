using AutoMapper;
using Kontatus.Domain.ViewModels;
using Kontatus.Helper.Utilitarios;
using Kontatus.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Kontatus.API.Configurations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ExcelDataReader;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;

namespace Kontatus.API.Controllers
{
    [Route("api/[controller]")]
    [ClaimRequirement(ClaimTypes.Name, "Authorization")]
    [ApiController]
    public class ArquivoController: ControllerBase
    {
        private readonly IArquivoService service;
        private readonly IMapper mapper;

        public ArquivoController(IArquivoService service, IMapper mapper)
        {
            this.service = service;
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
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = arquivoDTO.Arquivos[0].OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        try
                        {
                            var nome = reader.GetValue(3).ToString();
                            var cpf = reader.GetValue(5).ToString();
                            var ddd = reader.GetValue(1).ToString();
                            var numeroTelefone = reader.GetValue(2).ToString();

                            var pessoa = new Pessoa()
                            {
                                Nome = nome,
                                CPF = cpf,
                                Idade = 0
                            };
                            var telefone = new Telefone()
                            {
                                NumeroTelefone = ddd + numeroTelefone,
                            };

                            var result = await service.ImportarXLS(pessoa, telefone);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                    }
                }
            }
            return Result<bool>.Ok(true);
        }

    }
}
