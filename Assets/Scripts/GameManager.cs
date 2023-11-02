using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameProgress gameProgress;

    public int coinRewardMultiplier = 2;

    public Transform InGamePanel;
    public Transform EndofTheLevelPanel;

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

        gameProgress = new GameProgress();
        LoadGame();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(LoadNextLevel());
        }
        else
        {
            InGamePanel = CanvasManager.instance.InGamePanel;
            EndofTheLevelPanel = CanvasManager.instance.EndOfLevelPanel;
        }
    }

    private void Start()
    {
    }

    public void OnLevelFinished()
    {
        //calculate reward
        int coinReward = (PlayerManager.instance.characters.Count * gameProgress.coinRewardMultiplier);
        int coinStart = gameProgress.coinCount;
        
        //update progress
        gameProgress.lastUnlockedLevel++;
        gameProgress.coinCount += coinReward;

        //save game
        SaveGame();

        //open canvas and wait for user to push next level button
        EndofTheLevelPanel.gameObject.SetActive(true);
        EndofTheLevelPanel.gameObject.GetComponent<EndOfLevelUI>().OnPassed(coinReward, coinStart);
        PlayerManager.instance.CharactersParent.gameObject.SetActive(false);
        PlayerManager.instance.gameObject.SetActive(false);

    }

    public void OnLevelFailed()
    {
        EndofTheLevelPanel.gameObject.SetActive(true);
        EndofTheLevelPanel.gameObject.GetComponent<EndOfLevelUI>().OnFailed();
        PlayerManager.instance.CharactersParent.gameObject.SetActive(false);
        PlayerManager.instance.gameObject.SetActive(false);
    }

    public void LoadNextLevelButton()
    {
        StartCoroutine(LoadNextLevel());
    }

    public IEnumerator LoadNextLevel()
    {
        if (SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Level"+ gameProgress.lastUnlockedLevel.ToString()+".unity") == -1)//Assets/Scenes/Level1.unity
        {
            Debug.Log("Couldn' find scene: Assets/Scenes/Level1.unity// last unlocked level is: "+ gameProgress.lastUnlockedLevel);
            gameProgress.lastUnlockedLevel--;
            SaveGame();
            Debug.Log("No more levels beyond level: " + gameProgress.lastUnlockedLevel);
        }
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("Level" + gameProgress.lastUnlockedLevel);

        while (!sceneLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    private void LoadGame()//call only when the game opens
    {
        GameProgress progress = SaveSystem.LoadData<GameProgress>("/GameProgress.json");
        if (progress != null)
        {
            gameProgress = progress;
        }
        else
        {
            gameProgress = new GameProgress();
        }
    }

    public void SaveGame()//call when a level is finished, player bought something
    {
        if (gameProgress == null)
        {
            gameProgress = new GameProgress();
        }
        SaveSystem.SaveData("/GameProgress.json", gameProgress);
    }

    public void UpdateGameProgress(GameProgress progress)//call from other scripts
    {
        gameProgress = progress;
    }
}
