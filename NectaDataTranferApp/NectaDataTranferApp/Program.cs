
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using NectaDataTranferApp.Components;
using NectaDataTranferApp.Controllers;
using NectaDataTranferApp.Services;
using NectaDataTransfer.Services;
using NectaDataTransfer.Services.Sifa;
using NectaDataTransfer.Shared.Interfaces;
using NectaDataTransfer.Shared.Interfaces.Sifa;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddFluentUIComponents();

builder.Services.AddScoped<SessionService>();
builder.Services.AddSingleton<IMysqlService, MysqlService>();
builder.Services.AddSingleton<ICsvService, CsvService>();
builder.Services.AddSingleton<ISqlService, SqlService>();
builder.Services.AddSingleton<ITransferOption, TransferOptionService>();
builder.Services.AddSingleton<ISifaTransferOption, SifaTransferOptionService>();
builder.Services.AddSingleton<IConnectionService, ConnectionService>();
builder.Services.AddSingleton<ISifaConnectionService, SifaConnectionService>();
builder.Services.AddSingleton<IOlevelService, OlevelService>();
builder.Services.AddSingleton<ISifaService, SifaService>();
builder.Services.AddScoped<ITooltipService, TooltipService>();
builder.Services.AddScoped<ToastServiceNavigate>();
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(NameListController).Assembly));
//builder.Services.AddSingleton<ICAService, CAService>();
//builder.Services.AddSingleton<INameService, NameService>();
builder.Services.AddServerSideBlazor().AddCircuitOptions(x => x.DetailedErrors = true);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
   
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapControllers();
//app.MapBlazorHubs();    
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(NectaDataTranferApp.Client._Imports).Assembly);

app.Run();
