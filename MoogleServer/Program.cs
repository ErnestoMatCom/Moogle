using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using loader;

namespace MoogleServer;

public class Program{
static void Main(String[] args){

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


Loader.mainLoader();//cargara los documentos


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

}

}
