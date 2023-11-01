using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(IncreaseMoneyCount(moneyStart, Reward, MoneyCount.gameObject.GetComponent<TextMeshProUGUI>()));//simultaneously start increasing money count
    }

    private void TweenCoins()
    {
        ResetCoins();
        Vector2 moneyIconPos = MoneyCount.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition;
        
        float delay = 0f;
        for (int i = 0; i < coins.Length; i++)
        {
            Transform coin = coins[i].transform;

            coin.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            coin.GetComponent<RectTransform>().DOAnchorPos(moneyIconPos, 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack);//1.3 sec

            coin.DORotate(Vector3.zero, 0.8f).SetDelay(delay + 0.7f).SetEase(Ease.Flash);//1 sec

            coin.DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);//1.8sec

            delay += 0.1f;//next coin will start moving after 0.1 sec than current coin's strat

            MoneyCount.GetChild(0).DOScale(1.2f, 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(1.2f);//loop count should be an even number so that sclace comes back to normal
        }
    }

    private void ResetCoins()
    {
        for(int i = 0; i < coins.Length; i++)
        {
            coins[i].transform.rotation = coinQuaternions[i];
            coins[i].transform.position = coinPositions[i];
            //coins[i].SetActive(true);
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
