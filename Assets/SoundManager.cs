using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip walking;
    public AudioClip walking1;
    public AudioClip walking2;

    public AudioClip pop;
    public AudioClip pop1;
    public AudioClip pop2;
    public AudioClip pop3;

    public AudioClip axeToGround;

    public AudioClip coin;
    public AudioClip coin1;
    public AudioClip coin2;
    public AudioClip coin3;
    public AudioClip coin4;

    public AudioClip gateClip;
    public AudioClip levelCompletion;
    public AudioClip levelFailed;

    private AudioSource WalkingSource;
    private AudioSource WalkingSource1;
    private AudioSource WalkingSource2;
    private AudioSource PopingSource;
    private AudioSource CoinSource;

    public AudioSource Gate;
    public AudioSource levelEnd;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        WalkingSource = PlayerMovementController.instance.Parent.GetChild(2).gameObject.GetComponent<AudioSource>();
        WalkingSource1 = PlayerMovementController.instance.Parent.GetChild(3).gameObject.GetComponent<AudioSource>();
        WalkingSource2 = PlayerMovementController.instance.Parent.GetChild(4).gameObject.GetComponent<AudioSource>();
        PopingSource = PlayerMovementController.instance.Parent.GetChild(1).gameObject.GetComponent<AudioSource>();
        CoinSource = GameManager.Instance.EndofTheLevelPanel.gameObject.GetComponent<EndOfLevelUI>().MoneyCount.gameObject.GetComponent<AudioSource>();

        AudioListener.pause = !GameManager.Instance.gameProgress.soundOn;
        WalkingSource.clip = walking; WalkingSource1.clip = walking1; WalkingSource2.clip = walking2;
        Gate.clip = gateClip;
    }

    public void PlayGateClip()
    {
        Gate.Play();
    }

    public void PlayLevelPassedClip()
    {
        levelEnd.clip = levelCompletion;
        levelEnd.Play();
    }

    public void PlayLevelFailedClip()
    {
        levelEnd.clip = levelFailed;
        levelEnd.Play();
    }

    public void PlayWalkingClip()
    {
        WalkingSource.loop = true;
        WalkingSource1.loop = true;
        WalkingSource2.loop = true;

        WalkingSource.Play(); WalkingSource1.Play(); WalkingSource2.Play();
    }

    public void StopWalkingClip()
    {
        WalkingSource.loop = false; WalkingSource.Stop();
        WalkingSource1.loop = false; WalkingSource1.Stop();
        WalkingSource2.loop = false; WalkingSource2.Stop();
    }

    public void PlayPlayerPop()
    {
        //WalkingSource.volume = 0.5f;

        int rand = Random.Range(0, 4);
        AudioClip clip = pop;
        switch (rand)
        {
            case 0:
                clip = pop;
                break;
            case 1:
                clip = pop1;
                break;
            case 2:
                clip = pop2;
                break;
            case 3:
                clip = pop3;
                break;
        }
        PopingSource.clip = clip;
        PopingSource.Play();
    }

    public void PlayCoinSound()
    {
        int rand = Random.Range(0, 5);
        AudioClip clip = coin;
        switch (rand)
        {
            case 0:
                clip = coin;
                break;
            case 1:
                clip = coin1;
                break;
            case 2:
                clip = coin2;
                break;
            case 3:
                clip = coin3;
                break;
            case 4:
                clip = coin4;
                break;
        }
        CoinSource.clip = clip;
        CoinSource.Play();
    }
}
