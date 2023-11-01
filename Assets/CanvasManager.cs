using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public Transform EndOfLevelPanel;
    public Transform InGamePanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.Instance.EndofTheLevelPanel = EndOfLevelPanel;
        GameManager.Instance.InGamePanel = InGamePanel;
    }

    public void NextLevelButtonCallback()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadNextLevel());
    }
}
