public class BaseConhecimento
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public Boolean IsPublico { get; set; }
}