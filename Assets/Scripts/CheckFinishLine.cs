using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.OnLevelFinished();
    }
}
