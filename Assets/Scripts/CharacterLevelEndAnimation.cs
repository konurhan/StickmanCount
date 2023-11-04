using UnityEngine;

public class CharacterLevelEndAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Celebrate()
    {
        int rand = Random.Range(0, 3);
        animator.SetInteger("danceNum", rand);
    }

    public void Mourn()
    {
        //int rand = Random.Range(3, 6);
        int rand = 3;
        animator.SetInteger("danceNum", rand);
    }

    public void StopDancing()
    {
        animator.SetTrigger("stopDancing");
    }
}
