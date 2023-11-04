using UnityEngine;

public class DecalPool : MonoBehaviour
{
    public int maxPoolSize = 1000;
    public float decalSizeMin = 1.0f;
    public float decalSizeMax = 3.0f;

    private ParticleSystem DecalPS;
    private int decalDataPoolCursor;
    private DecalData[] decalDataPool;
    private ParticleSystem.Particle[] decalParticles;

    private void Awake()
    {
        DecalPS = GetComponent<ParticleSystem>();
        decalDataPool = new DecalData[maxPoolSize];
        decalParticles = new ParticleSystem.Particle[maxPoolSize];

        for(int i = 0; i < decalDataPool.Length; i++)
        {
            decalDataPool[i] = new DecalData();
        }
        decalDataPoolCursor = 0;
    }

    public void OnDropletParticleHitToGround(ParticleCollisionEvent particleCollisionEvent, Color color)
    {
        //color = Color.red;
        if (color == ParticleSystemManager.instance.Red)
        {
            Debug.Log("red decals will be displayed");
        }
        UpdateDataPool(particleCollisionEvent, color);
        DisplayDecals();
    }

    private void UpdateDataPool(ParticleCollisionEvent particleCollisionEvent, Color color)
    {
        if (decalDataPoolCursor >= maxPoolSize)
        {
            decalDataPoolCursor = 0;
        }

        decalDataPool[decalDataPoolCursor].Position = particleCollisionEvent.intersection;
        decalDataPool[decalDataPoolCursor].Position.y = 6f;
        //Vector3 decalEulers = Quaternion.LookRotation(particleCollisionEvent.normal).eulerAngles;color
        Vector3 decalEulers = Quaternion.LookRotation(-particleCollisionEvent.normal,Vector3.up).eulerAngles;
        decalEulers.z = Random.Range(0, 360);
        decalDataPool[decalDataPoolCursor].Rotation3D = decalEulers;
        decalDataPool[decalDataPoolCursor].Size3D = new Vector3(Random.Range(decalSizeMin, decalSizeMax), Random.Range(decalSizeMin, decalSizeMax), Random.Range(decalSizeMin, decalSizeMax));
        decalDataPool[decalDataPoolCursor].color = color;

        decalDataPoolCursor++;
    }

    private void DisplayDecals()
    {
        Debug.Log("DisplayDecalsCalled");
        for (int i = 0; i < decalDataPool.Length; i++)
        {
            decalParticles[i].position = decalDataPool[i].Position + new Vector3(0,-0.5f,0);
            decalParticles[i].rotation3D = decalDataPool[i].Rotation3D;//new Vector3(90,0,0);//
            //decalParticles[i].startSize = decalDataPool[i].Size;
            decalParticles[i].startSize3D = decalDataPool[i].Size3D;
            decalParticles[i].startColor = decalDataPool[i].color;
        }
        DecalPS.SetParticles(decalParticles, decalParticles.Length);
    }
}
