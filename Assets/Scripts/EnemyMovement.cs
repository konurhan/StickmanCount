using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyController controller;
    [SerializeField] private float reactionDistance;

    int distanceCount;
    float SpeedZ;
    [SerializeField] private Vector3 target;

    private void Start()
    {
        controller = transform.GetChild(1).gameObject.GetComponent<EnemyController>();
        distanceCount = 0;
        SpeedZ = 2f;//PlayerMovementController.instance.SpeedZ;
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceCount == 0)
        {
            if (IsClose())
            {
                StartMovement();
                distanceCount++;
                target = transform.position + (PlayerMovementController.instance.Parent.position - transform.position)/2;
                OverrideMovement(target);
                PlayerMovementController.instance.OverrideMovement(target);
            }
        }
        else
        {
            CheckForCombatEnd();
        }
    }

    private void CheckForCombatEnd()
    {
        if (PlayerManager.instance.characters.Count == 0) 
        {
            //call level end method
            GameManager.Instance.OnLevelFailed();
            return;
        }
        if (controller.characters.Count == 0)
        {
            PlayerMovementController.instance.ReturnToNormal();
        }
    }

    private void OverrideMovement(Vector3 target)
    {
        foreach (GameObject character in controller.characters)
        {
            character.GetComponent<IndividualMovement>().SetupForMovement(target, SpeedZ);
        }
    }

    /*public void MoveTowardsCommonMiddlePoint(Vector3 target) 
    {
        PlayerMovementController.instance.MoveTowardsCommonMiddlePoint(target);
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.x, Time.deltaTime * SpeedZ),
            transform.position.y,
            Mathf.Lerp(transform.position.z, target.z, Time.deltaTime * SpeedZ));
    }*/

    public bool IsClose()
    {
        if (Vector3.Distance(controller.playerParent.position, controller.CharactersParent.position) <= reactionDistance) return true;
        return false;
    }

    public void StartMovement()
    {
        foreach(GameObject character in controller.characters)
        {
            character.GetComponent<Animator>().SetBool("run", true);
        }
    }

    public void StopMovement()
    {
        foreach (GameObject character in controller.characters)
        {
            character.GetComponent<Animator>().SetBool("run", false);
        }
    }
}
