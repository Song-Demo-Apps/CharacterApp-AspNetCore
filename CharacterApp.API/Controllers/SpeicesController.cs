using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CharacterApp.Models;
using CharacterApp.Data;
using Microsoft.EntityFrameworkCore;


namespace CharacterApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeicesController : ControllerBase
{
    
    private readonly CharacterDbContext _context;

    public SpeicesController(CharacterDbContext context)
    {
        _context = context;
    }

    // GET: api/Speices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Speices>>> GetSpeices()
    {
        var speices = await _context.Speices.ToListAsync();
        return speices;
    }

    // GET: api/Speices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Speices>> GetSpeices(int id)
    {
        var speices = await _context.Speices.FindAsync(id);

        if (speices == null)
        {
            return NoContent();
        }

        return speices;
    }

    // POST: api/Speices
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<Speices>> PostSpeices(Speices speices)
    {
        _context.Speices.Add(speices);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetSpeices", new { id = speices.Id }, speices);
    }

    // PUT: api/Speices/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSpeices(int id, Speices speices)
    {
        if (id != speices.Id)
        {
            return BadRequest();
        }
        
        _context.Update(speices);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SpeicesExists(id))
            {
                return NoContent();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Speices/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpeices(int id)
    {
        var speices = await _context.Speices.FindAsync(id);
        if (speices == null)
        {
            return NotFound();
        }

        _context.Speices.Remove(speices);
        await _context.SaveChangesAsync();

        return Ok();
    }

    private bool SpeicesExists(int id)
    {
        return _context.Speices.Any(e => e.Id == id);
    }

}