namespace Confluence.Lib.Domain.Factories;

using Aggregates;
using ValueObjects;

/// <summary>
/// Implementação da Factory para criação de agregados BaseConhecimento.
/// Encapsula validações e lógica de criação complexa.
/// </summary>
public sealed class FabricaBaseConhecimento : IFabricaBaseConhecimento
{
    public BaseConhecimento CriarNovaBase(
        UsuarioId idProprietario,
        string nome,
        string descricao,
        bool isPublica = false)
    {
        ValidarDadosBase(idProprietario, nome, descricao);

        return BaseConhecimento.Criar(idProprietario, nome, descricao, isPublica);
    }

    public BaseConhecimento CriarBaseComArtigoInicial(
        UsuarioId idProprietario,
        string nomeBase,
        string descricaoBase,
        string tituloArtigo,
        ConteudoArtigo conteudoArtigo,
        Categoria categoria)
    {
        // Fazer null checks primeiro, antes de outras validações
        ArgumentNullException.ThrowIfNull(tituloArtigo);
        ArgumentNullException.ThrowIfNull(conteudoArtigo);
        ArgumentNullException.ThrowIfNull(categoria);

        ValidarDadosBase(idProprietario, nomeBase, descricaoBase);

        var base_ = BaseConhecimento.Criar(idProprietario, nomeBase, descricaoBase);

        var artigo = Entities.Artigo.Criar(tituloArtigo, conteudoArtigo, categoria);
        base_.AdicionarArtigo(artigo);

        return base_;
    }

    public void ValidarDadosBase(
        UsuarioId idProprietario,
        string nome,
        string descricao)
    {
        ArgumentNullException.ThrowIfNull(idProprietario);

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da base não pode ser vazio", nameof(nome));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição da base não pode ser vazia", nameof(descricao));

        if (nome.Length < 3 || nome.Length > 100)
            throw new ArgumentException("Nome deve ter entre 3 e 100 caracteres", nameof(nome));

        if (descricao.Length < 10 || descricao.Length > 500)
            throw new ArgumentException("Descrição deve ter entre 10 e 500 caracteres", nameof(descricao));
    }
}
