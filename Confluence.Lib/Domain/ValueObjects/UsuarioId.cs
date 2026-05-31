namespace Confluence.Lib.Domain.ValueObjects;

/// <summary>
/// Value Object que representa o identificador único de um Usuário.
/// Implementa imutabilidade e comparação por valor.
/// </summary>
public sealed class UsuarioId : IEquatable<UsuarioId>
{
    public Guid Valor { get; }

    private UsuarioId(Guid valor)
    {
        if (valor == Guid.Empty)
            throw new ArgumentException("UsuarioId não pode ser vazio", nameof(valor));

        Valor = valor;
    }

    public static UsuarioId Criar() => new(Guid.NewGuid());

    public static UsuarioId Criar(Guid valor) => new(valor);

    public override bool Equals(object? obj) => Equals(obj as UsuarioId);

    public bool Equals(UsuarioId? other) => other != null && Valor == other.Valor;

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => Valor.ToString();

    public static bool operator ==(UsuarioId? esquerda, UsuarioId? direita)
        => Equals(esquerda, direita);

    public static bool operator !=(UsuarioId? esquerda, UsuarioId? direita)
        => !Equals(esquerda, direita);
}
