namespace Confluence.Lib.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um arquivo anexado a um Artigo.
/// Imutável e comparável por valor.
/// </summary>
public sealed class Anexo : IEquatable<Anexo>
{
    public string Nome { get; }
    public string Url { get; }
    public TipoAnexo Tipo { get; }

    private Anexo(string nome, string url, TipoAnexo tipo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do anexo não pode ser vazio", nameof(nome));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL do anexo não pode ser vazia", nameof(url));

        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            throw new ArgumentException("URL do anexo inválida", nameof(url));

        Nome = nome.Trim();
        Url = url.Trim();
        Tipo = tipo;
    }

    public static Anexo Criar(string nome, string url, TipoAnexo tipo)
        => new(nome, url, tipo);

    public override bool Equals(object? obj) => Equals(obj as Anexo);

    public bool Equals(Anexo? other)
        => other != null && Url == other.Url;

    public override int GetHashCode() => Url.GetHashCode();

    public override string ToString() => $"{Nome} ({Tipo})";

    public static bool operator ==(Anexo? esquerda, Anexo? direita)
        => Equals(esquerda, direita);

    public static bool operator !=(Anexo? esquerda, Anexo? direita)
        => !Equals(esquerda, direita);
}

/// <summary>
/// Enum que representa os tipos de anexos permitidos.
/// </summary>
public enum TipoAnexo
{
    Imagem = 1,
    PDF = 2,
    Video = 3,
    Documento = 4,
    Outro = 5
}
