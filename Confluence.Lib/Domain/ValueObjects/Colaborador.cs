namespace Confluence.Lib.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um colaborador com acesso compartilhado a um recurso.
/// Imutável e comparável por valor.
/// Encapsula um usuário e seu nível de acesso.
/// </summary>
public sealed class Colaborador : IEquatable<Colaborador>
{
    public UsuarioId IdUsuario { get; }
    public PermissaoAcesso NivelAcesso { get; }

    private Colaborador(UsuarioId idUsuario, PermissaoAcesso nivelAcesso)
    {
        ArgumentNullException.ThrowIfNull(idUsuario);
        ArgumentNullException.ThrowIfNull(nivelAcesso);

        IdUsuario = idUsuario;
        NivelAcesso = nivelAcesso;
    }

    public static Colaborador Criar(UsuarioId idUsuario, PermissaoAcesso nivelAcesso)
        => new(idUsuario, nivelAcesso);

    /// <summary>
    /// Cria uma nova instância do colaborador com um nível de acesso diferente.
    /// </summary>
    public Colaborador ComNivelAcesso(PermissaoAcesso novoNivel)
        => new(IdUsuario, novoNivel);

    public override bool Equals(object? obj) => Equals(obj as Colaborador);

    public bool Equals(Colaborador? other)
        => other != null && IdUsuario == other.IdUsuario && NivelAcesso == other.NivelAcesso;

    public override int GetHashCode()
    {
        unchecked
        {
            return (IdUsuario.GetHashCode() * 397) ^ NivelAcesso.GetHashCode();
        }
    }

    public override string ToString() => $"{IdUsuario} ({NivelAcesso})";

    public static bool operator ==(Colaborador? esquerda, Colaborador? direita)
        => Equals(esquerda, direita);

    public static bool operator !=(Colaborador? esquerda, Colaborador? direita)
        => !Equals(esquerda, direita);
}
