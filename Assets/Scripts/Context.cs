using UnityEngine;

/// <summary>
/// Context is the core of the application, all the dependencies are injected here and made accessible through the <see cref="DependencyContainer"/>'s dictionary.
/// Uses Event Aggregation Pattern to have better control over each listener.
/// </summary>
public class Context : MonoBehaviour, IContext
{
    //// Helpers & Services
    protected ITilesDatabase _tilesDatabase;

    //// Monobehaviours
    protected Generator _generator;
    protected UIManager _uiManager;

    // Dependency Container for Injections
    protected IDependencyContainer _container;
    public IDependencyContainer dependencyContainer
    {
        get
        {
            if (_container == null)
            {
                _container = new DependencyContainer();
            }

            return _container;
        }
    }

    public void Bind()
    {
        InitializeServices();
        BindServices();
        RegisterEvents();
    }

    public void UnBind()
    {
        UnregisterEvents();
        UnbindServices();
    }

    public void InitializeServices()
    {
        _tilesDatabase = null ?? new TilesDatabase();        
        _generator = null ?? GetComponent<Generator>();
        _uiManager = null ?? GetComponent<UIManager>();
    }

    public void BindServices()
    {
        dependencyContainer.Bind<TilesDatabase>(_tilesDatabase);
        dependencyContainer.Bind<Generator>(_generator);
        dependencyContainer.Bind<UIManager>(_uiManager);
    }

    public void UnbindServices()
    {
        dependencyContainer.UnBind<TilesDatabase>();
        dependencyContainer.UnBind<Generator>();
        dependencyContainer.UnBind<UIManager>();
    }

    public void RegisterEvents()
    {
        dependencyContainer.Resolve<Generator>().OnWorldGenerated += OnWorldGenerated;
    }

    public void UnregisterEvents()
    {
        dependencyContainer.Resolve<Generator>().OnWorldGenerated -= OnWorldGenerated;
    }

    private void OnWorldGenerated()
    {
        dependencyContainer.Resolve<UIManager>().generatingLabel.SetActive(false);

        foreach (WanderBehaviour wanderer in FindObjectsOfType<WanderBehaviour>())
            wanderer.Initialize();
    }
}
