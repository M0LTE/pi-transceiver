using rig_controller.Hubs;
using rig_controller.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddTransient<UiUpdaterService>();
builder.Services.AddSingleton<RigStateService>();
builder.Services.AddHostedService<FlexKnobHostedService>();
builder.Services.AddHostedService<UiUpdaterHostedService>();
builder.Services.AddSingleton<GpioService>();
builder.Services.AddTransient<PttService>();
builder.Services.AddTransient<IFlowgraphControlService, GnuRadioFlowgraphControlService>();
builder.Services.AddHostedService<StartupService>();
builder.Services.AddTransient<IAdcChannelReaderService, Ads1115NativeChannelReaderService>();
builder.Services.AddTransient<PlatformInfoProvider>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5155);
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<UiHub>("/uiHub");

app.Run();
