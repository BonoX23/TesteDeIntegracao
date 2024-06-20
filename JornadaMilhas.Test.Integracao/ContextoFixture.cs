using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace JornadaMilhas.Test.Integracao;
public class ContextoFixture : IAsyncLifetime
{
    public JornadaMilhasContext Context { get; private set; }
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    //Metodo que inicializa o banco de Dados
    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
            .UseSqlServer(_msSqlContainer.GetConnectionString())
            .Options;

        Context = new JornadaMilhasContext(options);
        Context.Database.Migrate();
    }
    //Metodo utilizado para iniciar a base de dados que serão utilizados
    public void CriaDadosFake()
    {
        Periodo periodo = new PeriodoDataBuilder().Build();

        var rota = new Rota("Curitiba", "São Paulo");

        var fakerOferta = new Faker<OfertaViagem>()
            .CustomInstantiator(f => new OfertaViagem(
                rota,
                new PeriodoDataBuilder().Build(),
                100 * f.Random.Int(1, 100))
            )
            .RuleFor(o => o.Desconto, f => 40)
            .RuleFor(o => o.Ativa, f => true);

        var lista = fakerOferta.Generate(200);
        Context.OfertasViagem.AddRange(lista);
        Context.SaveChanges();

    }
    //Limpa os dados do banco para realizar um novo teste
    public async Task LimpaDadosDoBanco()
    {
        /* Uma das soluções em comentarios abaixo, mas pode ser muito custoso fazer desta formar            
              Context.OfertasViagem.RemoveRange(Context.OfertasViagem);
              Context.Rotas.RemoveRange(Context.Rotas);
              await Context.SaveChangesAsync();*/

        //Abaixo usamos funções SQL para deletar as tabelas de forma mais fácil
        Context.Database.ExecuteSqlRaw("DELETE FROM OfertasViagem");
        Context.Database.ExecuteSqlRaw("DELETE FROM Rotas");
    }

    //Metodo que encerra o banco de Dados
    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}
