var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.OpenRoutePlanner_ApiService>("apiservice");

builder.AddProject<Projects.OpenRoutePlanner_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
