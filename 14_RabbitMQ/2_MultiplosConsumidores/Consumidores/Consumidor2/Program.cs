using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Configura a fábrica de conexão com RabbitMQ, especificando o servidor e credenciais
var servidor = new ConnectionFactory {
    HostName = "localhost", // Endereço do servidor RabbitMQ
    Port = 5672,            // Porta padrão do RabbitMQ
    UserName = "usuario",   // Nome de usuário para autenticação
    Password = "Senha@123"  // Senha de acesso ao servidor
};

// Cria a conexão usando as configurações definidas acima
var conexao = servidor.CreateConnection();

// Com a conexão ativa, abre um canal de comunicação com o servidor
using var canal = conexao.CreateModel();

// Declara a fila de mensagens (queue) no servidor, garantindo que ela exista
// - durable: a fila persiste entre reinicializações do servidor
// - exclusive: false permite que outros canais usem a mesma fila
// - autoDelete: false impede a exclusão automática da fila quando não estiver em uso
canal.QueueDeclare(
    queue: "fila_de_tarefas",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

// Define que o canal deve receber uma mensagem por vez para processar (limita a carga de processamento)
canal.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

Console.WriteLine(" [*] Aguardando mensagens...");

// Cria o consumidor de eventos que processará as mensagens recebidas da fila
var consumidor = new EventingBasicConsumer(canal);

// Define o evento de recebimento de mensagem para o consumidor
consumidor.Received += (sender, evento) => {
    // Extrai o corpo da mensagem e converte de byte array para string
    try {
        //throw new Exception("ops");
        var corpoMensagem = evento.Body.ToArray();
        var mensagem = Encoding.UTF8.GetString(corpoMensagem);

        // Exibe a mensagem recebida no console
        Console.WriteLine(" [x] Mensagem recebida: {0}", mensagem);

        // Confirma o processamento da mensagem para o RabbitMQ (envia um ACK)
        canal.BasicAck(evento.DeliveryTag, multiple: false);
    } catch (Exception ex) {
        Console.WriteLine("Erro ao processar a mensagem: {0}", ex.Message);
        canal.BasicNack(evento.DeliveryTag, multiple: false, requeue: true); //com erro, mando a mensagem de volta para a fila
    }
};

// Inicia o consumo de mensagens da fila, usando o consumidor configurado
// - autoAck: false indica que a confirmação de processamento será manual (via BasicAck)
canal.BasicConsume(
    queue: "fila_de_tarefas",
    autoAck: false,
    consumer: consumidor
);

// Mantém o console aberto até o usuário pressionar "Enter"
Console.WriteLine(" Pressione [enter] para sair.");
Console.ReadLine();
