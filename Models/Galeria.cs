using System.ComponentModel.DataAnnotations;

namespace DevWeb.Api.Models;

public class Galeria
{
    public int Id {get; set; }
    
    public required string Nome {get; set; }

    public string? Descricao {get; set; }

    [Phone]
    public string? Telefone {get; set; }

    [EmailAddress]
    public string? Email {get; set; }

    public string? Site {get; set; }

    public required string Endereco {get; set; }

    public ICollection<Obra> Obras {get; set;} = new List<Obra>();
}