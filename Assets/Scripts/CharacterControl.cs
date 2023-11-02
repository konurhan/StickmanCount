using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public Color color;
    public bool isDying;

    private void Awake()
    {
        isDying = false;
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
        if (isDying) { return; }
        if (collision.gameObject.CompareTag("DeadlySurface")) 
        {
            if (PlayerManager.instance.delayedReposition == null && !gameObject.GetComponent<IndividualMovement>().canMoveIndividually)
            {
                PlayerManager.instance.delayedReposition = PlayerManager.instance.StartCoroutine(PlayerManager.instance.RepositionDelayed());
            }
            isDying = true;
            CallPSOnCollision(collision, color);
            PlayerManager.instance.KillCharacter(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<EnemyCollision>().isDying) return;
            isDying = true;
            collision.gameObject.GetComponent<EnemyCollision>().isDying = true;
            CallPSOnCollision(collision, color);
            CallPSOnCollision(collision, ParticleSystemManager.instance.Red);
            collision.gameObject.GetComponent<EnemyCollision>().controller.KillCharacter(collision.gameObject);
            PlayerManager.instance.KillCharacter(gameObject);
        }
    }

    public void CheckIfShouldFall()
    {
        if (PlayerManager.instance.isRePositioning) return;
        if(!Physics.Raycast(new Vector3(transform.position.x, 5.1f, transform.position.z), Vector3.down, 15))
        {
            //isFalling = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().drag = 0;
            GetComponent<Rigidbody>().angularDrag = 0.05f;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY /*|
                //RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionX*/;

            //following part can be realized inside a method name KillFallenCharacter in PlayerManager script as a coroutine
            PlayerManager.instance.characters.Remove(gameObject);
            PlayerManager.instance.UpdateCountText();
            gameObject.transform.SetParent(null,true);//so that character stops moving with others
            PlayerManager.instance.StartCoroutine(PlayerManager.instance.DestroyCharacterDelayed(gameObject));
        }
    }

    private void CallPSOnCollision(Collision collision, Color charColor)
    {
        Vector3 position = transform.position + Vector3.up;
        Vector3 normal = collision.GetContact(0).normal;
        ParticleSystemManager.instance.EmitDropletBurst(position, normal, charColor);
    }
}
