using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevWeb.Api.Models;

public class Obra
{
    public int Id {get; set; }

    public required string Nome {get; set; }

    public int? Ano {get; set; }

    public string? Descricao {get; set; }
    
    public int CategoriaId { get; set;}

    public Categoria? Categoria {get; set;}

}