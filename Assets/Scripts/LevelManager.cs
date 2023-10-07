using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private WorldClock timer;
    [SerializeField] private MonsterManager monster;
    //[SerializeField] private int startingLevel = 0;

    private int currentLevel = 0;
    private float currentTime = 0;
    private int candyGiven;

    [System.Serializable]
    public class LevelData
    {
        public int requiredTreats;
        public int maxCarriedTreats;
    }

    public LevelData[] levels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        if (timer == null) Debug.Log("Add WorldClock to Level Manager Script!");
        if (monster == null) Debug.Log("Add MonsterManager to Level Manager Script!");
        GameManager.Instance.SwitchState(GameState.GAME);  
        string activeSceneName = SceneManager.GetActiveScene().name;
        
        if (activeSceneName.StartsWith("Level "))
        {
            int levelNumber;
            if (int.TryParse(activeSceneName.Substring("Level ".Length), out levelNumber))
            {
                currentLevel = levelNumber - 1;
            }
        }

        LevelData currentLevelData = levels[currentLevel];
    }

    private void Update()
    {
        candyGiven = monster.GetMonsterTreats();

        if (PlayerWinsLevel())
        {
            currentLevel++;

            if (currentLevel < levels.Length)
            {
                LoadLevel(currentLevel);
                return;
            }
            else
            {
                Debug.Log("Game Completed!");
            }
        }
        if (LevelCompleted())
        {
            Debug.Log("GameOver!");
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    private void LoadLevel(int levelIndex)
    {
        levelIndex = Mathf.Clamp(levelIndex, 0, levels.Length - 1);
        currentLevel = levelIndex; // Update the current level
        Debug.Log("Loading Level " + (levelIndex + 1));
        SceneTransitionManager.Instance.LoadScene("Level " + (levelIndex + 1));
        LevelData currentLevelData = levels[currentLevel]; 
    }

    private bool PlayerWinsLevel()
    {
        if (candyGiven >= levels[currentLevel].requiredTreats)
        {
            return true;
        }
        return false;
    }

    private bool LevelCompleted()
    {
        if (currentTime >= timer.GetMaxTime() || monster.GetMonsterTreats() >= levels[currentLevel].requiredTreats)
        {
            return true;
        }
        return false;
    }

    public LevelData GetCurrentLevelData()
    {
        return levels[currentLevel];
    }

    public int GetCandyGiven()
    {
        return candyGiven;
    }
}
