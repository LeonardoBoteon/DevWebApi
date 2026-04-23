using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevWeb.Api.Data;
using DevWeb.Api.Models;

namespace DevWeb.Api.Controllers;

[ApiController]

[Route("api/[controller]")]
public class ObrasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ObrasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProducesResponseTypeAttribute>>> GetObras()
    {
        var obras = await _context.Obras
            .Include(o => o.Categoria)
            .ToListAsync();

        return Ok(obras);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Obra>> GetObra(int id)
    {
        var obra = await _context.Obras
            .Include(o => o.Categoria)
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (obra == null)
        {
            return NotFound(new {mensagem = $"Obra com ID {id} não encontrado"});
        }

        return Ok(obra);
    }

    [HttpPost]
    public async Task<ActionResult<Obra>> PostObra(Obra obra)
    {
        _context.Obras.Add(obra);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetObra), new { id = obra.Id}, obra);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutObra(int id, Obra obra)
    {
        if (id != obra.Id)
        {
            return BadRequest(new {mensagem = "O ID da URL não corresponde ao ID da Obra no body"});
        }

        var obraExistente = await _context.Obras.FindAsync(id);

        if (obraExistente == null)
        {
            return NotFound(new {mensagem = $"Produto com ID {id} não encontrado."});
        }

        obraExistente.Nome = obra.Nome;
        obraExistente.Ano = obra.Ano;
        obraExistente.Descricao = obra.Descricao;
        obraExistente.CategoriaId = obra.CategoriaId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteObra(int id)
    {
        var obra = await _context.Obras.FindAsync(id);

        if (obra == null)
        {
            return NotFound(new {mensagem = $"Produto com ID {id} não encontrado."});
        }

        _context.Obras.Remove(obra);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}