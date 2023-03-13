// See https://aka.ms/new-console-template for more information


using Btg.CaixaEletronico.Entities;

var caixaEletronico = new CaixaEletronico();
Console.WriteLine(caixaEletronico);

EncherCaixaEletronico(caixaEletronico);
Console.Clear();

Console.WriteLine(caixaEletronico);

Console.WriteLine("Informe o valor do saque: ");

if (int.TryParse(Console.ReadLine(), out int valorSaque))
{
    try
    {
        var notasRecevidas = caixaEletronico.Sacar(valorSaque);

        Console.WriteLine($"Você irá receber as seguintes notas: {Environment.NewLine}{string.Join(Environment.NewLine, notasRecevidas.Select(nota => $"{nota.Value}x {nota.Key:C} = {(nota.Key * nota.Value):C}"))}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}

void EncherCaixaEletronico(CaixaEletronico caixaEletronico)
{
    foreach (var nota in caixaEletronico.NotasAceitas)
        AdicionarNotaCaixa(caixaEletronico, nota);
}

void AdicionarNotaCaixa(CaixaEletronico caixaEletronico, int nota)
{
    Console.WriteLine($"Informe a quantidade de notas de {nota} do caixa: ");

    if (int.TryParse(Console.ReadLine(), out int quantidadeNotas))
        caixaEletronico.AdicionarNotas(nota, quantidadeNotas);
    else
    {
        Console.WriteLine("Quantidade inválida, tente novamente abaixo.");
        AdicionarNotaCaixa(caixaEletronico, nota);
    }

}