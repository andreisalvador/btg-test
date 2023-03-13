using Btg.CaixaEletronico.Constants;

namespace Btg.CaixaEletronico.Entities
{
    public class CaixaEletronico
    {
        private readonly int[] _notasAceitasPeloCaixaEletronico;
        private readonly Dictionary<int, int> _notasDoCaixaEletronico;
        private int _valorTotalCaixaEletronico;
        public int[] NotasAceitas => _notasAceitasPeloCaixaEletronico;

        public CaixaEletronico()
        {
            _valorTotalCaixaEletronico = 0;
            _notasAceitasPeloCaixaEletronico = new[] { Notas.CEM, Notas.CINQUENTA, Notas.VINTE, Notas.DEZ };
            _notasDoCaixaEletronico = new Dictionary<int, int>
            {
                { Notas.CEM, 0 },
                { Notas.CINQUENTA, 0 },
                { Notas.VINTE, 0 },
                { Notas.DEZ, 0 },
            };
        }

        public void AdicionarNotas(int nota, int quantidade)
        {
            if (!_notasAceitasPeloCaixaEletronico.Contains(nota))
                throw new InvalidOperationException("Valor da nota não suportado pelo caixa eletrônico.");

            _notasDoCaixaEletronico[nota] += quantidade;
            _valorTotalCaixaEletronico += nota * quantidade;
        }

        private void Sacar(int nota, int quantidade)
        {
            if (_notasDoCaixaEletronico[nota] == 0)
                throw new InvalidOperationException($"Não há mais notas de {nota:C} disponíveis no caixa eletrônico.");

            _notasDoCaixaEletronico[nota] -= quantidade;
            _valorTotalCaixaEletronico -= nota * quantidade;
        }

        public Dictionary<int, int> Sacar(int valor)
        {
            bool possivelSacarComNotasDisponiveis = valor % 10 == 0;

            if (!possivelSacarComNotasDisponiveis)
                throw new InvalidOperationException($"O caixa eletrônico apenas possui as seguintes notas: {string.Join(",", _notasAceitasPeloCaixaEletronico)}");

            if (valor > _valorTotalCaixaEletronico)
                throw new InvalidOperationException($"O caixa eletrônico não possui o montante total de {valor:C} para ser sacado.");

            var notas = BuscarNotas(valor);

            foreach (var item in notas)
                Sacar(item.Key, item.Value);

            return notas;
        }

        private Dictionary<int, int> BuscarNotas(int valor)
        {
            var notasDisponiveis = _notasDoCaixaEletronico.Where(nota => nota.Value > 0).Select(nota => nota.Key).ToList();
            var notas = new Dictionary<int, int>();

            int valorRestante = valor;
            int indexNotaInicial = 0;

            while (valorRestante != 0)
            {
                if(notasDisponiveis.Count == indexNotaInicial)
                    throw new InvalidOperationException($"O caixa não possui notas o suficiente para o saque do valor {valor:C}");

                valorRestante = DescobrirMenorQuantidadeNotasPossivelPorNota(valorRestante, notasDisponiveis[indexNotaInicial++], notas);
            }

            return notas;
        }

        private int DescobrirMenorQuantidadeNotasPossivelPorNota(int valor, int nota, Dictionary<int, int> notas)
        {
            int quantidadeNotasNecessariasParaValor = valor / nota;

            if (quantidadeNotasNecessariasParaValor > 0)
            {
                var quantidadeNotas = _notasDoCaixaEletronico[nota] < quantidadeNotasNecessariasParaValor ? _notasDoCaixaEletronico[nota] : quantidadeNotasNecessariasParaValor;

                notas.Add(nota, quantidadeNotas);

                var valorComNotas = quantidadeNotas * nota;

                if (valorComNotas < valor)
                    return valor - valorComNotas;
                else
                    return 0;
            }

            return valor;
        }

        public override string ToString()
            => $"Notas de 100: {_notasDoCaixaEletronico[Notas.CEM]} | Notas de 50: {_notasDoCaixaEletronico[Notas.CINQUENTA]} | Notas de 20: {_notasDoCaixaEletronico[Notas.VINTE]} | Notas de 10: {_notasDoCaixaEletronico[Notas.DEZ]}";

    }
}
