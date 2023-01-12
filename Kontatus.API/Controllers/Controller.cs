using ConsigIntegra.API.Configurations;
using ConsigIntegra.Helper.Utilitarios;
using ConsigIntegra.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GED.API.Controllers
{
  interface IController<T> where T : ConsigIntegra.Domain.Entity.Base, new()
  {
    Task<Result<IEnumerable<T>>> List(bool? basico);
    Task<Result<T>> Get(int id);
    Task<Result<T>> Create(T entidade);
    Task<Result<T>> Update(int id, T entidade);
    Task<Result<T>> Delete(int id);
    Result<T> Inactivate(int id);
  }

  [Route("api/[controller]")]
    [ApiController]
  public abstract class Controller<T> : ControllerBase, IController<T> where T : ConsigIntegra.Domain.Entity.Base, new()
  {
    protected readonly IService<T> service;

    protected Controller(IService<T> service)
    {
      this.service = service;
    }

    /// <summary>
    /// Retorna todos os registros
    /// </summary>
    /// <param name="basico">Indica se deve ser enviado apenas as informações basicas de cada registro</param>
    /// <returns>Array de todos os registros</returns>
    [HttpGet]
    public virtual async Task<Result<IEnumerable<T>>> List([FromQuery] bool? basico)
    {
      try
      {
        return Result<IEnumerable<T>>.Ok(await service.List(basico ?? false));
      }
      catch (Exception ex)
      {
        return Result<IEnumerable<T>>.Err(ex.Message);
      }
    }

    /// <summary>
    /// Retorna o registro de acordo com o Id
    /// </summary>
    /// <returns>Registro com o id informado</returns>
    [HttpGet("{id}")]
    public virtual async Task<Result<T>> Get(int id)
    {
      try
      {
        var result = await service.GetById(id);
        return Result<T>.Ok(result);
      }
      catch (Exception ex)
      {
        return Result<T>.Err(ex.Message);
      }


    }

    /// <summary>
    /// Cria um novo registro com os dados informados
    /// </summary>
    /// <returns>Registro criado contendo o Id</returns>
    [HttpPost]
    public virtual async Task<Result<T>> Create([FromBody] T entidade)
    {
      try
      {
        return Result<T>.Ok(await service.Create(entidade));
      }
      catch (Exception ex)
      {
        return Result<T>.Err(ex.Message);
      }
    }

    /// <summary>
    /// Atualiza um registro de acordo com o id e dados informados
    /// </summary>
    /// <returns>Registro atualizado</returns>
    [HttpPut("{id}")]
    public virtual async Task<Result<T>> Update(int id, [FromBody] T entidade)
    {
      try
      {
        entidade.ID = id;
        return Result<T>.Ok(await service.Update(entidade));
      }
      catch (Exception ex)
      {
        return Result<T>.Err(ex.Message);
      }
    }

    /// <summary>
    /// Remove um registro de acordo com o id
    /// </summary>
    /// <returns>Status</returns>
    [HttpDelete("{id}")]
    public virtual async Task<Result<T>> Delete(int id)
    {
      try
      {
        await service.Delete(id);
        return Result<T>.Ok();
      }
      catch (Exception ex)
      {
        return Result<T>.Err(ex.Message);
      }
    }

    /// <summary>
    /// Faz a inativação (exclusão logica) de um registro de acordo com o id
    /// </summary>
    /// <returns>Status</returns>
    [HttpPut("{id}/Inativar")]
    public virtual Result<T> Inactivate(int id)
    {
      try
      {
        service.Inactivate(id).Wait();

        return Result<T>.Ok();
      }
      catch (Exception ex)
      {
        return Result<T>.Err(ex.Message);
      }
    }

    /// <summary>
    /// Faz a ativação  de um registro de acordo com o id
    /// </summary>
    /// <returns>Status</returns>
    [HttpPut("{id}/Ativar")]
    public virtual async Task<Result<T>> Activate(int id)
    {
      try
      {
        await service.Activate(id);
        return Result<T>.Ok();
      }
      catch (Exception ex)
      {
        return Result<T>.Err(ex.Message);
      }
    }


  }
}
