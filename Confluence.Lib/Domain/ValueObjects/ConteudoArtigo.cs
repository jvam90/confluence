namespace Confluence.Lib.Domain.ValueObjects;

/// <summary>
/// Value Object que encapsula o conteúdo de um Artigo.
/// Imutável e comparável por valor.
/// Agrupa descrição, markup e anexos relacionados.
/// </summary>
public sealed class ConteudoArtigo : IEquatable<ConteudoArtigo>
{
    private const int DescricaoMinLength = 10;
    private const int DescricaoMaxLength = 500;
    private const int MarkupMaxLength = 50000;

    public string Descricao { get; }
    public string MarkupConteudo { get; }
    public IReadOnlyList<Anexo> Anexos { get; }

    private ConteudoArtigo(string descricao, string markupConteudo, IEnumerable<Anexo>? anexos = null)
    {
        ValidarDescricao(descricao);
        ValidarMarkup(markupConteudo);

        Descricao = descricao.Trim();
        MarkupConteudo = markupConteudo.Trim();
        Anexos = anexos?.ToList().AsReadOnly() ?? new List<Anexo>().AsReadOnly();
    }

    public static ConteudoArtigo Criar(string descricao, string markupConteudo, IEnumerable<Anexo>? anexos = null)
        => new(descricao, markupConteudo, anexos);

    /// <summary>
    /// Cria uma nova instância adicionando um anexo (retorna novo objeto, não modifica o atual).
    /// </summary>
    public ConteudoArtigo AdicionarAnexo(Anexo anexo)
    {
        ArgumentNullException.ThrowIfNull(anexo);

        var anexosAtualizados = new List<Anexo>(Anexos) { anexo };
        return new ConteudoArtigo(Descricao, MarkupConteudo, anexosAtualizados);
    }

    /// <summary>
    /// Cria uma nova instância removendo um anexo.
    /// </summary>
    public ConteudoArtigo RemoverAnexo(string urlAnexo)
    {
        if (string.IsNullOrWhiteSpace(urlAnexo))
            throw new ArgumentException("URL do anexo não pode ser vazia", nameof(urlAnexo));

        var anexosAtualizados = Anexos.Where(a => a.Url != urlAnexo).ToList();
        return new ConteudoArtigo(Descricao, MarkupConteudo, anexosAtualizados);
    }

    public override bool Equals(object? obj) => Equals(obj as ConteudoArtigo);

    public bool Equals(ConteudoArtigo? other)
    {
        if (other == null)
            return false;

        return Descricao == other.Descricao &&
               MarkupConteudo == other.MarkupConteudo &&
               Anexos.SequenceEqual(other.Anexos);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 31 + Descricao.GetHashCode();
            hash = hash * 31 + MarkupConteudo.GetHashCode();

            foreach (var anexo in Anexos)
                hash = hash * 31 + anexo.GetHashCode();

            return hash;
        }
    }

    public override string ToString() => $"{Descricao} ({Anexos.Count} anexos)";

    public static bool operator ==(ConteudoArtigo? esquerda, ConteudoArtigo? direita)
        => Equals(esquerda, direita);

    public static bool operator !=(ConteudoArtigo? esquerda, ConteudoArtigo? direita)
        => !Equals(esquerda, direita);

    private static void ValidarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição não pode ser vazia", nameof(descricao));

        if (descricao.Length < DescricaoMinLength || descricao.Length > DescricaoMaxLength)
            throw new ArgumentException(
                $"Descrição deve ter entre {DescricaoMinLength} e {DescricaoMaxLength} caracteres",
                nameof(descricao));
    }

    private static void ValidarMarkup(string markup)
    {
        if (string.IsNullOrWhiteSpace(markup))
            throw new ArgumentException("Conteúdo markdown não pode ser vazio", nameof(markup));

        if (markup.Length > MarkupMaxLength)
            throw new ArgumentException(
                $"Conteúdo não pode exceder {MarkupMaxLength} caracteres",
                nameof(markup));
    }
}
