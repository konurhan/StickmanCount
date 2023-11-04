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
        PoolEnemyCharacters(enemyCharPoolSize);
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

        Rigidbody rb = character.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.drag = 1000;
        rb.angularDrag = 10;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        character.GetComponent<CharacterControl>().isDying = false;

        character.transform.position = Vector3.zero;
        character.transform.rotation = Quaternion.identity;
        return character;
    }

    public void SetPlayerCharacter(GameObject character)
    {
        /*character.GetComponent<Rigidbody>().drag = 1000;
        character.GetComponent<Rigidbody>().angularDrag = 10;
        character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;*/
        character.GetComponent<IndividualMovement>().canMoveIndividually = false;
        character.SetActive(false);
        character.transform.parent = null;
    }

    public void PoolEnemyCharacters(int num)
    {
        for (int i = 0; i < num; i++)
        {
            InstantiateAndAddEnemyChars();
        }
    }

    public void InstantiateAndAddEnemyChars()
    {
        GameObject enemy = Instantiate(Resources.Load("Prefabs/Boy")) as GameObject;
        pooledEnemyChars.Add(enemy);
        enemy.SetActive(false);
    }

    public GameObject GetEnemyCharacter(EnemyController controller)
    {
        GameObject character = null;
        int count = pooledEnemyChars.Count;
        for (int i = 0; i < count; i++)
        {
            if (!pooledEnemyChars[i].activeInHierarchy)
            {
                character = pooledEnemyChars[i];
                break;
            }
        }
        if (character == null)
        {
            PoolPlayersCharacters(charPoolExpansionAmount);
            enemyCharPoolSize += charPoolExpansionAmount;
            character = pooledEnemyChars[pooledEnemyChars.Count - 1];
        }

        character.GetComponent<EnemyCollision>().SetEnemyController(controller);
        character.GetComponent<EnemyCollision>().isDying = false;
        return character;
    }

    public void SetEnemyCharacter(GameObject character)
    {
        character.GetComponent<IndividualMovement>().canMoveIndividually = false;
        character.GetComponent<EnemyCollision>().SetEnemyController(null);
        character.SetActive(false);
        character.transform.parent = null;
    }
}
