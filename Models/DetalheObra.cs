namespace DevWeb.Api.Models;

public class DetalheObra
{
    public int Id {get; set;}
    public string? Descricao {get; set;}

    public string? Material {get; set;}

    public string? Tamanho {get; set;}

    public int ObraId {get; set;}

    public Obra? Obra {get; set;}

}