using CreateExcelWorkerService;
using CreateExcelWorkerService.Services;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration Configuration = hostContext.Configuration;

        services.AddSingleton(sp => new ConnectionFactory()
        {
            HostName = Configuration.GetSection("RabbitMqConnection:Host").Get<string>(),
            Port = Configuration.GetSection("RabbitMqConnection:Port").Get<int>(),
            DispatchConsumersAsync = true
        });


        services.AddSingleton<RabbitMqClientServices>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
