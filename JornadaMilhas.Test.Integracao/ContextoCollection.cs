namespace JornadaMilhas.Test.Integracao;
[CollectionDefinition(nameof(ContextoCollection))]

//Se criarmos uma nova classe de teste ela irá criar um novo HashCode, para evitarmos isso criamos um ICollectionFixture
public class ContextoCollection : ICollectionFixture<ContextoFixture>
{
}