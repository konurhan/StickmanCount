using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public static PlayerMovementController instance;

    public bool isHoldingDown;
    public Vector3 lastTouchPos;

    public Camera mainCam;
    public RaycastHit hit;

    public Transform Parent;

    public float horSpeed;
    public float SpeedZ;

    bool overrideMovementControl;

    private void Awake()
    {
        instance = this;
        isHoldingDown = false;
        overrideMovementControl = false;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void ReturnToNormal()
    {
        overrideMovementControl = false;
        foreach (GameObject character in PlayerManager.instance.characters)
        {
            character.GetComponent<IndividualMovement>().StopIndividualMovement();
        }
        PlayerManager.instance.RepositionCharacters();
    }
}
