using System.Text;
using RabbitMQ.Client;

// Configura a conexão com o servidor RabbitMQ
var servidor = new ConnectionFactory() {
  HostName = "localhost",         // Endereço do servidor RabbitMQ (neste caso, local)
  Port = 5672,                    // Porta padrão do RabbitMQ
  UserName = "usuario",           // Nome de usuário para autenticação
  Password = "Senha@123"          // Senha para autenticação
};

// Cria uma conexão com o servidor RabbitMQ
var conexao = servidor.CreateConnection();

// Cria um canal de comunicação dentro da conexão
using var canal = conexao.CreateModel();

// Declara uma exchange chamada "mensagem_empresa" do tipo "direct",
// que permite o envio de mensagens para filas específicas com base
// em uma chave de roteamento (routing key).
canal.ExchangeDeclare(
    exchange: "mensagem_empresa",
    type: ExchangeType.Direct
);

Console.WriteLine("Digite uma mensagem (ou '!finalizar' para encerrar):");

// Loop que permite o envio de mensagens contínuas
while (true) {
  // Lê a mensagem do usuário
  string? mensagem = Console.ReadLine();

  // Verifica se o usuário deseja encerrar o programa
  if (mensagem == null || mensagem == "!finalizar") break;

  // Converte a mensagem para um array de bytes para o envio
  var corpoMensagem = Encoding.UTF8.GetBytes(mensagem!);

  Console.WriteLine("Digite uma rota:");
  // Lê a chave de roteamento fornecida pelo usuário
  string? rota = Console.ReadLine();

  // Publica a mensagem na exchange "mensagem_empresa" com a chave de roteamento especificada
  canal.BasicPublish(
      exchange: "mensagem_empresa", // Nome da exchange onde a mensagem será enviada
      routingKey: rota,             // Rota (chave de roteamento) que especifica para onde a mensagem deve ser direcionada
      basicProperties: null,        // Propriedades da mensagem (neste caso, null)
      body: corpoMensagem           // Conteúdo da mensagem em bytes
  );

  Console.WriteLine("[x] Mensagem enviada: {0}", mensagem);
}

Console.WriteLine("Pressione qualquer tecla para sair");
Console.ReadKey(); // Aguarda o usuário pressionar uma tecla para encerrar o programa
