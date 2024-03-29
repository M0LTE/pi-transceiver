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
builder.Services.AddSingleton<IGpioService, NativeGpioService>();
builder.Services.AddTransient<PttService>();
builder.Services.AddTransient<IFlowgraphControlService, GnuRadioFlowgraphControlService>();
builder.Services.AddHostedService<StartupService>();
builder.Services.AddSingleton<IAdcChannelReaderService, Ads1115NativeChannelReaderService>();
builder.Services.AddTransient<PlatformInfoProvider>();
builder.Services.AddSingleton<IPiUpsHatService, PiUpsHatService>();
builder.Services.AddSingleton<II2cDacService, I2cDacService>();
builder.Services.AddSingleton<IFanService, FanService>();



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
