using System.ComponentModel.DataAnnotations;
using ClientApi.Entities;
using ClientApi.Services;
using Microsoft.Identity.Web;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web.Resource;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";

// Get all clients
app.MapGet("/api/clients", (ClientService clientService) =>
    {
        return Results.Ok(clientService.GetClients());
    })
    .WithName("GetClients");

// Get a client by ID
app.MapGet("/api/clients/{id:int}", (int id, ClientService clientService) =>
    {
        var client = clientService.GetClientById(id);

        return client == null ?
            Results.NotFound($"Client with ID {id} not found.") :
            Results.Ok(client);
    })
    .WithName("GetClientById");

// Create a new client
app.MapPost("/api/clients", (Client client, ClientService clientService) =>
    {
        if (!Validator.TryValidateObject(client, new ValidationContext(client), null, true))
        {
            return Results.BadRequest("Client data is incomplete or invalid.");
        }

        clientService.AddClient(client);
        return Results.Created($"/api/clients/{client.Id}", client);
    })
    .WithName("CreateClient");

// Update an existing client
app.MapPut("/api/clients/{id:int}", (int id, Client client, ClientService clientService) =>
    {
        var existingClient = clientService.GetClientById(id);
        if (existingClient == null)
        {
            return Results.NotFound($"Client with ID {id} not found.");
        }

        client.Id = id;
        clientService.UpdateClient(client);
        return Results.Ok(client);
    })
    .WithName("UpdateClient");

// Delete a client by ID
app.MapDelete("/api/clients/{id:int}", (int id, ClientService clientService) =>
    {
        var existingClient = clientService.GetClientById(id);
        if (existingClient == null)
        {
            return Results.NotFound($"Client with ID {id} not found.");
        }

        clientService.DeleteClient(id);
        return Results.NoContent();
    })
    .WithName("DeleteClient");

app.Run();