using System.Collections;
using UnityEngine;

public class CheckFinishLine : MonoBehaviour
{
    int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (count == 0)
        {
            StartCoroutine(InvokeMethodDelayed(1f));
            count++;
        }
        
    }

    private IEnumerator InvokeMethodDelayed(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        GameManager.Instance.OnLevelFinished();
    }
}
