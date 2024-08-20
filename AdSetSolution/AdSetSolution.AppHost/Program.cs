var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AdSetSolution_WebApplication>("adsetsolution-webapplication");

builder.Build().Run();
