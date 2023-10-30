using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    public static ParticleSystemManager instance;

    public ParticleSystem DropletPSRed;
    public ParticleSystem DropletPSBlue;
    public ParticleSystem DecalPSred;
    public ParticleSystem DecalPSblue;

    public DecalPool decalPoolRed;
    public DecalPool decalPoolBlue;

    public Color32 Red;
    public Color32 Blue;

    public int emittionAmount;

    private void Awake()
    {
        instance = this;
        decalPoolBlue = transform.GetChild(2).GetComponent<DecalPool>();
        decalPoolRed = transform.GetChild(3).GetComponent<DecalPool>();

    }

    private void Start()
    {
        
    }

    //call from collision of characterControl of enemy and player characters
    public void EmitDropletBurst(Vector3 position, Vector3 contactNormal, Color color)
    {
        EmitAtLocation(position, contactNormal, color);
    }

    public void EmitAtLocation(Vector3 position, Vector3 contactNormal, Color color)
    {
        ParticleSystem ps;
        if (color == Red)
        {
            ps = DropletPSRed;
            Debug.Log("Color is red");
        }
        else ps = DropletPSBlue;

        ps.transform.position = position;
        //ps.transform.rotation = Quaternion.LookRotation(contactNormal);
        ps.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
        ParticleSystem.MainModule main = ps.main;
        //main.startSize = 3f;
        ps.Emit(emittionAmount);
    }
}


