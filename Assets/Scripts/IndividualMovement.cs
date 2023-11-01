using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualMovement : MonoBehaviour
{
    public bool canMoveIndividually;
    public Vector3 target;
    public Vector3 direction;

    [SerializeField] public float Speed;

    void Start()
    {
        canMoveIndividually = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMoveIndividually)
        {
            //MoveToTarget();
            /*MoveInDirection();
            MoveToTargetHorizontally();*/
            //MoveToTarget();
        }
    }

    /*private void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * SpeedZ);
    }

    private void MoveToTargetHorizontally()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.x, Time.deltaTime * SpeedX),
            transform.position.y, transform.position.z);
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.x, Time.deltaTime * SpeedX),
            transform.position.y,
            Mathf.Lerp(transform.position.z, target.z, Time.deltaTime * SpeedZ));
    }*/

    private void MoveInDirection()
    {
        /*transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + direction.x, Time.deltaTime * SpeedZ),
            transform.position.y,
            Mathf.Lerp(transform.position.z, transform.position.z + direction.z, Time.deltaTime * SpeedZ));*/
    }

    public void SetupForMovement()
    {
        canMoveIndividually = true;
    }

    public void StopIndividualMovement()
    {
        canMoveIndividually = false;
    }
}
