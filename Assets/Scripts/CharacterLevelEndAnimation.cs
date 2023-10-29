using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelEndAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        int rand = Random.Range(0, 3);
        animator.SetInteger("danceNum", rand);
    }
}
