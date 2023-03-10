namespace ScriptPack.Domain;

/// <summary>
/// Interface que define um nodo na árvore de pacotes de scripts.
/// </summary>
/// <remarks>
/// A árvore de pacotes de script organiza os scripts em pastas virtuais que
/// podem ser navegadas e selecionadas para execução.
/// </remarks>
public interface INode
{
  /// <summary>
  /// Obtém ou define o nodo pai.
  /// </summary>
  /// <remarks>
  /// Se este nodo não tem pai, o valor retornado é nulo.
  /// </remarks>
  INode? Parent { get; set; }

  /// <summary>
  /// Obtém ou define o nome do nodo do arquivo.
  /// </summary>
  string Name { get; set; }

  /// <summary>
  /// Obtém ou define um valor que indica se o nodo do arquivo está habilitado.
  /// Se estiver desabilitado, o conteúdo do nodo não será executado.
  /// </summary>
  bool Enabled { get; set; }

  /// <summary>
  /// Obtém o caminho virtual do nodo dentro da hierarquia de nodos.
  /// </summary>
  string Path { get; }

  /// <summary>
  /// Obtém ou define uma lista de falhas ocorridas durante a criação ou
  /// execução do nodo.
  /// </summary>
  List<Fault> Faults { get; set; }

  /// <summary>
  /// Obtém uma lista de filhos do nodo.
  /// </summary>
  /// <returns>
  /// Uma lista de nodos filhos.
  /// </returns>
  IEnumerable<INode> Children();

  /// <summary>
  /// Aceita um visitante na árvore de nodos e repassa o visitante para seus
  /// filhos.
  /// </summary>
  /// <param name="visitor">
  /// O visitante a ser aceito.
  /// </param>
  /// <remarks>
  /// Este método deve ser implementado para permitir que a árvore de nodos seja
  /// visitada por um visitante.
  /// </remarks>
  void Accept(IVisitor visitor);

  /// <summary>
  /// Aceita um visitante na árvore de nodos e repassa o visitante para seus
  /// filhos.
  /// </summary>
  /// <param name="visitor">
  /// O visitante a ser aceito.
  /// </param>
  /// <returns>
  /// Uma tarefa que representa a operação assíncrona de aceitar o visitante.
  /// </returns>
  /// <remarks>
  /// Este método deve ser implementado para permitir que a árvore de nodos seja
  /// visitada por um visitante de forma assíncrona.
  /// </remarks>
  Task AcceptAsync(IAsyncVisitor visitor);
}