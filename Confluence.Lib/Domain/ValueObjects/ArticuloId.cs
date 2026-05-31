namespace Confluence.Lib.Domain.ValueObjects;

/// <summary>
/// Value Object que representa o identificador único de um Artigo dentro de uma Base.
/// </summary>
public sealed class ArticuloId : IEquatable<ArticuloId>
{
    public Guid Valor { get; }

    private ArticuloId(Guid valor)
    {
        if (valor == Guid.Empty)
            throw new ArgumentException("ArticuloId não pode ser vazio", nameof(valor));

        Valor = valor;
    }

    public static ArticuloId Criar() => new(Guid.NewGuid());

    public static ArticuloId Criar(Guid valor) => new(valor);

    public override bool Equals(object? obj) => Equals(obj as ArticuloId);

    public bool Equals(ArticuloId? other) => other != null && Valor == other.Valor;

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => Valor.ToString();

    public static bool operator ==(ArticuloId? esquerda, ArticuloId? direita)
        => Equals(esquerda, direita);

    public static bool operator !=(ArticuloId? esquerda, ArticuloId? direita)
        => !Equals(esquerda, direita);
}
