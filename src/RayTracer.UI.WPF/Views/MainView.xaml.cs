using RayTracer.UI.ViewModels;

namespace RayTracer.UI.WPF;

public partial class MainView
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        
        DataContext = viewModel;
    }
}