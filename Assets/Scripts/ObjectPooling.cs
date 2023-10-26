using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;

    [SerializeField] private int playerCharPoolSize;
    [SerializeField] private int enemyCharPoolSize;

    [SerializeField] private int charPoolExpansionAmount;

    [SerializeField] private List<GameObject> pooledPlayerChars;
    [SerializeField] private List<GameObject> pooledEnemyChars;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PoolPlayersCharacters(playerCharPoolSize);
    }

    public void PoolPlayersCharacters(int num)
    {
        for (int i = 0; i < num; i++)
        {
            InstantiateAndAddPlayerChars();
        }
    }

    public void InstantiateAndAddPlayerChars()
    {
        GameObject player = Instantiate(Resources.Load("Prefabs/Grandfather")) as GameObject;
        pooledPlayerChars.Add(player);
        player.SetActive(false);
    }

    public GameObject GetPlayerCharacter()
    {
        GameObject character = null;
        int count = pooledPlayerChars.Count;
        for (int i = 0; i < count; i++)
        {
            if (!pooledPlayerChars[i].activeInHierarchy)
            {
                character = pooledPlayerChars[i];
                break;
            }
        }
        if (character == null)
        {
            PoolPlayersCharacters(charPoolExpansionAmount);
            playerCharPoolSize += charPoolExpansionAmount;
            character = pooledPlayerChars[pooledPlayerChars.Count - 1];
        }

        return character;
    }

    public void SetPlayerCharacter(GameObject character)
    {
        character.SetActive(false);
        character.transform.parent = null;
    }
}
