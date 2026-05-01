using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevWeb.Api.Data;
using DevWeb.Api.Models;

namespace DevWeb.Api.Controllers;

[ApiController]

[Route("api/[controller]")]
public class GaleriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public GaleriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Galeria>>> GetGaleria()
    {
        var galerias = await _context.Galerias.ToListAsync();
        return Ok(galerias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Galeria>> GetGaleria(int id)
    {
        var galeria = await _context.Galerias
            .Include(g => g.Obras)
            .FirstOrDefaultAsync(g => g.Id == id);
        if (galeria == null)
        {
            return NotFound(new {mensagem = $"Galeria com o id {id} não encontrado."});
        }
        return Ok(galeria);
    }

    [HttpPost]
    public async Task<ActionResult<Galeria>> PostGaleria (Galeria galeria)
    {
        _context.Galerias.Add(galeria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGaleria), new { id = galeria.Id}, galeria);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutGaleria (int id, Galeria galeria)
    {
        if (id != galeria.Id)
        {
            return BadRequest(new {mensagem = "O ID da URL não corresponde ao ID da Galeria."});
        }

        var galeriaExistente = await _context.Galerias.FindAsync(id);

        if (galeriaExistente == null)
        {
            return NotFound(new {mensagem = $"Galeria com ID {id} não encontrado."});
        }

        galeriaExistente.Nome = galeria.Nome;
        galeriaExistente.Descricao = galeria.Descricao;
        galeriaExistente.Telefone = galeria.Telefone;
        galeriaExistente.Email = galeria.Email;
        galeriaExistente.Site = galeria.Site;
        galeriaExistente.Endereco = galeria.Endereco;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGaleria (int id)
    {
        var galeria = await _context.Galerias.FindAsync(id);

        if (galeria == null)
        {
            return NotFound(new {mensagem = $"Galeria com ID {id} não encontrado."});
        }

        try
        {
            _context.Galerias.Remove(galeria);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest (new
            {
                mensagem = "Não é possível deletar essa Galeria porque ela possui obras associadas. " +
                "Remova ou reatribua as obras antes de deletar a galeria."
            });
        }

        return NoContent();
    }

}