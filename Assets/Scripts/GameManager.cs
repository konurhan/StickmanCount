using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameProgress gameProgress;

    public int coinRewardMultiplier = 2;

    public Transform InGamePanel;
    public Transform EndofTheLevelPanel;

    public bool vibrationOn;
    public bool soundOn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        gameProgress = LoadGame();
        
        vibrationOn = gameProgress.vibrationOn;
        soundOn = gameProgress.soundOn;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("build index is 0, starting to load next level");
            StartCoroutine(LoadNextLevel());
        }
        else
        {
            InGamePanel = CanvasManager.instance.InGamePanel;
            EndofTheLevelPanel = CanvasManager.instance.EndOfLevelPanel;
        }
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
        gameProgress.soundOn = soundOn;
        SaveGame(gameProgress);

        AudioListener.pause = !soundOn;
    }

    public void ToggleVibration()
    {
        vibrationOn = !vibrationOn;
        gameProgress.vibrationOn = vibrationOn;
        SaveGame(gameProgress);
    }

    public void OnLevelFinished()
    {
        //stop sound effect
        SoundManager.instance.StopWalkingClip();

        //calculate reward
        int coinReward = (PlayerManager.instance.characters.Count * gameProgress.coinRewardMultiplier);
        int coinStart = gameProgress.coinCount;
        
        //update progress
        gameProgress.lastUnlockedLevel++;
        gameProgress.coinCount += coinReward;

        //save game
        SaveGame(gameProgress);

        //open canvas and wait for user to push next level button
        EndofTheLevelPanel.gameObject.SetActive(true);
        EndofTheLevelPanel.gameObject.GetComponent<EndOfLevelUI>().OnPassed(coinReward, coinStart);
        PlayerManager.instance.CharactersParent.gameObject.SetActive(false);
        PlayerManager.instance.gameObject.SetActive(false);
        SoundManager.instance.PlayLevelPassedClip();
    }

    public void OnLevelFailed()
    {
        EndofTheLevelPanel.gameObject.SetActive(true);
        EndofTheLevelPanel.gameObject.GetComponent<EndOfLevelUI>().OnFailed();
        PlayerManager.instance.CharactersParent.gameObject.SetActive(false);
        PlayerManager.instance.gameObject.SetActive(false);
        SoundManager.instance.PlayLevelFailedClip();
    }

    public void LoadNextLevelButton()
    {
        StartCoroutine(LoadNextLevel());
    }

    public IEnumerator LoadNextLevel()
    {
        while (SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Level"+ gameProgress.lastUnlockedLevel.ToString()+".unity") == -1)//Assets/Scenes/Level1.unity
        {
            Debug.Log("Couldn' find scene: Assets/Scenes/Level1.unity// last unlocked level is: "+ gameProgress.lastUnlockedLevel);
            gameProgress.lastUnlockedLevel--;
            SaveGame(gameProgress);
            Debug.Log("No more levels beyond level: " + gameProgress.lastUnlockedLevel);
        }
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("Level" + gameProgress.lastUnlockedLevel);
        Debug.Log("async level load is called");

        while (!sceneLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    private GameProgress LoadGame()//call only when the game opens
    {
        GameProgress progress = SaveSystem.LoadData<GameProgress>("/GameProgress.json");
        GameProgress returned;
        if (progress != null)
        {
            returned = progress;
        }
        else
        {
            returned = new GameProgress();
        }
        return returned;
    }

    public void SaveGame(GameProgress toBeSaved)//call when a level is finished, player bought something
    {
        if (toBeSaved == null)
        {
            toBeSaved = new GameProgress();
        }
        SaveSystem.SaveData("/GameProgress.json", toBeSaved);
    }

    public void UpdateGameProgress(GameProgress progress)//call from other scripts
    {
        gameProgress = progress;
    }
}
