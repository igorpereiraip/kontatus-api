using AutoMapper;
using ConsigIntegra.API.Configurations;
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
    public class LogUsuarioController
    {
        private readonly ILogUsuarioService service;
        private readonly IMapper mapper;

        public LogUsuarioController(ILogUsuarioService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet("getbyusuarioid/{usuarioID}")]
        public async Task<Result<List<LogUsuarioViewModel>>> GetByUsuarioID(int usuarioID)
        {
            try
            {
                var logs = mapper.Map<List<LogUsuarioViewModel>>(await service.GetByUsuarioID(usuarioID));

                return Result<List<LogUsuarioViewModel>>.Ok(logs);
            }
            catch (Exception ex)
            {
                return Result<List<LogUsuarioViewModel>>.Err(ex.Message);
            }
        }

        [HttpGet("getbyusuarioidfiltrado/{usuarioID}/{filtro}")]
        public async Task<Result<List<LogUsuarioViewModel>>> GetByUsuarioIDFiltrado(int usuarioID, string filtro)
        {
            try
            {
                var logs = mapper.Map<List<LogUsuarioViewModel>>(await service.GetByUsuarioIDFiltrado(usuarioID, filtro));

                return Result<List<LogUsuarioViewModel>>.Ok(logs);
            }
            catch (Exception ex)
            {
                return Result<List<LogUsuarioViewModel>>.Err(ex.Message);
            }
        }

    }
}
