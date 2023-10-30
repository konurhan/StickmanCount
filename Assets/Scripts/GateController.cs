using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public GateType type;
    public int number;

    public bool isConsumed;
    public int gateIndex;

    public Transform Text;
    public GameObject Bubbles;

    private void Awake()
    {
        isConsumed = false;
    }

    void Start()
    {
        GetGateIndex();
        SetText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isConsumed) return;
        if (!other.gameObject.CompareTag("Player")) return;
        Text.gameObject.SetActive(false);
        Bubbles.SetActive(false);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        SetGatesConsumed();
        switch (type)
        {
            case GateType.Multiplier:
                PlayerManager.instance.MultiplyBy(number); 
                break;
            case GateType.Divider:
                PlayerManager.instance.DivideBy(number);
                break;
            case GateType.Adder:
                PlayerManager.instance.AddBy(number);
                break;
            case GateType.Subtracter:
                PlayerManager.instance.SubtractBy(number);
                break;
        }
    }

    private void GetGateIndex()
    {
        if (transform.parent.GetSiblingIndex() == 0)
        {
            gateIndex = 0;
        }
        else if(transform.parent.GetSiblingIndex() == 1)
        {
            gateIndex = 1;
        }
    }

    public void SetGatesConsumed()
    {
        isConsumed = true;
        if (gateIndex == 0)
        {
            transform.parent.parent.GetChild(1).GetChild(2).GetComponent<GateController>().isConsumed = true;
            transform.parent.parent.GetChild(1).GetChild(2).GetComponent<BoxCollider>().enabled = false;
        }
        else if (gateIndex == 1)
        {
            transform.parent.parent.GetChild(0).GetChild(2).GetComponent<GateController>().isConsumed = true;
            transform.parent.parent.GetChild(0).GetChild(2).GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void SetText()
    {
        switch (type)
        {
            case GateType.Multiplier:
                Text.gameObject.GetComponent<TextMeshPro>().text = "x" + number.ToString();
                break;
            case GateType.Divider:
                Text.gameObject.GetComponent<TextMeshPro>().text = "÷" + number.ToString();
                break;
            case GateType.Adder:
                Text.gameObject.GetComponent<TextMeshPro>().text = "+" + number.ToString();
                break;
            case GateType.Subtracter:
                Text.gameObject.GetComponent<TextMeshPro>().text = "-" + number.ToString();
                break;
        }
    }
}

public enum GateType
{
    Multiplier,
    Divider,
    Adder,
    Subtracter
}
