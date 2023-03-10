using ScriptPack.Domain;

namespace ScriptPack.Algorithms;

/// <summary>
/// Aplica as regras de configuração para seleção da conexão apropriada para
/// execução de scripts de migração de dados.
/// </summary>
public class ConnectionSelector
{
  /// <summary>
  /// Seleciona as conexões apropriadas para o pacote especificado com base nas
  /// regras de configuração.
  /// </summary>
  /// <param name="package">
  /// O pacote para o qual as conexões devem ser selecionadas.
  /// </param>
  /// <returns>
  /// Um array de objetos ConnectionNode que representam as conexões
  /// selecionadas.
  /// </returns>
  public ConnectionNode[] SelectConnections(PackageNode package)
  {
    var catalog = package.Ancestor<CatalogNode>()
        ?? throw new InvalidOperationException(
            "O nodo de pacote não está vinculado a um catálogo: "
                + package.Name);

    var connections = catalog.Descendants<ConnectionNode>().ToArray();

    //
    //  Considerando a configuração de conexão alvo do próprio pacote.
    //

    var selectedConnections = (
        from connection in connections
        where package.TargetConnections.Contains(connection.Name)
        select connection
    ).ToArray();

    if (selectedConnections.Length > 0)
      return selectedConnections;

    //
    //  Escolhendo uma conexão padrão pelo critério:
    //
    //  1.  A primeira conexão padrão e não-vinculada (BoundTo=null)
    //  2.  Se não, a primeira conexão padrão e vinculada (BoundTo!=null)
    //  3.  Se não, a primeira conexão não-padrão e não-vinculada (BoundTo=null)
    //  4.  Se não, a primeira conexão não-padrão e vinculada (BoundTo!=null)
    //

    var selectedConnection = (
        from connection in connections
        orderby connection.Default descending
        orderby connection.BoundTo == null descending
        select connection
    ).FirstOrDefault();

    return selectedConnection != null
        ? new[] { selectedConnection }
        : new ConnectionNode[0];
  }
}
