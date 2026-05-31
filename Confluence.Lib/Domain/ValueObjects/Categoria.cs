namespace Confluence.Lib.Domain.ValueObjects;

/// <summary>
/// Value Object que representa a classificação de um Artigo.
/// Imutável e comparável por valor.
/// Exemplos: "Backend", "Frontend", "DevOps", "Documentação"
/// </summary>
public sealed class Categoria : IEquatable<Categoria>
{
    private const int NomeMinLength = 3;
    private const int NomeMaxLength = 50;

    public string Nome { get; }
    public string? Descricao { get; }

    private Categoria(string nome, string? descricao = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da categoria não pode ser vazio", nameof(nome));

        if (nome.Length < NomeMinLength || nome.Length > NomeMaxLength)
            throw new ArgumentException(
                $"Nome da categoria deve ter entre {NomeMinLength} e {NomeMaxLength} caracteres",
                nameof(nome));

        Nome = nome.Trim();
        Descricao = descricao?.Trim();
    }

    public static Categoria Criar(string nome, string? descricao = null)
        => new(nome, descricao);

    public override bool Equals(object? obj) => Equals(obj as Categoria);

    public bool Equals(Categoria? other)
        => other != null && Nome == other.Nome;

    public override int GetHashCode() => Nome.GetHashCode();

    public override string ToString() => Nome;

    public static bool operator ==(Categoria? esquerda, Categoria? direita)
        => Equals(esquerda, direita);

    public static bool operator !=(Categoria? esquerda, Categoria? direita)
        => !Equals(esquerda, direita);
}
