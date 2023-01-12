using AutoMapper;
using Kontatus.API.Configurations;
using Kontatus.Domain.Entity;
using Kontatus.Domain.ViewModels;
using Kontatus.Helper.Utilitarios;
using Kontatus.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kontatus.API.Controllers
{
    [Route("api/[controller]")]
    [ClaimRequirement(ClaimTypes.Name, "Authorization")]
    [ApiController]
    public class DadosIN100Controller
    {
        private readonly IDadosIN100Service service;
        private readonly IMapper mapper;

        public DadosIN100Controller(IDadosIN100Service service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet("getbyidsolicitacao/{idSolicitacao}")]
        public async Task<Result<DadosIN100>> GetByIdSolicitacao(string idSolicitacao)
        {
            try
            {
                var dados = mapper.Map<DadosIN100>(await service.GetByIdSolicitacao(idSolicitacao));

                if(dados.DataCadastro.AddDays(1) < DateTime.Now)
                {
                    throw new Exception("Data Expirada para acessar informações do IN100 solicitadas.");
                }

                return Result<DadosIN100>.Ok(dados);
            }
            catch (Exception ex)
            {
                return Result<DadosIN100>.Err(ex.Message);
            }
        }

    }
}
