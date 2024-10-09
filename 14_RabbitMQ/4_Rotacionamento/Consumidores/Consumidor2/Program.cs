using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Configura a conexão com o servidor RabbitMQ
var servidor = new ConnectionFactory() {
    HostName = "localhost",        // Endereço do servidor RabbitMQ (neste caso, local)
    Port = 5672,                   // Porta padrão do RabbitMQ
    UserName = "usuario",          // Nome de usuário para autenticação
    Password = "Senha@123"         // Senha para autenticação
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

// Declara uma fila exclusiva com um nome gerado automaticamente pelo RabbitMQ
// para receber as mensagens direcionadas a este consumidor
//var nomeFila = canal.QueueDeclare().QueueName; //para criar uma fila com nome aleatório

var nomeFila = "fila_compras"; //para criar uma fila com nome específico
canal.QueueDeclare(
    queue: "fila_compras",  // Nome da fila, neste caso "fila_vendas"
    durable: true,         // Define a durabilidade da fila, ou seja, se ela sobreviverá a reinicializações do servidor
    exclusive: false,      // Define se a fila é exclusiva ao consumidor que a criou
    autoDelete: false,     // Define se a fila será excluída automaticamente quando todos os consumidores forem desconectados
    arguments: null        // Argumentos adicionais para configurações avançadas
);

// Liga a fila declarada à exchange "mensagem_empresa" com a chave de roteamento "Compras".
// Assim, a fila receberá apenas as mensagens enviadas com essa rota específica.
canal.QueueBind(
    queue: nomeFila,
    exchange: "mensagem_empresa",
    routingKey: "Compras"
);

Console.WriteLine("Consumidor 2 - Aguardando mensagens...");

// Cria um consumidor para processar mensagens da fila
var consumidor = new EventingBasicConsumer(canal);

// Define o que fazer ao receber uma mensagem
consumidor.Received += (sender, evento) => {
    // Obtém o conteúdo da mensagem em bytes
    var corpoMensagem = evento.Body.ToArray();
    // Converte o conteúdo da mensagem para uma string
    var mensagem = Encoding.UTF8.GetString(corpoMensagem);

    // Exibe a mensagem recebida no console
    Console.WriteLine("[x] Mensagem recebida: {0}", mensagem);
};

// Inicia o consumo de mensagens na fila especificada
// autoAck: true significa que o RabbitMQ marcará a mensagem como confirmada automaticamente após o consumo
canal.BasicConsume(queue: nomeFila, autoAck: true, consumer: consumidor);

Console.WriteLine("Pressione [enter] para finalizar.");
Console.ReadLine(); // Aguarda o usuário pressionar Enter para encerrar o programa
