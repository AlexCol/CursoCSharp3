// Importa namespaces necessários
using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Configuração do servidor RabbitMQ para a criação da conexão
var server = new ConnectionFactory() {
  HostName = "localhost",    // Endereço do servidor RabbitMQ
  Port = 5672,               // Porta padrão do RabbitMQ
  UserName = "usuario",      // Nome de usuário para autenticação
  Password = "Senha@123"     // Senha para autenticação
};

// Estabelece uma conexão com o servidor RabbitMQ
using var conexao = server.CreateConnection();

// Criação de um canal de comunicação para enviar e receber mensagens
using var canal = conexao.CreateModel();

// Declaração da fila chamada "fila_01" para garantir sua existência antes de consumi-la
canal.QueueDeclare(
    queue: "fila_01",            // Nome da fila
    durable: true,               // 'true' garante que a fila persiste após reiniciar o servidor
    exclusive: false,            // 'false' permite que outros canais também possam acessar a fila
    autoDelete: false,           // 'false' evita que a fila seja deletada quando o consumidor desconecta
    arguments: null              // Argumentos adicionais (nulos neste caso)
);

// Criação de um consumidor para receber mensagens da fila
var consumidor = new EventingBasicConsumer(canal);

// Define o que fazer ao receber uma mensagem (evento "Received")
consumidor.Received += (sender, evento) => {
  // Extrai o corpo da mensagem do evento como array de bytes
  var corpoMensagem = evento.Body.ToArray();

  // Converte o array de bytes para string
  var mensagem = Encoding.UTF8.GetString(corpoMensagem);

  // Exibe a mensagem recebida no console
  Console.WriteLine("[x] Mensagem recebida: {0}", mensagem);

  try {
    // Processa a mensagem
    // (Coloque aqui o código para processamento da mensagem)

    // Confirma a mensagem manualmente para removê-la da fila após o processamento bem-sucedido
    canal.BasicAck(evento.DeliveryTag, multiple: false); //!para exemplo de uso da DLQ (Dead Letter Queue), olhar ProgramDLQ.cs
    Console.WriteLine("[x] Mensagem confirmada");
  } catch (Exception ex) {
    // Loga o erro e opcionalmente trata a falha (exemplo: reprocessamento)
    Console.WriteLine("Erro ao processar a mensagem: {0}", ex.Message);

    // Rejeita a mensagem e a coloca de volta na fila para reprocessamento
    canal.BasicNack(evento.DeliveryTag, multiple: false, requeue: true);
  }
};

// Inicia o consumo de mensagens na fila "fila_01" com confirmação manual
canal.BasicConsume(
    queue: "fila_01",            // Nome da fila a ser consumida
    autoAck: false,              // 'false' indica que a confirmação será manual
    consumer: consumidor         // Consumidor responsável por processar as mensagens
);

// Informa ao usuário que o sistema está aguardando mensagens
Console.WriteLine("Aguardando mensagens. Pressione qualquer tecla para sair");
Console.ReadKey();
