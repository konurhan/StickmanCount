using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualMovement : MonoBehaviour
{
    public bool canMoveIndividually;
    public Vector3 target;

    private float SpeedZ;
    void Start()
    {
        canMoveIndividually = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMoveIndividually)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.x, Time.deltaTime * SpeedZ),
            transform.position.y,
            Mathf.Lerp(transform.position.z, target.z, Time.deltaTime * SpeedZ));
    }

    public void SetupForMovement(Vector3 target, float Speed)
    {
        canMoveIndividually = true;
        this.target = target;
        SpeedZ = Speed;
    }

    public void StopIndividualMovement()
    {
        canMoveIndividually = false;
        target = Vector3.zero;
        SpeedZ = 0;
    }
}
