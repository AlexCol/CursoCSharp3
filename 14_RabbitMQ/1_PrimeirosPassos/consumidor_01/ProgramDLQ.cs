
//! exemplo de lógica para o  caso de falha ao receber uma mensagem
//! 

/*
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

const string NomeDaFilaPrincipal = "fila_01";
const string NomeDaFilaDLQ = "fila_01_dlq";
const int LimiteDeTentativas = 3; // Limite de tentativas antes de mover para DLQ

// Configuração do servidor RabbitMQ
var server = new ConnectionFactory() {
  HostName = "localhost",    // Endereço do servidor RabbitMQ
  Port = 5672,               // Porta padrão do RabbitMQ
  UserName = "usuario",      // Nome de usuário para autenticação
  Password = "Senha@123"     // Senha para autenticação
};

// Estabelece a conexão com o servidor RabbitMQ
using var conexao = server.CreateConnection();
using var canal = conexao.CreateModel();

// Declara a fila principal
canal.QueueDeclare(queue: NomeDaFilaPrincipal, durable: true, exclusive: false, autoDelete: false, arguments: null);

// Declara a Dead Letter Queue (DLQ)
canal.QueueDeclare(queue: NomeDaFilaDLQ, durable: true, exclusive: false, autoDelete: false, arguments: null);

// Criação do consumidor para a fila principal
var consumidor = new EventingBasicConsumer(canal);
consumidor.Received += (sender, evento) => {
  // Extrai o corpo da mensagem e converte para string
  var corpoMensagem = evento.Body.ToArray();
  var mensagem = Encoding.UTF8.GetString(corpoMensagem);

  // Verifica o número de tentativas anteriores
  int tentativas = evento.BasicProperties.Headers != null && evento.BasicProperties.Headers.ContainsKey("tentativas")
                   ? Convert.ToInt32(evento.BasicProperties.Headers["tentativas"])
                   : 0;

  Console.WriteLine("[x] Tentativa {0} para a mensagem: {1}", tentativas + 1, mensagem);

  try {
    // Simulando uma falha de processamento para teste
    throw new Exception("Erro ao processar a mensagem");

    // Confirma a mensagem se o processamento for bem-sucedido
    canal.BasicAck(evento.DeliveryTag, multiple: false);
    Console.WriteLine("[x] Mensagem confirmada");
  } catch (Exception ex) {
    // Incrementa o número de tentativas
    tentativas++;
    Console.WriteLine("Erro ao processar a mensagem: {0}", ex.Message);

    if (tentativas >= LimiteDeTentativas) {
      // Envia a mensagem para a DLQ após exceder o limite de tentativas
      var propriedadesDLQ = canal.CreateBasicProperties();
      propriedadesDLQ.Headers = evento.BasicProperties.Headers ?? new Dictionary<string, object>();
      propriedadesDLQ.Headers["tentativas"] = tentativas;

      // Publica a mensagem na DLQ
      canal.BasicPublish(exchange: "", routingKey: NomeDaFilaDLQ, basicProperties: propriedadesDLQ, body: corpoMensagem);
      Console.WriteLine("[x] Mensagem movida para a DLQ: {0}", mensagem);

      // Confirma a mensagem para removê-la da fila principal
      canal.BasicAck(evento.DeliveryTag, multiple: false);
    } else {
      // Cria novas propriedades com o contador de tentativas atualizado
      var propriedades = canal.CreateBasicProperties();
      propriedades.Headers = evento.BasicProperties.Headers ?? new Dictionary<string, object>();
      propriedades.Headers["tentativas"] = tentativas;

      // Reenvia a mensagem para a fila principal
      canal.BasicPublish(exchange: "", routingKey: NomeDaFilaPrincipal, basicProperties: propriedades, body: corpoMensagem);
      Console.WriteLine("[x] Mensagem reencaminhada para tentativa {0}", tentativas);

      // Confirma a mensagem para removê-la da fila atual e reencaminhá-la
      canal.BasicAck(evento.DeliveryTag, multiple: false);
    }
  }
};

// Inicia o consumo de mensagens na fila principal
canal.BasicConsume(queue: NomeDaFilaPrincipal, autoAck: false, consumer: consumidor);

Console.WriteLine("Aguardando mensagens. Pressione qualquer tecla para sair");
Console.ReadKey();

// */