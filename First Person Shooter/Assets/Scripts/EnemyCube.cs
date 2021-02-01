using UnityEngine;
public enum SimpleStateBehaviour
{
    Idle,
    EnemyFound,
    Shooting
}

public class EnemyCube : MonoBehaviour
{
    public LayerMask whatIsPlayer;
    public LayerMask whatCanEnemySee;
    public Transform playerTarget;
    private EnemyCubeHealth health;
    public EnemyProjectileGun gun;

    public SimpleStateBehaviour currentState;

    public float detectionRange = 10f;
    public float shootingRange = 5f;
    public float rotSpeed = 15f;

    public float playerDistance;

    private void Awake()
    {
        health = GetComponent<EnemyCubeHealth>();
    }
    private void Update()
    {
        if (health.isDeath) return;

        SearchForPlayer();

        switch (currentState)
        {
            case SimpleStateBehaviour.Idle:
                // Do nothing
                if (playerTarget) currentState = SimpleStateBehaviour.EnemyFound;
                break;
            case SimpleStateBehaviour.EnemyFound:
                // Look at the enemy and see if it can shoot him using a raycast
                if (!playerTarget) currentState = SimpleStateBehaviour.Idle;
                LookAtPlayer(playerTarget);
                if (CanShootPlayer() || playerDistance <= shootingRange) currentState = SimpleStateBehaviour.Shooting;
                break;
            case SimpleStateBehaviour.Shooting:
                LookAtPlayer(playerTarget);
                if (!CanShootPlayer() && playerDistance > shootingRange)
                {
                    gun.shooting = false;
                    currentState = SimpleStateBehaviour.EnemyFound;
                }
                else
                {
                    // Use the gun
                    gun.shooting = true;
                }
                break;
        }
    }

    private void SearchForPlayer()
    {
        Collider[] player = Physics.OverlapSphere(transform.position, detectionRange, whatIsPlayer);
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].CompareTag("Player"))
            {
                playerTarget = player[i].transform;
                break;
            }
            else
            {
                playerTarget = null;
            }
        }

        if (playerTarget) playerDistance = (playerTarget.position - transform.position).magnitude;
    }

    private void LookAtPlayer(Transform player)
    {
        if (!player) return;

        var lookPos = player.position - transform.position;
        lookPos.y = 0;

        var targetRotation = Quaternion.LookRotation(lookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    private bool CanShootPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, whatCanEnemySee))
        {
            if (hit.transform.CompareTag("Obstacle"))
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
                return false;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
                return true;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * detectionRange, Color.red);
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
