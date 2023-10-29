using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletParticleCollision : MonoBehaviour
{
    public ParticleSystem DropletPS;
    public List<ParticleCollisionEvent> collisionEvents;
    [SerializeField] private string Name;
    [SerializeField] private Color color;
    private void Start()
    {
        DropletPS = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        if (name == "red")
        {
            color = ParticleSystemManager.instance.Red;
        }
        else
        {
            color = ParticleSystemManager.instance.Blue;
        }
    }


    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("ParticleCollision is called");
        ParticlePhysicsExtensions.GetCollisionEvents(DropletPS, other, collisionEvents);
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            ParticleSystemManager.instance.decalPool.OnDropletParticleHitToGround(collisionEvents[i],color);
        }
    }
}
