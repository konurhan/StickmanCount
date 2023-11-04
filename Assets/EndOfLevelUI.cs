using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class EndOfLevelUI : MonoBehaviour
{
    public Transform Message;
    public Transform AnimatedCharacter;
    public Transform Earnings;
    public Transform NextLevelButton;
    public Transform MoneyCount;
    public Transform BouncySurface;
    public Transform CoinsParent;

    WaitForSecondsRealtime waitCoinArrival = new WaitForSecondsRealtime(1.2f);

    [SerializeField] private GameObject[] coins;
    private Quaternion[] coinQuaternions;
    private Vector3[] coinPositions;

    private void Awake()
    {
        int count = CoinsParent.childCount;
        coins = new GameObject[count];
        coinQuaternions = new Quaternion[count];
        coinPositions = new Vector3[count];

        for (int i = 0; i < count; i++)//cache coins
        {
            GameObject coin = CoinsParent.GetChild(i).gameObject;
            coins[i] = coin;
            coinQuaternions[i] = coin.transform.rotation;
            coinPositions[i] = coin.transform.position;
        }
    }
    public void OnFailed()
    {
        Message.gameObject.GetComponent<TextMeshProUGUI>().text = "LEVEL FAILED";
        AnimatedCharacter.GetChild(0).gameObject.GetComponent<CharacterLevelEndAnimation>().Mourn();
        MoneyCount.gameObject.SetActive(false);
        Earnings.gameObject.SetActive(false);
        NextLevelButton.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Repeat Level";
    }

    public void OnPassed(int Reward, int moneyStart)
    {
        Message.gameObject.GetComponent<TextMeshProUGUI>().text = "LEVEL COMPLETED";
        AnimatedCharacter.GetChild(0).gameObject.GetComponent<CharacterLevelEndAnimation>().Celebrate();
        MoneyCount.gameObject.SetActive(true);
        Earnings.gameObject.SetActive(true);
        Earnings.gameObject.GetComponent<TextMeshProUGUI>().text = "Earnings: " + Reward;
        NextLevelButton.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Next Level";

        TweenCoins();
        StartCoroutine(IncreaseMoneyCount(moneyStart, Reward, MoneyCount.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>()));
    }

    private void TweenCoins()
    {
        ResetCoins();
        Vector2 moneyIconPos = MoneyCount.GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition;
        
        float delay = 0f;
        for (int i = 0; i < coins.Length; i++)
        {
            Transform coin = coins[i].transform;

            coin.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            coin.GetComponent<RectTransform>().DOAnchorPos(moneyIconPos, 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                SoundManager.instance.PlayCoinSound();
            });

            coin.DORotate(Vector3.zero, 0.8f).SetDelay(delay + 0.7f).SetEase(Ease.Flash);

            coin.DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;

            MoneyCount.GetChild(1).DOScale(1.2f, 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(1.2f);
        }
    }

    private void ResetCoins()
    {
        for(int i = 0; i < coins.Length; i++)
        {
            coins[i].transform.rotation = coinQuaternions[i];
            coins[i].transform.position = coinPositions[i];
        }
    }

    private IEnumerator IncreaseMoneyCount(int initial, int toBeAdded, TextMeshProUGUI moneyText)
    {
        moneyText.text = initial.ToString();
        yield return waitCoinArrival;

        WaitForSecondsRealtime waitObj = new WaitForSecondsRealtime(1.2f / toBeAdded);
        int count = 0;
        while (count < toBeAdded)
        {
            count++;
            moneyText.text = (initial+count).ToString();
            yield return waitObj;
        }
    }
}
