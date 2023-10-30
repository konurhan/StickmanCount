using System.Collections;
using System.Collections.Generic;
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
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        gameProgress = new GameProgress();
    }

    private void Start()
    {
        LoadGame();
        LoadNextLevel();
    }

    public void OnLevelFinished()
    {
        //calculate reward - and show it on screen
        int coinReward = PlayerManager.instance.characters.Count * coinRewardMultiplier;
        //update progress
        gameProgress.lastUnlockedLevel++;
        gameProgress.coinCount += coinReward;

        //save game
        SaveGame();
        //pause time

        //open canvas and wait for user to push next level button
        EndofTheLevelPanel.gameObject.SetActive(true);
        PlayerManager.instance.CharactersParent.gameObject.SetActive(false);
        PlayerManager.instance.gameObject.SetActive(false);

    }

    public void OnLevelFailed()
    {
        EndofTheLevelPanel.gameObject.SetActive(true);
        PlayerManager.instance.CharactersParent.gameObject.SetActive(false);
        PlayerManager.instance.gameObject.SetActive(false);
    }

    public IEnumerator LoadNextLevel()
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("Level" + gameProgress.lastUnlockedLevel);

        while (!sceneLoad.isDone)
        {
            yield return null;
        }
        EndofTheLevelPanel.gameObject.SetActive(false);
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
