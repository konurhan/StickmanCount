using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public static PlayerMovementController instance;

    public bool isHoldingDown;
    public Vector3 lastTouchPos;

    public Camera mainCam;
    public Camera canvasCam;
    public RaycastHit hit;

    public Transform Parent;

    public float horSpeed;
    public float SpeedZ;

    bool overrideMovementControl;

    bool isStarted;

    LayerMask mask = 1 << 6;

    private void Awake()
    {
        instance = this;
        isHoldingDown = false;
        overrideMovementControl = false;
        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted)
        {
            return;
        }

        if (!overrideMovementControl)
        {
            HoldingCheck();
        }        
    }

    public void HoldingCheck()
    {
        if(Input.GetMouseButtonDown(0) && !isHoldingDown)
        {
            isHoldingDown = true;
            UpdateLastPosition();
        }
        if(Input.GetMouseButtonUp(0) && isHoldingDown)
        {
            isHoldingDown = false;
        }

        if (isHoldingDown)//move horizontally
        {
            Parent.transform.position = new Vector3(Mathf.Lerp(Parent.transform.position.x, lastTouchPos.x, Time.deltaTime * horSpeed)
                , Parent.transform.position.y, Parent.transform.position.z);
            UpdateLastPosition();
        }

        Parent.transform.position = new Vector3(Parent.transform.position.x, Parent.transform.position.y,
            Mathf.Lerp(Parent.transform.position.z, Parent.transform.position.z + 1, Time.deltaTime * SpeedZ));
    }

    private void UpdateLastPosition()
    {
        Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit);
        if (hit.point == Vector3.zero) return; 
        lastTouchPos = hit.point;
    }

    public void OverrideMovement()
    {
        overrideMovementControl = true;
        foreach (GameObject character in PlayerManager.instance.characters)
        {
            character.GetComponent<IndividualMovement>().SetupForMovement();
        }
        SoundManager.instance.StopWalkingClip();
    }

    public void ReturnToNormal()
    {
        overrideMovementControl = false;
        foreach (GameObject character in PlayerManager.instance.characters)
        {
            character.GetComponent<IndividualMovement>().StopIndividualMovement();
        }
        PlayerManager.instance.RepositionCharacters();
        SoundManager.instance.PlayWalkingClip();
    }

    public void TouchToStart()
    {
        isStarted = true;
        CanvasManager.instance.InGamePanel.GetChild(0).gameObject.SetActive(false);
        CanvasManager.instance.StartingUnitsButton.gameObject.SetActive(false);
        CanvasManager.instance.CoinMultiplierButton.gameObject.SetActive(false);
        CanvasManager.instance.SettingsButton.gameObject.SetActive(false);
        SoundManager.instance.PlayWalkingClip();
    }
}
