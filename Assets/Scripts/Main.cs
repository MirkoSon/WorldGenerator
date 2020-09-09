using UnityEngine;

/// <summary>
/// Monobehaviour class that starts and ends each game. Works in bundle with the <see cref="IContext"/>
/// to call the Binding and the Unbinding of dependencies, so that no listener or dependency would persist accidentally between games.
/// </summary>
[RequireComponent(typeof(IContext))]
public class Main : MonoBehaviour, IMain
{
    public RulesData rules;
    public GameObject tilesLibrary;
    public Transform tileGroup;

    private IContext _context;
    private static RulesData _rules;

    protected IContext context => _context ?? (_context = GetComponent<IContext>());
    protected IDependencyContainer dependencyContainer => context.dependencyContainer;

    public static RulesData Rules
    {
        get
        {
            return _rules;
        }
        set //Needs to be public set for the GeneratorTester to work
        {
            _rules = value;
        }
    }

    void Start()
    {
        Init();
        StartGame();
    }

    protected void Init()
    {
        _rules = rules;
        dependencyContainer.Bind<Main>(this);
        context.Bind();
    }

    public void StartGame()
    {
        dependencyContainer.Resolve<TilesDatabase>().Initialize(tilesLibrary);
        dependencyContainer.Resolve<Generator>().Initialize(dependencyContainer);
        dependencyContainer.Resolve<UIManager>().Initialize(dependencyContainer);
    }

    public void EndGame()
    {
        context.UnBind();
    }

    private void OnApplicationQuit()
    {
        EndGame();
    }
}