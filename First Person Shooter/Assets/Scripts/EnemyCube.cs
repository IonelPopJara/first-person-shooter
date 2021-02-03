using UnityEngine;
public enum SimpleStateBehaviour
{
    Idle,
    EnemyFound,
    Shooting
}

public class EnemyCube : MonoBehaviour
{
    [Header("State Machine")]
    public SimpleStateBehaviour currentState;

    [Header("Components")]
    public Transform enemyEyes;
    public EnemyProjectileGun gun;
    public EnemyGunRotation gunRotation;

    [Header("Enemy stats")]
    public float detectionRange = 10f;
    public float shootingRange = 5f;
    public float rotSpeed = 15f;

    [Header("Layer Masks")]
    public LayerMask whatIsPlayer;
    public LayerMask whatCanEnemySee;
    
    [Header("Debug")]
    public Transform playerTarget;
    public float playerDistance;
    public Vector3 shootPoint;

    private EnemyCubeHealth health;

    private void Awake()
    {
        health = GetComponent<EnemyCubeHealth>();
    }

    private void Update()
    {
        if (health == null)
        {
            Debug.Log($"ERROR: No {typeof(EnemyCubeHealth)} provided");
            return;
        }

        if (health.isDeath) return;

        SearchForPlayer();

        gun.SetTargetPoint(shootPoint);

        gunRotation.PlayerTarget = playerTarget;

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
                    // Don't use the gun
                    gun.SetShooting(false);
                    currentState = SimpleStateBehaviour.EnemyFound;
                }
                else
                {
                    // Use the gun
                    gun.SetShooting(true);
                }
                break;
        }
    }

    private void SearchForPlayer()
    {
        //Debug.Log("Search For Player");
        playerTarget = null;

        Collider[] player = Physics.OverlapSphere(transform.position, detectionRange, whatIsPlayer);
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].CompareTag("Player"))
            {
                Debug.Log($"{player[i].name}");
                playerTarget = player[i].transform;
                break;
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
        Ray ray = new Ray(enemyEyes.position, enemyEyes.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange, whatCanEnemySee))
        {
            if (hit.transform.CompareTag("Obstacle"))
            {
                Debug.DrawRay(enemyEyes.position, enemyEyes.forward * hit.distance, Color.red);
                shootPoint = hit.point;
                return false;
            }
            else
            {
                Debug.DrawRay(enemyEyes.position, enemyEyes.forward * hit.distance, Color.green);
                shootPoint = hit.point;
                return true;
            }
        }
        else
        {
            Debug.DrawRay(enemyEyes.position, enemyEyes.forward * detectionRange, Color.red);
            shootPoint = ray.GetPoint(detectionRange + 20f);
            return false;
        }

        //RaycastHit hit;
        //if (Physics.Raycast(enemyEyes.position, enemyEyes.forward, out hit, detectionRange, whatCanEnemySee))
        //{
        //    if (hit.transform.CompareTag("Obstacle"))
        //    {
        //        Debug.DrawRay(enemyEyes.position, enemyEyes.forward * hit.distance, Color.red);
        //        shootPoint = hit.point;
        //        return false;
        //    }
        //    else
        //    {
        //        Debug.DrawRay(enemyEyes.position, enemyEyes.forward * hit.distance, Color.green);
        //        shootPoint = hit.point;
        //        return true;
        //    }
        //}
        //else
        //{
        //    Debug.DrawRay(enemyEyes.position, enemyEyes.forward * detectionRange, Color.red);

        //    return false;
        //}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
