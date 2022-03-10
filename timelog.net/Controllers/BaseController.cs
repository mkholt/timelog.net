using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using timelog.net.Data;

namespace timelog.net.Controllers;

public abstract class BaseController<TType> : Controller
{
    protected abstract string EntityName { get; }
    private readonly IRepository<TType> _repository;

    protected BaseController(IRepository<TType> repository) => _repository = repository;
        
    [HttpPost]
    public async Task<ActionResult<TType?>> Create(TType p) => await _repository.Add(p);

    [HttpGet]
    public async Task<ActionResult<List<TType>>> GetAll()
    {
        var entities = await _repository.GetAll();
        return entities.ToList();
    }
        
    [HttpGet]
    [Route("{entityId:int}")]
    public async Task<ActionResult<TType>> Get(int entityId)
    {
        var entity = await _repository.GetById(entityId);
        return entity is null
            ? NotFound($"{EntityName} not found: {entityId}")
            : entity;
    }

    [HttpPatch]
    [Route("{entityId:int}")]
    public async Task<IActionResult> Update(int entityId, [FromBody] TType entity)
    {
        try
        {
            return await _repository.Update(entityId, entity)
                ? Ok()
                : NotFound($"{EntityName} not found: {entityId}");
        } catch (Exception)
        {
            return StatusCode(500, $"An error occurred updating the {EntityName}: {entityId}");
        }
    }

    [HttpDelete]
    [Route("{entityId:int}")]
    public async Task<IActionResult> Delete(int entityId)
    {
        return await _repository.Remove(entityId)
            ? NoContent()
            : NotFound($"{EntityName} not found: {entityId}");
    }
}