namespace Confluence.Lib.Domain.ValueObjects;

/// <summary>
/// Value Object que define o nível de acesso a um recurso compartilhado.
/// Imutável e comparável por valor.
/// </summary>
public sealed class PermissaoAcesso : IEquatable<PermissaoAcesso>
{
    public NivelAcesso Valor { get; }

    private PermissaoAcesso(NivelAcesso valor)
    {
        if (!Enum.IsDefined(typeof(NivelAcesso), valor))
            throw new ArgumentException("Nível de acesso inválido", nameof(valor));

        Valor = valor;
    }

    public static PermissaoAcesso Leitor => new(NivelAcesso.Leitor);
    public static PermissaoAcesso Editor => new(NivelAcesso.Editor);
    public static PermissaoAcesso Proprietario => new(NivelAcesso.Proprietario);

    public static PermissaoAcesso Criar(NivelAcesso nivel) => new(nivel);

    public bool TemPermissaoLeitura => true;

    public bool TemPermissaoEdicao => Valor == NivelAcesso.Editor || Valor == NivelAcesso.Proprietario;

    public bool TemPermissaoCompartilhamento => Valor == NivelAcesso.Proprietario;

    public bool TemPermissaoDeletacao => Valor == NivelAcesso.Proprietario;

    public override bool Equals(object? obj) => Equals(obj as PermissaoAcesso);

    public bool Equals(PermissaoAcesso? other) => other != null && Valor == other.Valor;

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => Valor.ToString();

    public static bool operator ==(PermissaoAcesso? esquerda, PermissaoAcesso? direita)
        => Equals(esquerda, direita);

    public static bool operator !=(PermissaoAcesso? esquerda, PermissaoAcesso? direita)
        => !Equals(esquerda, direita);
}

/// <summary>
/// Enum que define os níveis de acesso disponíveis.
/// </summary>
public enum NivelAcesso
{
    Leitor = 1,
    Editor = 2,
    Proprietario = 3
}
