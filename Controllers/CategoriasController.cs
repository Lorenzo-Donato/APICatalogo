using System;
using System.Security.Cryptography.X509Certificates;
using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _repository;
    private readonly ILogger _logger;
    public CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
    
            var categorias = _repository.GetCategorias();
            return Ok(categorias);
    }

    [HttpGet("id:int", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        var categoria = _repository.GetCategoria(id);
        
        if (categoria is null)
        {
            return NotFound("Categoria nula");
        }
        return Ok(categoria);     
    }


    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria is null)
        {
            return BadRequest("Cateoria nula");
        }

        var categoriaCriada = _repository.Create(categoria);

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return BadRequest("Categoria não encontrada");
        }
        
        _repository.Update(categoria);
        return Ok(categoria);
    }   

    [HttpDelete("id:int")]
    public ActionResult<Categoria> Delete(int id)
    {
        var categoria = _repository.GetCategoria(id);

        if (categoria is null)
        {
            return NotFound("Categoria não encontrada...");
        }

        var categoriaExcluida = _repository.Delete(id);
        return Ok(categoriaExcluida);
    }
}
