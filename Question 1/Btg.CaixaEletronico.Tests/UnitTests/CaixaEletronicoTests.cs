using Btg.CaixaEletronico.Constants;
using FluentAssertions;
using System;
using Xunit;

namespace Btg.CaixaEletronico.Tests
{
    public class CaixaEletronicoTests
    {
        [Fact]
        public void AdicionarNotas_DeveLancarExceção_QuandoOsValoresDasNotasForemInvalidos()
        {
            //Arrange
            var caixa = new CaixaEletronico.Entities.CaixaEletronico();


            //Act & Assert
            caixa.Invoking(c => c.AdicionarNotas(200, 10)).Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Valor da nota não suportado pelo caixa eletrônico.");
        }

        [Fact]
        public void Sacar_DeveLancarExceção_QuandoValorDeSaqueNaoContemplarNotasAceitas()
        {
            //Arrange
            var caixa = new CaixaEletronico.Entities.CaixaEletronico();


            //Act & Assert
            caixa.Invoking(c => c.Sacar(211)).Should().ThrowExactly<InvalidOperationException>()
                .WithMessage($"O caixa eletrônico apenas possui as seguintes notas: {string.Join(",", caixa.NotasAceitas)}");
        }

        [Fact]
        public void Sacar_DeveLancarExceção_QuandoValorDeSaqueForMaiorDoQueOValorTotalDoCaixa()
        {
            //Arrange
            var valorSaque = 600;
            var caixa = new CaixaEletronico.Entities.CaixaEletronico();
            caixa.AdicionarNotas(Notas.CEM, 5);


            //Act & Assert
            caixa.Invoking(c => c.Sacar(valorSaque)).Should().ThrowExactly<InvalidOperationException>()
                .WithMessage($"O caixa eletrônico não possui o montante total de {valorSaque:C} para ser sacado.");
        }

        [Theory]
        [InlineData(30, 2)]
        [InlineData(60, 2)]
        [InlineData(80, 3)]
        public void Sacar_DeveRetornarQuantidadeMinimaDeNotasUtilizadas_QuandoValorDeSaqueForValido(int valorSaque, int qtdNotas)
        {
            //Arrange
            var caixa = new CaixaEletronico.Entities.CaixaEletronico();
            caixa.AdicionarNotas(Notas.CEM, 0);
            caixa.AdicionarNotas(Notas.CINQUENTA, 1);
            caixa.AdicionarNotas(Notas.VINTE, 1);
            caixa.AdicionarNotas(Notas.DEZ, 1);


            //Act
            var notas = caixa.Sacar(valorSaque);

            //Assert
            notas.Count.Should().Be(qtdNotas);
        }


        [Fact]
        public void Sacar_DeveRetornarQuantidadeMinimaDeNotas_QuandoValorDeSaqueForValido()
        {
            //Arrange
            var caixa = new CaixaEletronico.Entities.CaixaEletronico();
            caixa.AdicionarNotas(Notas.CEM, 1);
            caixa.AdicionarNotas(Notas.CINQUENTA, 1);
            caixa.AdicionarNotas(Notas.VINTE, 1);
            caixa.AdicionarNotas(Notas.DEZ, 1);
            var saqueTotal = Notas.CEM + Notas.CINQUENTA + Notas.VINTE + Notas.DEZ;

            //Act
            var notas = caixa.Sacar(saqueTotal);

            //Assert
            notas[Notas.CEM].Should().Be(1);
            notas[Notas.CINQUENTA].Should().Be(1);
            notas[Notas.VINTE].Should().Be(1);
            notas[Notas.DEZ].Should().Be(1);
        }
    }
}