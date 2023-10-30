using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public Color color;
    public EnemyController controller;
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
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeadlySurface"))
        {
            CallPSOnCollision(collision);
            controller.KillCharacter(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            CallPSOnCollision(collision);
            controller.KillCharacter(gameObject);
            PlayerManager.instance.KillCharacter(collision.gameObject);
        }
    }*/

    /*private void CallPSOnCollision(Collision collision)
    {
        Vector3 position = transform.position + Vector3.up;
        Vector3 normal = collision.GetContact(0).normal;
        ParticleSystemManager.instance.EmitDropletBurst(position, normal, color);
    }*/

    //call when character is added to an enemy group
    public void SetEnemyController(EnemyController controller)
    {
        if(controller == null)
        {
            this.controller = null;
            return;
        }
        this.controller = controller;
    }
}
