using System;
using System.Text;
using RabbitMQ.Client;

// Criação da conexão com o servidor RabbitMQ
var server = new ConnectionFactory() {
  // Configurações do servidor RabbitMQ, incluindo hostname, porta e credenciais
  HostName = "localhost",      // Endereço do servidor (localhost para ambiente local)
  Port = 5672,                 // Porta padrão do RabbitMQ (5672)
  UserName = "usuario",        // Usuário de autenticação
  Password = "Senha@123"       // Senha de autenticação
};

// Estabelece a conexão com o servidor RabbitMQ
using var conexao = server.CreateConnection();

// Criação de um canal de comunicação com o RabbitMQ
using var canal = conexao.CreateModel();

// Declara a fila chamada "fila_01" para garantir que existe antes de enviar mensagens
canal.QueueDeclare(
    queue: "fila_01",            // Nome da fila
    durable: true,              // 'true' indica que a fila persiste após reiniciar o servidor
    exclusive: false,            // 'false' permite que outros canais acessem a fila
    autoDelete: false,           // 'false' indica que a fila não será apagada quando não estiver em uso
    arguments: null              // Argumentos adicionais (null, neste caso)
);

// Define a mensagem a ser enviada
string mensagem = "Olá, mundo novo!";

// Converte a string da mensagem para um array de bytes para envio pelo RabbitMQ
var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);

// Publica a mensagem no RabbitMQ na fila especificada
canal.BasicPublish(
    exchange: "",                // Exchange vazio usa o "default exchange"
    routingKey: "fila_01",       // Nome da fila como chave de roteamento
    basicProperties: null,       // Propriedades adicionais (null neste caso)
    body: corpoMensagem          // Corpo da mensagem em bytes
);

// Log de confirmação de envio
Console.WriteLine("[x] Mensagem enviada: {0}", mensagem);

// Aguardar o usuário pressionar uma tecla para encerrar
Console.WriteLine("Pressione qualquer tecla para sair");
Console.ReadKey();
