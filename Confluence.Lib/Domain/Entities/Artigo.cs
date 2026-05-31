namespace Confluence.Lib.Domain.Entities;

using ValueObjects;

/// <summary>
/// Entity que representa um artigo dentro de uma Base de Conhecimento.
/// Possui identidade única (ArticuloId) e pode ser modificada.
/// Faz parte do agregado BaseConhecimento.
/// </summary>
public sealed class Artigo
{
    private string _titulo = null!;
    private ConteudoArtigo _conteudo = null!;
    private bool _isPublico;
    private DateTime _dataAtualizacao;

    public ArticuloId Id { get; }
    public string Titulo
    {
        get => _titulo;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Título não pode ser vazio", nameof(value));
            if (value.Length < 5 || value.Length > 200)
                throw new ArgumentException("Título deve ter entre 5 e 200 caracteres", nameof(value));
            _titulo = value.Trim();
        }
    }

    public ConteudoArtigo Conteudo
    {
        get => _conteudo;
        private set
        {
            ArgumentNullException.ThrowIfNull(value);
            _conteudo = value;
        }
    }

    public Categoria Categoria { get; private set; }
    public bool IsPublico
    {
        get => _isPublico;
        private set => _isPublico = value;
    }

    public DateTime DataCriacao { get; private set; }
    public DateTime DataAtualizacao
    {
        get => _dataAtualizacao;
        private set => _dataAtualizacao = value;
    }

    private Artigo(
        ArticuloId id,
        string titulo,
        ConteudoArtigo conteudo,
        Categoria categoria,
        bool isPublico = false)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(titulo);
        ArgumentNullException.ThrowIfNull(conteudo);
        ArgumentNullException.ThrowIfNull(categoria);

        Id = id;
        Titulo = titulo;
        Conteudo = conteudo;
        Categoria = categoria;
        IsPublico = isPublico;
        DataCriacao = DateTime.UtcNow;
        _dataAtualizacao = DataCriacao;
    }

    /// <summary>
    /// Factory method para criar um novo artigo.
    /// </summary>
    public static Artigo Criar(
        string titulo,
        ConteudoArtigo conteudo,
        Categoria categoria,
        bool isPublico = false)
    {
        return new Artigo(
            ArticuloId.Criar(),
            titulo,
            conteudo,
            categoria,
            isPublico);
    }

    /// <summary>
    /// Factory method para recriar um artigo existente (para persistência).
    /// </summary>
    public static Artigo ReconstruirDoRepositorio(
        ArticuloId id,
        string titulo,
        ConteudoArtigo conteudo,
        Categoria categoria,
        bool isPublico,
        DateTime dataCriacao,
        DateTime dataAtualizacao)
    {
        ArgumentNullException.ThrowIfNull(id);

        var artigo = new Artigo(id, titulo, conteudo, categoria, isPublico)
        {
            DataCriacao = dataCriacao,
            DataAtualizacao = dataAtualizacao
        };

        return artigo;
    }

    public void AtualizarTitulo(string novoTitulo)
    {
        Titulo = novoTitulo;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AtualizarConteudo(ConteudoArtigo novoConteudo)
    {
        Conteudo = novoConteudo;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AtualizarCategoria(Categoria novaCategoria)
    {
        ArgumentNullException.ThrowIfNull(novaCategoria);
        Categoria = novaCategoria;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Publicar()
    {
        if (IsPublico)
            throw new InvalidOperationException("Artigo já está publicado");

        IsPublico = true;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Despublicar()
    {
        if (!IsPublico)
            throw new InvalidOperationException("Artigo não está publicado");

        IsPublico = false;
        DataAtualizacao = DateTime.UtcNow;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Artigo artigo)
            return false;
        return Id == artigo.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => $"{Titulo} ({Categoria})";
}
