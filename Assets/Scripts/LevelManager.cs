using UnityEngine;
using UnityEngine.SceneManagement; 
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private WorldClock timer;
    [SerializeField] private MonsterManager monster;
    [SerializeField] private int currentLevel = 0;
    private bool levelCompleted = false;
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
        if (monster == null) Debug.Log("Add MonsterManger to Level Manager Script!");
        LoadLevel(currentLevel);
    }

    private void Update()
    {
        candyGiven = monster.GetMonsterTreats();
        Debug.Log("Candy Given: " + candyGiven); 
        if (!LevelCompleted())
        {
            currentTime += Time.deltaTime; 

            if (LevelCompleted())
            {
                if (PlayerWinsLevel())
                {
                    currentLevel++;

                    if (currentLevel < levels.Length)
                    {
                        LoadLevel(currentLevel);
                    }
                    else
                    {
                        Debug.Log("Game Completed!");
                    }
                }
                else
                {
                    Debug.Log("GameOver!"); 
                }
            }
        }
    }

    private bool PlayerWinsLevel()
    {
        if (candyGiven >= levels[currentLevel].requiredTreats)
        {
            return true;
        }
        return false;
    }

    private void LoadLevel(int levelNum)
    {
        levelCompleted = false;

        Debug.Log("Level " + (levelNum + 1) + " Loaded");
    }

    private bool LevelCompleted()
    {
        if (currentTime >= timer.GetMaxTime() || monster.GetMonsterTreats() >= levels[currentLevel].requiredTreats)
        {
            return true; 
        }
        return false; 
    }
}
