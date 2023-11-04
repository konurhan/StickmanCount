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
