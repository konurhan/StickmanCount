using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyController controller;
    [SerializeField] private float reactionDistance;
    [SerializeField] private float assignmentDistance;
    int distanceCheckCount;
    float SpeedZ;

    [SerializeField] private Vector3 target;

    private Transform enemyParent;
    private Transform playerParent;

    private void Start()
    {
        controller = transform.GetChild(1).gameObject.GetComponent<EnemyController>();
        distanceCheckCount = 0;

        enemyParent = controller.CharactersParent;
        playerParent = PlayerManager.instance.CharactersParent;
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceCheckCount == 0)
        {
            if (IsClose())
            {
                distanceCheckCount++;
                StartMovement();
                target = (playerParent.position - enemyParent.position)/2 + enemyParent.position;
                OverrideMovement();
                PlayerMovementController.instance.OverrideMovement();
                if (PlayerManager.instance.delayedReposition != null)
                {
                    PlayerManager.instance.StopCoroutine(PlayerManager.instance.delayedReposition);
                }

                ChangeEnemyRotations(playerParent.position - enemyParent.position);
                ChangePlayerRotations(enemyParent.position - playerParent.position);
            }
        }
        else
        {
            CheckForCombatEnd();
            AssignPlayersToEnemies();
        }
    }

    private void ChangeEnemyRotations(Vector3 direction)
    {
        enemyParent.transform.rotation = Quaternion.LookRotation(-direction);
    }
    private void ChangePlayerRotations(Vector3 direction)
    {
        playerParent.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void CheckForCombatEnd()
    {
        if (PlayerManager.instance.characters.Count == 0) 
        {
            StopMovement();
            ReturnToNormal();
            GameManager.Instance.OnLevelFailed();//delay this call so that you can see regrouping after combat has finished
            ChangeEnemyRotations(-Vector3.forward);
            return;
        }
        if (controller.characters.Count == 0)
        {
            PlayerMovementController.instance.ReturnToNormal();
            gameObject.SetActive(false);
            ChangePlayerRotations(Vector3.forward);
        }
    }

    private void AssignPlayersToEnemies()
    {
        foreach(GameObject player in PlayerManager.instance.characters)
        {
            if (enemyParent.childCount == 0) break;
            if (Vector3.Distance(player.transform.position, enemyParent.GetChild(0).position) <= assignmentDistance)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, enemyParent.GetChild(0).position,
                    Time.deltaTime * player.GetComponent<IndividualMovement>().Speed);
            }
        }
        foreach (GameObject enemy in controller.characters)
        {
            if (playerParent.childCount == 0) break;
            if (Vector3.Distance(enemy.transform.position, playerParent.GetChild(0).position) <= assignmentDistance)
            {
                enemy.transform.position = Vector3.Lerp(enemy.transform.position, playerParent.GetChild(0).position,
                    Time.deltaTime * enemy.GetComponent<IndividualMovement>().Speed);
            }
        }
    }

    private void OverrideMovement()
    {
        foreach (GameObject character in controller.characters)
        {
            character.GetComponent<IndividualMovement>().SetupForMovement();
        }
    }

    private void ReturnToNormal()
    {
        foreach (GameObject character in controller.characters)
        {
            character.GetComponent<IndividualMovement>().StopIndividualMovement();
        }
        controller.RepositionCharacters();
    }

    public bool IsClose()
    {
        if (Vector3.Distance(controller.playerParent.position, controller.CharactersParent.position) <= reactionDistance) return true;
        return false;
    }

    public void StartMovement()
    {
        foreach(GameObject character in controller.characters)
        {
            character.GetComponent<Animator>().SetBool("run", true);
        }
    }

    public void StopMovement()
    {
        foreach (GameObject character in controller.characters)
        {
            character.GetComponent<Animator>().SetBool("run", false);
        }
    }
}
