using System.Text.Json.Serialization;

namespace SPack.Domain;

/// <summary>
/// Catálogo de produtos e seus scripts.
/// </summary>
public class Catalog : IFileNode
{
  public Catalog()
  {
    this.Products = new(this);
    this.Connections = new(this);
    this.Faults = new(this);
  }

  /// <summary>
  /// Nodo pai.
  /// </summary>
  [JsonIgnore]
  public Repository? Parent { get; set; }

  [JsonIgnore]
  INode? INode.Parent { get => Parent; set => Parent = (Repository?)value; }

  /// <summary>
  /// Nome do script.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Caminho virtual do nodo dentro da árvore de nodos.
  /// </summary>
  public string Path => $"/{Name}";

  /// <summary>
  /// Nome do nó.
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// Indica se a seção está habilitada.
  /// Se estiver desabilitada, o conteúdo da seção não será executado.
  /// </summary>
  public bool Enabled { get; set; } = true;

  /// <summary>
  /// Caminho relativo do arquivo referente.
  /// </summary>
  [JsonIgnore]
  public string? FilePath { get; set; } = string.Empty;

  /// <summary>
  /// Bases de dados componentes do sistema.
  /// </summary>
  public NodeList<Connection> Connections { get; set; }

  /// <summary>
  /// Lista de produtos disponíveis no Catálogo.
  /// </summary>
  [JsonIgnore]
  public NodeList<Product> Products { get; set; }

  /// <summary>
  /// Falhas ocorridas durante a execução do catálogo.
  /// </summary>
  [JsonIgnore]
  public NodeList<Fault> Faults { get; set; }

  public IEnumerable<INode> GetChildren()
  {
    foreach (var item in Connections) yield return item;
    foreach (var item in Products) yield return item;
    foreach (var item in Faults) yield return item;
  }

  public void Accept(IVisitor visitor)
  {
    visitor.Visit(this);
    Connections.ForEach(item => item.Accept(visitor));
    Products.ForEach(item => item.Accept(visitor));
    Faults.ForEach(item => item.Accept(visitor));
  }

  public async Task AcceptAsync(IAsyncVisitor visitor)
  {
    await visitor.VisitAsync(this);
    await Task.WhenAll(Connections.Select(item => item.AcceptAsync(visitor)));
    await Task.WhenAll(Products.Select(item => item.AcceptAsync(visitor)));
    await Task.WhenAll(Faults.Select(item => item.AcceptAsync(visitor)));
  }

  public override string ToString() => $"{base.ToString()} {Path}";
}