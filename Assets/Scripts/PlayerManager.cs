using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

//Manages characters belonging to the player
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform CharactersParent;
    public List<GameObject> characters = new List<GameObject>();

    [Range(0, 4)] [SerializeField] private float radius;
    [Range(0, 2)] [SerializeField] private float radiusCoefficient;

    [Range(0, 1)] [SerializeField] private float tweenDuration;

    public Transform countText;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
        }

        CacheCharacters();
        UpdateCountText();
    }
    
    public void CacheCharacters()
    {
        characters.Clear();
        int childCount = CharactersParent.childCount;
        for (int i = 0; i < childCount; i++) 
        { 
            characters.Add(CharactersParent.GetChild(i).gameObject);
        }
    }

    //repositions characters so that they stay in a circle
    public void RepositionCharacters()
    {
        int count = characters.Count;
        for(int i = 0; i < count; i++) 
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
            GameObject newChar = ObjectPooling.instance.GetPlayerCharacter();
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
            ObjectPooling.instance.SetPlayerCharacter(characters[i]);
            characters.RemoveAt(i);
        }
        UpdateCountText();
        RepositionCharacters();
    }

    //called when character collides with it's enemy or with a deadly surface
    public void KillCharacter(GameObject character)
    {
        characters.Remove(character);
        ObjectPooling.instance.SetPlayerCharacter(character);
        UpdateCountText();
        //play particle system popping effect
    }

    public IEnumerator DestroyCharacterDelayed(GameObject character)
    {
        yield return new WaitForSecondsRealtime(2);
        ObjectPooling.instance.SetPlayerCharacter(character);
    }

    public void UpdateCountText()
    {
        countText.gameObject.GetComponent<TextMeshPro>().text = characters.Count.ToString();
    }

    public void MultiplyBy(int num)
    {
        int toAdd = characters.Count * (num-1);
        AddNewCharacters(toAdd);
    }

    public void DivideBy(int num)
    {
        int toRemove = characters.Count - (characters.Count / num);
        RemoveCharacters(toRemove);
    }

    public void SubtractBy(int num)
    {
        RemoveCharacters(num);
    }

    public void AddBy(int num)
    {
        AddNewCharacters(num); 
    }
}
