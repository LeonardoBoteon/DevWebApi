using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevWeb.Api.Models;

public class Obra
{
    public int Id {get; set; }

    public required string Nome {get; set; }

    public int? Ano {get; set;}
    
    public int CategoriaId {get; set;}

    public int ArtistaId {get; set;}

    public int GaleriaId {get; set;}

    public DetalheObra? DetalheObra {get; set;}

    public Categoria? Categoria {get; set;}

    public Artista? Artista {get; set;}

    public Galeria? Galeria {get; set;}
}