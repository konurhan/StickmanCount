using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropletParticleCollision : MonoBehaviour
{
    public ParticleSystem DropletPS;
    public List<ParticleCollisionEvent> collisionEvents;
    [SerializeField] private string Name;
    [SerializeField] private Color32 color;
    [SerializeField] private DecalPool decalPool;
    
    private void Start()
    {
        DropletPS = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        if (color.CompareRGB(ParticleSystemManager.instance.Red))
        {
            decalPool = ParticleSystemManager.instance.decalPoolRed;
        }
        else if (color.CompareRGB(ParticleSystemManager.instance.Blue))
        {
            color = ParticleSystemManager.instance.Blue;
            decalPool = ParticleSystemManager.instance.decalPoolBlue;
        }
    }


    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("ParticleCollision is called");
        ParticlePhysicsExtensions.GetCollisionEvents(DropletPS, other, collisionEvents);
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            decalPool.OnDropletParticleHitToGround(collisionEvents[i],color);
        }
    }
}
