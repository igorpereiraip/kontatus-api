using AutoMapper;
using ConsigIntegra.Domain.ViewModels;
using ConsigIntegra.Helper.Utilitarios;
using ConsigIntegra.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using ConsigIntegra.API.Configurations;
using System.Security.Claims;

namespace ConsigIntegra.API.Controllers
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
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
