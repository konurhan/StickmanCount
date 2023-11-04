
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

    public void SetupForMovement()
    {
        canMoveIndividually = true;
    }

    public void StopIndividualMovement()
    {
        canMoveIndividually = false;
    }
}
