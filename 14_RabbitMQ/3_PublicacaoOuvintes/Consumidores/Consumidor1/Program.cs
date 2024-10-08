using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Configura as credenciais e o endereço do servidor RabbitMQ
var server = new ConnectionFactory() {
  HostName = "localhost",  // Nome do host do servidor RabbitMQ
  Port = 5672,             // Porta padrão do RabbitMQ
  UserName = "usuario",    // Nome de usuário para autenticação
  Password = "Senha@123"   // Senha para autenticação
};

// Cria uma conexão com o servidor RabbitMQ
using var conexao = server.CreateConnection();

// Cria um canal de comunicação através da conexão aberta
using var canal = conexao.CreateModel();

// Declara um "exchange" do tipo "fanout" para broadcast de mensagens
canal.ExchangeDeclare(
    exchange: "logs",
    type: ExchangeType.Fanout
);

// Declara uma fila exclusiva e anônima para o consumidor e obtém o nome dela
var nomeFila = canal.QueueDeclare().QueueName;

// Vincula a fila ao exchange "logs" para receber todas as mensagens de broadcast
canal.QueueBind(queue: nomeFila, exchange: "logs", routingKey: "");

// Exibe uma mensagem no console para indicar que está aguardando mensagens
Console.WriteLine(" [*] Aguardando mensagens.");

// Cria um consumidor para receber mensagens do RabbitMQ
var consumidor = new EventingBasicConsumer(canal);

// Evento que é executado quando uma nova mensagem é recebida
consumidor.Received += (sender, evento) => {
  var corpo = evento.Body.ToArray();  // Converte o corpo da mensagem para um array de bytes
  var mensagem = Encoding.UTF8.GetString(corpo);  // Decodifica a mensagem em texto

  // Exibe a mensagem recebida no console
  Console.WriteLine(" [x] Recebido {0}", mensagem);
};

// Inicia o consumo de mensagens na fila declarada, com reconhecimento automático
canal.BasicConsume(queue: nomeFila, autoAck: true, consumer: consumidor);

// Aguarda o usuário pressionar Enter para finalizar o programa
Console.WriteLine(" Pressione [enter] para finalizar.");
Console.ReadLine();
