public class Level01Controller : AbstractLevelController
{
    PowerupSpawner _powerUpSpawner;
    EnemySpawner _enemySpawner;

    void Start()
    {

        _powerUpSpawner = FindAnyObjectByType<PowerupSpawner>();
        _powerUpSpawner.OnObjectCollected += OnPowerUpCollected;

        _enemySpawner = FindAnyObjectByType<EnemySpawner>();
        _enemySpawner.OnObjectDestroyed += OnEnemyDestroyed;

        _levelManager.RegisterController(this);

        LevelState = LevelState.ASSEMBLE;

        Invoke(nameof(Run), StartingTime);
    }

    void OnPowerUpCollected(int amount)
    {
        
    }

    void OnEnemyDestroyed(int amount)
    {
        if (amount == 5)
        {
            LevelState = LevelState.COMPLETE;
            Invoke(nameof(Disassemble), CompletionTime);
        }
    }
    void Run()
    {
        LevelState = LevelState.RUN;
    }

    void Disassemble()
    {
        LevelState = LevelState.DISASSEMBLE;
    }
}
