using System;
using System.Text;
using RabbitMQ.Client;

// Configura a fábrica de conexões do RabbitMQ, definindo as informações do servidor
var servidor = new ConnectionFactory() {
  HostName = "localhost", // Define o hostname do servidor RabbitMQ
  Port = 5672, // Define a porta de conexão (padrão é 5672)
  UserName = "usuario", // Define o nome de usuário para autenticação
  Password = "Senha@123" // Define a senha para autenticação
};

// Cria uma conexão com o servidor RabbitMQ
var conexao = servidor.CreateConnection();

// Cria um canal de comunicação para enviar mensagens
using var canal = conexao.CreateModel();

// Declara uma exchange do tipo 'fanout' chamada 'logs'
// 'fanout' faz com que todas as filas conectadas à exchange recebam as mensagens
canal.ExchangeDeclare(
    exchange: "logs",
    type: ExchangeType.Fanout
);

Console.WriteLine("Digite uma mensagem (ou '!finalizar' para encerrar):");

// Loop para ler e enviar mensagens até que o usuário digite '!finalizar'
while (true) {
  // Lê a mensagem digitada pelo usuário
  string? mensagem = Console.ReadLine();

  // Se a mensagem for nula ou '!finalizar', interrompe o loop
  if (mensagem == null || mensagem == "!finalizar") break;

  // Codifica a mensagem como um array de bytes para envio
  var corpoMensagem = Encoding.UTF8.GetBytes(mensagem!);

  // Publica a mensagem na exchange 'logs' para que todas as filas conectadas a recebam
  canal.BasicPublish(
      exchange: "logs", // Nome da exchange
      routingKey: "", // Chave de roteamento vazia, pois 'fanout' ignora essa chave
      basicProperties: null, // Propriedades da mensagem (nenhuma especificada)
      body: corpoMensagem // Corpo da mensagem em bytes
  );

  Console.WriteLine("[x] Mensagem enviada: {0}", mensagem);
}

// Informa que o programa está pronto para finalizar
Console.WriteLine("Pressione qualquer tecla para sair");
Console.ReadKey();
