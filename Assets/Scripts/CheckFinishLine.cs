using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckFinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(InvokeMethodDelayed(1f));
    }

    private IEnumerator InvokeMethodDelayed(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        GameManager.Instance.OnLevelFinished();
    }
}
