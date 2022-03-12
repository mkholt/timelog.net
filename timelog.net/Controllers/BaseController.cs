using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using timelog.net.Data;
using timelog.net.Exceptions;

namespace timelog.net.Controllers;

public abstract class BaseController<TType> : Controller
{
    protected virtual string EntityName => HttpContext.Request.RouteValues["Controller"] as string ?? "Entity";

    private readonly IRepository<TType> _repository;

    protected BaseController(IRepository<TType> repository) => _repository = repository;

    protected virtual Task Validate(int? entityId) => Task.CompletedTask;

    [HttpPost]
    public async Task<ActionResult<TType?>> Create(TType p) => await _repository.Add(p);

    [HttpGet]
    public virtual async Task<ActionResult<List<TType>>> GetAll() =>
        await HandleRequest(null, async () => (await _repository.GetAll()).ToList());

    [HttpGet]
    [Route("{entityId:int}")]
    public async Task<ActionResult<TType>> Get(int entityId) =>
        await HandleRequest(entityId, async () =>
            (await _repository.GetById(entityId)) ?? throw new ControllerException(NotFound($"{EntityName} not found: {entityId}")));

    [HttpPatch]
    [Route("{entityId:int}")]
    public async Task<ActionResult> Update(int entityId, [FromBody] TType entity)
    {
        return await HandleRequest(entityId, async () =>
        {
            try
            {
                return await _repository.Update(entityId, entity)
                    ? Ok()
                    : NotFound($"{EntityName} not found: {entityId}");
            }
            catch (Exception)
            {
                throw new ControllerException(HttpStatusCode.InternalServerError, $"An error occurred updating the {EntityName}: {entityId}");
            }
        });
    }

    [HttpDelete]
    [Route("{entityId:int}")]
    public async Task<ActionResult> Delete(int entityId)
    {
        return await HandleRequest(entityId, async () =>
            await _repository.Remove(entityId)
                ? NoContent()
                : NotFound($"{EntityName} not found: {entityId}"));
    }

    protected async Task<ActionResult<TResult>> HandleRequest<TResult>(int? entityId, Func<Task<TResult>> handler)
    {
        try
        {
            await Validate(entityId);

            return await handler();
        }
        catch (ControllerException ex)
        {
            return ex.Result ?? StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    protected async Task<ActionResult> HandleRequest(int? entityId, Func<Task<ActionResult>> handler)
    {
        try
        {
            await Validate(entityId);

            return await handler();
        }
        catch (ControllerException ex)
        {
            return ex.Result ?? StatusCode((int)ex.StatusCode, ex.Message);
        }
    }
}