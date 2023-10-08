using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] private MonsterManager monster;

    //[SerializeField] private int startingLevel = 0;

    private int currentLevel;
    private float currentTime = 0;
    public int candyGiven;

    [System.Serializable]
    public class LevelData
    {
        public int requiredTreats;
        public int maxCarriedTreats;
        public int maxTime; 
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
            return;
        }

        if (monster == null) Debug.Log("Add MonsterManager to Level Manager Script!");
        GameManager.Instance.SwitchState(GameState.GAME);
        string activeSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Active Scene Name: " + activeSceneName);

        if (activeSceneName.StartsWith("Level "))
        {
            int levelNumber;
            if (int.TryParse(activeSceneName.Substring("Level ".Length), out levelNumber))
            {
                currentLevel = levelNumber - 1;
                //Debug.Log("Current Level: " + currentLevel);
            }
        }
    }

    private void Start()
    {

    } 

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() != GameState.GAME) return;

        //candyGiven = monster.GetMonsterTreats();

        if (PlayerWinsLevel())
        {
            GameManager.Instance.SwitchState(GameState.WIN);
            return;
        }

        else
        {
            currentTime += Time.deltaTime;
        }
    }

    public void LoadNextLevel()
    {
        if ((currentLevel + 1) < levels.Length)
        {
            currentLevel++;
            LoadLevel(currentLevel);
            return;
        }
        else
        {
            SceneTransitionManager.Instance.LoadScene("WinScene");
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
        if (currentLevel >= 0 && currentLevel < levels.Length && candyGiven >= levels[currentLevel].requiredTreats)
        {
            return true;
        }
        return false;
    }

    private bool LevelCompleted()
    {
        if (currentTime >= GetMaxTime() || monster.GetMonsterTreats() >= levels[currentLevel].requiredTreats)
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

    public int GetCurrentLevel()
    {
        string activeSceneName = SceneManager.GetActiveScene().name; 
        if (activeSceneName.StartsWith("Level "))
        {
            int levelNumber;
            if (int.TryParse(activeSceneName.Substring("Level ".Length), out levelNumber))
            {
                currentLevel = levelNumber - 1;
                //Debug.Log("Current Level: " + currentLevel);
            }
        }
        return currentLevel; 
    }

    public int GetMaxCarriedTreats()
    {
        return levels[GetCurrentLevel()].maxCarriedTreats;   
    }

    public int GetMaxTime()
    {
        return levels[GetCurrentLevel()].maxTime;  
    }
}
