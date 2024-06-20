﻿using JornadaMilhas.Dados;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao
{
    [Collection(nameof(ContextoCollection))]
    public class OfertaViagemDalRecuperarPorId
    {
        private readonly JornadaMilhasContext _context;

        public OfertaViagemDalRecuperarPorId(ITestOutputHelper output, ContextoFixture fixture)
        {
            _context = fixture.Context;
            output.WriteLine(_context.GetHashCode().ToString());
        }

        [Fact]
        public void RetornaNuloQuandoIdInexistente()
        {
            //arrange
            var dal = new OfertaViagemDAL(_context);

            //act
            var ofertaRecuperada = dal.RecuperarPorId(-2);

            //assert
            Assert.Null(ofertaRecuperada);
        }
    }
}
