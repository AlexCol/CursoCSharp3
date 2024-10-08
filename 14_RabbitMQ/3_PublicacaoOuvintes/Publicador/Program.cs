using System;
using System.Text;
using RabbitMQ.Client;

var servidor = new ConnectionFactory() {
  HostName = "localhost",
  Port = 5672,
  UserName = "usuario",
  Password = "Senha@123"
};

var conexao = servidor.CreateConnection();

using var canal = conexao.CreateModel();
canal.ExchangeDeclare(
    exchange: "logs",
    type: ExchangeType.Fanout
);

Console.WriteLine("Digite uma mensagem (ou '!finalizar' para encerrar):");
while (true) {
  string? mensagem = Console.ReadLine();
  if (mensagem == null || mensagem == "!finalizar") break;

  var corpoMensagem = Encoding.UTF8.GetBytes(mensagem!);

  canal.BasicPublish(
      exchange: "logs",
      routingKey: "",
      basicProperties: null,
      body: corpoMensagem
  );

  Console.WriteLine("[x] Mensagem enviada: {0}", mensagem);
}

Console.WriteLine("Pressione qualquer tecla para sair");
Console.ReadKey();