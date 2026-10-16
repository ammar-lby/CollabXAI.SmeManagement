using CollabXAI.Application.Dtos;
using CollabXAI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollabXAI.Api.Controllers;

[ApiController]
[Route("api/smes")]
public class SmesController : ControllerBase
{
    private readonly ISmeService _smeService;

    public SmesController(ISmeService smeService)
    {
        _smeService = smeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SmeResponse>>> GetAll()
    {
        return Ok(await _smeService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SmeResponse>> GetById(Guid id)
    {
        return Ok(await _smeService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<SmeResponse>> Create(CreateSmeRequest request)
    {
        var created = await _smeService.CreateAsync(request);

        // 201 with a Location header pointing at the new sme
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")] //update an existing guid 
    public async Task<ActionResult<SmeResponse>> Update(Guid id, UpdateSmeRequest request)
    {
        return Ok(await _smeService.UpdateAsync(id, request));
    }

    [HttpDelete("{id:guid}")] 
    public async Task<IActionResult> Delete(Guid id)
    {
        await _smeService.DeleteAsync(id);
        return NoContent();
    }
}
