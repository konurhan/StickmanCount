using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    public static ParticleSystemManager instance;

    public ParticleSystem DropletPSRed;
    public ParticleSystem DropletPSBlue;
    public ParticleSystem DecalPS;

    public DecalPool decalPool;

    public Color Red;
    public Color Blue;

    public int emittionAmount;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        decalPool = transform.GetChild(2).GetComponent<DecalPool>();
    }

    //call from collision of characterControl of enemy and player characters
    public void EmitDropletBurst(Vector3 position, Vector3 contactNormal, Color color)
    {
        EmitAtLocation(position, contactNormal, color);
    }

    public void EmitAtLocation(Vector3 position, Vector3 contactNormal, Color color)
    {
        ParticleSystem ps;
        if (color == Red) ps = DropletPSRed;
        else ps = DropletPSBlue;

        ps.transform.position = position;
        //ps.transform.rotation = Quaternion.LookRotation(contactNormal);
        ps.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
        ps.Emit(emittionAmount);
    }
}


