public class User
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string NomeUsuario { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public ICollection<BaseConhecimento> BasesConhecimento { get; set; } = null!;
}