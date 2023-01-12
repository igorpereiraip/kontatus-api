using AutoMapper;
using ConsigIntegra.API.Configurations;
using ConsigIntegra.Domain.Entity;
using ConsigIntegra.Domain.ViewModels;
using ConsigIntegra.Helper.Utilitarios;
using ConsigIntegra.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConsigIntegra.API.Controllers
{
    [Route("api/[controller]")]
    [ClaimRequirement(ClaimTypes.Name, "Authorization")]
    [ApiController]
    public class SolicitacaoIN100Controller
    {
        private readonly ISolicitacaoIN100Service service;
        private readonly IMapper mapper;

        public SolicitacaoIN100Controller(ISolicitacaoIN100Service service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet("getbyusuarioid/{usuarioID}")]
        public async Task<Result<List<SolicitacaoIN100>>> GetByUsuarioID(int usuarioID)
        {
            try
            {
                var logs = mapper.Map<List<SolicitacaoIN100>>(await service.GetByUsuarioID(usuarioID));

                return Result<List<SolicitacaoIN100>>.Ok(logs);
            }
            catch (Exception ex)
            {
                return Result<List<SolicitacaoIN100>>.Err(ex.Message);
            }
        }

    }
}
