using Markdig;
using TalentSchool.Infrastructure.Extensions;
var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton<MarkdownPipeline>(new MarkdownPipelineBuilder()
    .UseAdvancedExtensions()
    .UseSoftlineBreakAsHardlineBreak()
    .Build());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();

app.UseInfrastructure();
app.MapStaticAssets();


app.MapRazorPages();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    
    .WithStaticAssets();


app.Run();