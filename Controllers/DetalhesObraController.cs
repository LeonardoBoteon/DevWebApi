using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevWeb.Api.Data;
using DevWeb.Api.Models;

namespace DevWeb.Api.Controllers;

[ApiController]

[Route("api/[controller]")]
public class DetalhesObraController : ControllerBase
{
    private readonly AppDbContext _context;
    public DetalhesObraController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("obra/{obraId}")]
    public async Task<ActionResult<DetalheObra>>
    GetDetalhePorObra(int obraId)
    {
        var detalhe = await _context.DetalhesObra
            .Include(d => d.Obra)
            .FirstOrDefaultAsync(d => d.ObraId == obraId);
        
        if (detalhe == null)
        {
            return NotFound (new
            {
                mensagem = $"Nenhum detalhe encontrado para a obra de ID {obraId}."

            });
        }

        return Ok(detalhe);
    }

    [HttpPost]
    public async Task<ActionResult<DetalheObra>> PostDetalhe(DetalheObra detalhe)
    {
        var obra = await _context.Obras.FindAsync(detalhe.ObraId);
        if (obra == null)
        {
            return BadRequest(new
            {
                mensagem = $"Obra com ID {detalhe.ObraId} não encontrada."
            });
        }

        var detalheExistente = await _context.DetalhesObra
            .FirstOrDefaultAsync(d => d.ObraId == detalhe.ObraId);
        
        if (detalheExistente != null)
        {
            return Conflict(new
            {
                mensagem = $"A obra de ID {detalhe.ObraId} já possui um detalhe cadastrado. " + "Use PUT para atualizar o detalhe existente."
            });
        }

        _context.DetalhesObra.Add(detalhe);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetDetalhePorObra),
            new { ObraId = detalhe.ObraId },
            detalhe
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDetalhe(int id, DetalheObra detalhe)
    {
        if (id != detalhe.Id)
        {
            return BadRequest(new { mensagem = "O ID da URL não corresponde ao ID do detalhe."});
        }

        var detalheExistente = await _context.DetalhesObra.FindAsync(id);

        if (detalheExistente == null)
        {
            return NotFound(new {mensagem = $"Detalhe com id {id} não encontrado."});
        }

        detalheExistente.Descricao = detalhe.Descricao;
        detalheExistente.Material = detalhe.Material;
        detalheExistente.Tamanho = detalhe.Tamanho;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDetalhe(int id)
    {
        var detalhe = await _context.DetalhesObra.FindAsync(id);
        if (detalhe == null)
        {
            return NotFound(new {mesnsagem = $"Detalhe com ID {id} não encontrado."});
        }

        _context.DetalhesObra.Remove(detalhe);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}