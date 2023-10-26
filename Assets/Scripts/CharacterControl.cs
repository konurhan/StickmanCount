using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] bool isFalling;
    private Coroutine deathRoutine;


    private void Awake()
    {
        isFalling = false;
    }
    void Start()
    {
        
    }

    void Update()
    {
        CheckIfShouldFall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeadlySurface")) 
        { 
            PlayerManager.instance.KillCharacter(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            PlayerManager.instance.KillCharacter(collision.gameObject);
            PlayerManager.instance.KillCharacter(gameObject);
        }
    }

    public void CheckIfShouldFall()
    {
        if(!Physics.Raycast(new Vector3(transform.position.x, 5.1f, transform.position.z), Vector3.down, 15))
        {
            isFalling = true;
            GetComponent<Rigidbody>().isKinematic = false;
            //following part can be realized inside a method name KillFallenCharacter in PlayerManager script as a coroutine
            PlayerManager.instance.characters.Remove(gameObject);
            PlayerManager.instance.UpdateCountText();
            gameObject.transform.parent = null;//so that character stops moving with other
            deathRoutine = PlayerManager.instance.StartCoroutine(PlayerManager.instance.DestroyCharacterDelayed(gameObject));
        }
    }
}
