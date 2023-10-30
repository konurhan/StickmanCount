using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform CharactersParent;
    public List<GameObject> characters = new List<GameObject>();

    [Range(0, 4)][SerializeField] private float radius;
    [Range(0, 2)][SerializeField] private float radiusCoefficient;

    [Range(0, 1)][SerializeField] private float tweenDuration;

    public Transform countText;

    public float creationDistance;
    public int groupSize;

    public Transform playerParent;

    private int creationCount;

    private void Start()
    {
        CacheCharacters();
        UpdateCountText();
        playerParent = PlayerManager.instance.CharactersParent;
        creationCount = 0;
    }

    private void Update()
    {
        if (creationCount > 0) return;

        if (IsClose())
        {
            CreateCharactersWhenPlayerClose();
            creationCount++;
        }
    }

    public void CreateCharactersWhenPlayerClose()
    {
        AddNewCharacters(groupSize);
    }

    public bool IsClose()
    {
        if (Vector3.Distance(playerParent.position, CharactersParent.position) <= creationDistance) return true;
        return false;
    }

    public void CacheCharacters()
    {
        characters.Clear();
        int childCount = CharactersParent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            characters.Add(CharactersParent.GetChild(i).gameObject);
        }
        RepositionCharacters();
    }

    //repositions characters so that they stay in a circle
    public void RepositionCharacters()
    {
        int count = characters.Count;
        for (int i = 0; i < count; i++)
        {
            float x = radiusCoefficient * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            float z = radiusCoefficient * Mathf.Sqrt(i) * Mathf.Sin(i * radius);
            Vector3 moveTo = new Vector3(x, 0, z);
            characters[i].transform.DOLocalMove(moveTo, tweenDuration).SetEase(Ease.OutBack);
        }
    }

    //used to request "count" number of characters from the pool
    public void AddNewCharacters(int count)//reset character values before using them
    {
        for (int i = 0; i < count; i++)
        {
            //GameObject newChar = Instantiate(Resources.Load("Prefabs/Grandfather"), CharactersParent) as GameObject;
            GameObject newChar = ObjectPooling.instance.GetEnemyCharacter(this);
            newChar.SetActive(true);
            newChar.transform.SetParent(CharactersParent);
            newChar.transform.position = new Vector3(CharactersParent.position.x, 5f, CharactersParent.position.z);
            characters.Add(newChar);
        }
        UpdateCountText();
        RepositionCharacters();
    }

    public void RemoveCharacters(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ObjectPooling.instance.SetEnemyCharacter(characters[i]);
            characters.RemoveAt(i);
        }
        UpdateCountText();
        RepositionCharacters();
    }

    //called when character collides with it's enemy or with a deadly surface
    public void KillCharacter(GameObject character)
    {
        characters.Remove(character);
        ObjectPooling.instance.SetEnemyCharacter(character);
        UpdateCountText();
    }

    public IEnumerator DestroyCharacterDelayed(GameObject character)
    {
        yield return new WaitForSecondsRealtime(2);
        ObjectPooling.instance.SetEnemyCharacter(character);
    }

    public void UpdateCountText()
    {
        countText.gameObject.GetComponent<TextMeshPro>().text = characters.Count.ToString();
    }
}
