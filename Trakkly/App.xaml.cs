namespace Trakkly;

public partial class App : Application
{
    private readonly ProjectManager _projectManager;

   
    public App(ProjectManager projectManager)
    {
        InitializeComponent();
        _projectManager = projectManager;
    }

    protected override void OnSleep()
    {
        // Save in background without awaiting to avoid crashes
        Task.Run(async () => await _projectManager.SaveAsync());
    }


    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage()) { Title = "Trakkly" };
    }
}