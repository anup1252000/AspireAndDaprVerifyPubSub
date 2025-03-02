using Aspire.Hosting.Dapr;
using Aspire.Hosting.Redis; // Add this using directive

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cache"); // This line should work now
var stateStore = builder.AddDaprStateStore("statestore");

var serviceBus = builder.AddDaprPubSub("servicebus-pubsub",new DaprComponentOptions
{
    LocalPath = "component/servicebus-pubsub.Yaml"
});

//var sb1=builder.AddDaprPubSub("azure-servicebus-subscription", new DaprComponentOptions
//{
//    LocalPath = "component/azure-servicebus-subscription.yaml",


//});
var sb1 = builder.AddDaprPubSub("azure-servicebus-subscription", new DaprComponentOptions
{
    LocalPath = "component/azure-servicebus-subscription.yaml"
});

var appservice = builder.AddProject<Projects.AspireAndDaprVerify>("aspireanddaprverify")
    .WithExternalHttpEndpoints()
      .WithDaprSidecar(sidecar =>
        {
            sidecar.WithOptions(new DaprSidecarOptions
            {
                AppId = "aspireanddaprverify",
                AppPort = 5281,
                DaprHttpPort = 3502,
                DaprGrpcPort = 50001,
            });
        }).WithReference(redis).WaitFor(redis).WithReference(stateStore).WithReference(sb1);
builder.AddDapr(x =>
{
    x.EnableTelemetry = true;
});
builder.AddProject<Projects.AspireAndDaprClientVerify>("aspireanddaprclientverify")
    .WithExternalHttpEndpoints()
    .WithDaprSidecar(sidecar =>
    {
        sidecar.WithOptions(new DaprSidecarOptions
        {
            AppId = "aspireanddaprclientverify",
            AppPort = 5027,
            DaprHttpPort = 3501,
            DaprGrpcPort = 50002,
            
        });
    }).WithReference(appservice).WaitFor(appservice)
    .WithReference(serviceBus);

builder.Build().Run();
