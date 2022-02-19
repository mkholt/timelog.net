using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using timelog.net.Data;

namespace timelog.net.Controllers;

public abstract class BaseController<TType> : Controller
{
    private readonly IRepository<TType> _repository;

    protected BaseController(IRepository<TType> repository) => _repository = repository;
        
    [HttpPost]
    public async Task<ActionResult<TType>> Create(TType p) => await _repository.Add(p);

    [HttpGet]
    public async Task<ActionResult<List<TType>>> GetAll()
    {
        var entitities = await _repository.GetAll();
        return entitities.ToList();
    }
        
    [HttpGet]
    [Route("{entityId:int}")]
    public async Task<ActionResult<TType>> Get(int entityId)
    {
        var entity = await _repository.GetById(entityId);
        return entity is null
            ? NotFound(entityId)
            : entity;
    }

    [HttpPatch]
    [Route("{entityId:int}")]
    public async Task<IActionResult> Update(int entityId, [FromBody] TType entity)
    {
        return await _repository.Update(entityId, entity)
            ? Ok()
            : NotFound(entityId);
    }

    [HttpDelete]
    [Route("{entityId:int}")]
    public async Task<IActionResult> Delete(int entityId)
    {
        return await _repository.Remove(entityId)
            ? Ok()
            : NotFound(entityId);
    }
}