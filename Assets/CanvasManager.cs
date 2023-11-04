using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public Transform EndOfLevelPanel;
    public Transform InGamePanel;

    public Transform StartingUnitsButton;
    public Transform CoinMultiplierButton;
    public Transform SettingsButton;

    [SerializeField] private Color AudioColor;
    [SerializeField] private Color VibrationColor;

    [SerializeField] private Button AudioButton;
    [SerializeField] private Button VibrationButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.Instance.EndofTheLevelPanel = EndOfLevelPanel;
        GameManager.Instance.InGamePanel = InGamePanel;

        StartingUnitsButton.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.gameProgress.startingUnitsCount.ToString();
        CoinMultiplierButton.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.gameProgress.coinRewardMultiplier.ToString();

        EndOfLevelPanel.gameObject.GetComponent<EndOfLevelUI>().MoneyCount.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                                                                                       GameManager.Instance.gameProgress.coinCount.ToString();
        InGamePanel.GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                                                                                       GameManager.Instance.gameProgress.coinCount.ToString();

        if (GameManager.Instance.gameProgress.vibrationOn)
        {
            VibrationButton.image.color = VibrationColor;
        }
        else
        {
            VibrationButton.image.color = Color.white;
        }

        if (GameManager.Instance.gameProgress.soundOn)
        {
            AudioButton.image.color = AudioColor;
        }
        else
        {
            AudioButton.image.color = Color.white;
        }
    }

    public void OnSettingsPressed()
    {
        if (!SettingsButton.GetChild(0).gameObject.activeInHierarchy)
        {
            SettingsButton.GetChild(0).gameObject.SetActive(true);
            SettingsButton.GetChild(1).DORotate(new Vector3(0, 0, 180), 0.6f);
        }
        else
        {
            SettingsButton.GetChild(0).gameObject.SetActive(false);
            SettingsButton.GetChild(1).DORotate(new Vector3(0, 0, -1), 0.6f);
        }
    }

    public void OnAudioPressed()
    {
        //open close audio
        GameManager.Instance.ToggleSound();

        //change color
        if (AudioButton.image.color == Color.white)
        {
            AudioButton.image.color = AudioColor;
        }
        else
        {
            AudioButton.image.color = Color.white;
        }
    }

    public void OnVibrationPressed()
    {
        //open close vibration
        GameManager.Instance.ToggleVibration();

        //change color
        if (VibrationButton.image.color == Color.white)
        {
            VibrationButton.image.color = VibrationColor;
        }
        else
        {
            VibrationButton.image.color = Color.white;
        }
    }

    public void NextLevelButtonCallback()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadNextLevel());
    }

    public void BuyAStartingUnit()
    {
        //check if there is enough money
        if (GameManager.Instance.gameProgress.unitPrice <= GameManager.Instance.gameProgress.coinCount)
        {
            GameManager.Instance.gameProgress.coinCount -= GameManager.Instance.gameProgress.unitPrice;
            GameManager.Instance.gameProgress.unitPrice *= 2;
            GameManager.Instance.gameProgress.startingUnitsCount += 1;
            GameManager.Instance.SaveGame(GameManager.Instance.gameProgress);
        }
        else
        {
            return;
        }

        //update money and money text in boyh UI panels
        string newCount = GameManager.Instance.gameProgress.coinCount.ToString();
        TweenCount(EndOfLevelPanel.gameObject.GetComponent<EndOfLevelUI>().MoneyCount, newCount);
        TweenCount(InGamePanel.GetChild(2), newCount);

        //tween button: scale setloops yoyo
        TweenButtonOnPressed(StartingUnitsButton, GameManager.Instance.gameProgress.startingUnitsCount);

        //add a unit and reposition starting units
        PlayerManager.instance.AddNewCharacters(1);

        //update gameprogress
        GameManager.Instance.gameProgress.startingUnitsCount++;
        GameManager.Instance.SaveGame(GameManager.Instance.gameProgress);
    }

    public void BuyCoinRewardBoost()
    {
        if (GameManager.Instance.gameProgress.coinMultiplierPrice <= GameManager.Instance.gameProgress.coinCount)
        {
            GameManager.Instance.gameProgress.coinCount -= GameManager.Instance.gameProgress.coinMultiplierPrice;
            GameManager.Instance.gameProgress.coinMultiplierPrice *= 2;
            GameManager.Instance.gameProgress.coinRewardMultiplier += 1;
            GameManager.Instance.SaveGame(GameManager.Instance.gameProgress);
        }
        else
        {
            return;
        }

        string newCount = GameManager.Instance.gameProgress.coinCount.ToString();
        TweenCount(EndOfLevelPanel.gameObject.GetComponent<EndOfLevelUI>().MoneyCount, newCount);
        TweenCount(InGamePanel.GetChild(2), newCount);
        TweenButtonOnPressed(CoinMultiplierButton, GameManager.Instance.gameProgress.coinRewardMultiplier);
    }

    private void TweenButtonOnPressed(Transform button, int newAmount)
    {
        RectTransform numberText = button.GetChild(1).gameObject.GetComponent<RectTransform>();
        RectTransform buttonRect = button.gameObject.GetComponent<RectTransform>();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(numberText.DOScale(0, 0.1f));
        sequence.Append(buttonRect.DOScale(buttonRect.localScale / 1.1f, 0.1f).SetLoops(10, LoopType.Yoyo));
        sequence.Append(buttonRect.DOShakeAnchorPos(0.8f, 40, 15, 30)).OnComplete(() =>
        {
            button.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
                    "x " + newAmount.ToString();
        });
        sequence.Append(numberText.DOScale(new Vector3(0.4f,0.1f,1),0.4f).SetEase(Ease.OutBounce));
    }

    private void TweenCount(Transform text, string newText)//call for both UI panels
    {
        RectTransform numberText = text.GetChild(0).gameObject.GetComponent<RectTransform>();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(numberText.DOScale(0, 0.1f)).OnComplete(() =>
        {
            text.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = newText;
        });
        sequence.Append(numberText.DOScale(1, 0.4f).SetEase(Ease.InOutBack));
    }
}
