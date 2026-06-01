namespace Confluence.Lib.Domain.ValueObjects;

/// <summary>
/// Value Object que representa o identificador único de um Artigo dentro de uma Base.
/// </summary>
public sealed class ArtigoId : IEquatable<ArtigoId>
{
    public Guid Valor { get; }

    private ArtigoId(Guid valor)
    {
        if (valor == Guid.Empty)
            throw new ArgumentException("ArtigoId não pode ser vazio", nameof(valor));

        Valor = valor;
    }

    public static ArtigoId Criar() => new(Guid.NewGuid());

    public static ArtigoId Criar(Guid valor) => new(valor);

    public override bool Equals(object? obj) => Equals(obj as ArtigoId);

    public bool Equals(ArtigoId? other) => other != null && Valor == other.Valor;

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => Valor.ToString();

    public static bool operator ==(ArtigoId? esquerda, ArtigoId? direita)
        => Equals(esquerda, direita);

    public static bool operator !=(ArtigoId? esquerda, ArtigoId? direita)
        => !Equals(esquerda, direita);
}
