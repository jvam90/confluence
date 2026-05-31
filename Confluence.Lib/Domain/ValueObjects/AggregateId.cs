namespace Confluence.Lib.Domain.ValueObjects;

/// <summary>
/// Value Object que representa o identificador único de uma Base de Conhecimento.
/// Aggregate Root ID do Bounded Context de Gerenciamento de Conhecimento.
/// </summary>
public sealed class AggregateId : IEquatable<AggregateId>
{
    public Guid Valor { get; }

    private AggregateId(Guid valor)
    {
        if (valor == Guid.Empty)
            throw new ArgumentException("AggregateId não pode ser vazio", nameof(valor));

        Valor = valor;
    }

    public static AggregateId Criar() => new(Guid.NewGuid());

    public static AggregateId Criar(Guid valor) => new(valor);

    public override bool Equals(object? obj) => Equals(obj as AggregateId);

    public bool Equals(AggregateId? other) => other != null && Valor == other.Valor;

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => Valor.ToString();

    public static bool operator ==(AggregateId? esquerda, AggregateId? direita)
        => Equals(esquerda, direita);

    public static bool operator !=(AggregateId? esquerda, AggregateId? direita)
        => !Equals(esquerda, direita);
}
