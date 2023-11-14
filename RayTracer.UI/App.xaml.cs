using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RayTracer.Interfaces;
using RayTracer.UI.ViewModels;
using RayTracer.Utilities;

namespace RayTracer.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices(RegisterDependencies)
            .Build();
    }

    private static void RegisterDependencies(HostBuilderContext hostContext, IServiceCollection services)
    {
        services
            .AddSingleton<MainView>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<IPpmWriter, PpmWriter>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();
        
        var mainWindow = AppHost.Services.GetRequiredService<MainView>();
        mainWindow.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}