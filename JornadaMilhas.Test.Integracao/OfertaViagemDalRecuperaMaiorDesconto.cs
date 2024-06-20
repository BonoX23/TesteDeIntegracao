using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test.Integracao
{
    [Collection(nameof(ContextoCollection))]
    public class OfertaViagemDalRecuperaMaiorDesconto : IDisposable //TearDown é o IDisposable
    {
        private readonly JornadaMilhasContext _context;
        private readonly ContextoFixture _fixture;

        public OfertaViagemDalRecuperaMaiorDesconto(ContextoFixture fixture)
        {
            _context = fixture.Context;
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.LimpaDadosDoBanco();
        }

        [Fact]
        // destino = são paulo, desconto = 40, preco = 80
        public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
        {
            //arrange
            var rota = new Rota("Curitiba", "São Paulo");
            Periodo periodo = new PeriodoDataBuilder() { DataInicial = new DateTime(2024, 5, 20) }.Build();
            _fixture.CriaDadosFake();

            var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
            {
                Desconto = 40,
                Ativa = true
            };


            var dal = new OfertaViagemDAL(_context);
            dal.Adicionar(ofertaEscolhida);


            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
            var precoEsperado = 40;

            //act
            var oferta = dal.RecuperaMaiorDesconto(filtro);

            //assert
            Assert.NotNull(oferta);
            Assert.Equal(precoEsperado, oferta.Preco, 0.0001);

        }


        //para um teste não atrapalhar o outro deve-se usar
        [Fact]
        public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto60()
        {
            //arrange
            var rota = new Rota("Curitiba", "São Paulo");
            Periodo periodo = new PeriodoDataBuilder() { DataInicial = new DateTime(2024, 5, 20) }.Build();
            _fixture.CriaDadosFake();

            var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
            {
                Desconto = 60,
                Ativa = true
            };


            var dal = new OfertaViagemDAL(_context);
            dal.Adicionar(ofertaEscolhida);


            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
            var precoEsperado = 20;

            //act
            var oferta = dal.RecuperaMaiorDesconto(filtro);

            //assert
            Assert.NotNull(oferta);
            Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
        }
    }
}
