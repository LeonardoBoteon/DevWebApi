using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevWeb.Api.Data;
using DevWeb.Api.Models;

namespace DevWeb.Api.Controllers;

[ApiController]

[Route("api/[controller]")]
public class ArtistasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ArtistasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Artista>>> GetArtistas()
    {
        var artistas = await _context.Artistas.ToListAsync();
        return Ok(artistas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Artista>> GetArtista(int id)
    {
        var artista = await _context.Artistas
            .Include(a => a.Obras)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (artista == null)
        {
            return NotFound(new {mensagem = $"Artista com o id {id} não encontrado."});
        }
        return Ok(artista);
    }

    [HttpPost]
    public async Task<ActionResult<Artista>> PostArtista (Artista artista)
    {
        _context.Artistas.Add(artista);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArtista), new { id = artista.Id}, artista);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutArtista (int id, Artista artista)
    {
        if (id != artista.Id)
        {
            return BadRequest(new {mensagem = "O ID da URL não corresponde ao ID do Artista."});
        }

        var artistaExistente = await _context.Artistas.FindAsync(id);

        if (artistaExistente == null)
        {
            return NotFound(new {mensagem = $"Artista com ID {id} não encontrado."});
        }

        artistaExistente.Nome = artista.Nome;
        artistaExistente.Ano = artista.Ano;
        artistaExistente.Pais = artista.Pais;
        artistaExistente.Descricao = artista.Descricao;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteArtista (int id)
    {
        var artista = await _context.Artistas.FindAsync(id);

        if (artista == null)
        {
            return NotFound(new {mensagem = $"Artista com ID {id} não encontrado."});
        }

        try
        {
            _context.Artistas.Remove(artista);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest (new
            {
                mensagem = "Não é possível deletar esse Artista porque ele possui obras associadas. " +
                "Remova ou reatribua as obras antes de deletar o artista."
            });
        }

        return NoContent();
    }

}