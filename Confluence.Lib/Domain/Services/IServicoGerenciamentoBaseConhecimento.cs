namespace Confluence.Lib.Domain.Services;

using Aggregates;
using ValueObjects;

/// <summary>
/// Domain Service que encapsula lógica complexa de gerenciamento de bases de conhecimento.
/// Orquestra múltiplas agregações e mantém invariantes de domínio.
/// Implementa o Single Responsibility Principle (SOLID).
/// </summary>
public interface IServicoGerenciamentoBaseConhecimento
{
    /// <summary>
    /// Cria uma nova base de conhecimento para um usuário.
    /// Validações: proprietário deve ser válido.
    /// </summary>
    Task<BaseConhecimento> CriarBaseConhecimentoAsync(
        UsuarioId idProprietario,
        string nome,
        string descricao,
        bool isPublica = false);

    /// <summary>
    /// Adiciona um novo artigo a uma base existente.
    /// Validações: base deve existir, artigo deve ser válido.
    /// </summary>
    Task<Entities.Artigo> AdicionarArtigoAsync(
        AggregateId idBase,
        string titulo,
        ConteudoArtigo conteudo,
        Categoria categoria,
        bool isPublico = false);

    /// <summary>
    /// Atualiza o conteúdo de um artigo.
    /// Validações: artigo e base devem existir.
    /// </summary>
    Task AtualizarArtigoAsync(
        AggregateId idBase,
        ArticuloId idArtigo,
        string? novoTitulo = null,
        ConteudoArtigo? novoConteudo = null,
        Categoria? novaCategoria = null);

    /// <summary>
    /// Remove um artigo de uma base.
    /// </summary>
    Task RemoverArtigoAsync(AggregateId idBase, ArticuloId idArtigo);

    /// <summary>
    /// Publica uma base de conhecimento (torna pública).
    /// Validações: apenas proprietário pode publicar.
    /// </summary>
    Task PublicarBaseConhecimentoAsync(
        UsuarioId idProprietario,
        AggregateId idBase);

    /// <summary>
    /// Despublica uma base (torna privada).
    /// </summary>
    Task DespublicarBaseConhecimentoAsync(
        UsuarioId idProprietario,
        AggregateId idBase);

    /// <summary>
    /// Deleta uma base de conhecimento e todos os seus artigos.
    /// Validações: apenas proprietário pode deletar.
    /// </summary>
    Task DeletarBaseConhecimentoAsync(
        UsuarioId idProprietario,
        AggregateId idBase);

    /// <summary>
    /// Obtém uma base com autorização validada.
    /// </summary>
    Task<BaseConhecimento?> ObterBaseComAutorizacaoAsync(
        AggregateId idBase,
        UsuarioId idUsuario);
}
