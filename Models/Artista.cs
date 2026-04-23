using System.ComponentModel.DataAnnotations;

namespace DevWeb.Api.Models;

public class Artista
{
    public int Id{get; set; }

    public required string Nome{get; set; }

    [MaxLength(4)]
    public int? Ano {get; set; }

    public string? Pais {get; set; }

    public string? Descricao{get; set; }


    public required List<Obra> Obras {get; set; }
}