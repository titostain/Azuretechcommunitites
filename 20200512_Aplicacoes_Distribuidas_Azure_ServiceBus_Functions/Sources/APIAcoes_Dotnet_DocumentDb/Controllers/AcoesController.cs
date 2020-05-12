using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;
using APIAcoes.Models;

namespace APIAcoes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AcoesController : ControllerBase
    {
        private static readonly Contador _CONTADOR = new Contador();
        private readonly ILogger<AcoesController> _logger;

        public AcoesController(ILogger<AcoesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public object Get()
        {
            return new
            {
                NumeroMensagensEnviadas = _CONTADOR.ValorAtual
            };
        }

        [HttpPost]
        public object Post(
            [FromServices] IConfiguration config,
            Acao acao)
        {
            var conteudoAcao = JsonSerializer.Serialize(acao);
            _logger.LogInformation($"Dados: {conteudoAcao}");

            var body = Encoding.UTF8.GetBytes(conteudoAcao);

            string topic = config["AzureServiceBus:Topic"];
            var client = new TopicClient(
                config["AzureServiceBus:ConnectionString"], topic);
            client.SendAsync(new Message(body)).Wait();
            _logger.LogInformation(
                $"Azure Service Bus - Envio para o tópico {conteudoAcao} concluído");

            lock (_CONTADOR)
            {
                _CONTADOR.Incrementar();
            }

            return new
            {
                Resultado = "Mensagem enviada com sucesso!"
            };
        }
    }
}