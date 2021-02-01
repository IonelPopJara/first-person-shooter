using UnityEngine;

public class EnemyCubeHealth : MonoBehaviour
{
    [Header("Enemy Components")]
    public EnemyProjectileGun enemyGun;

    [Header("Enemy Stats")]
    public int maxHealth;

    [Header("Debug")]
    public bool isDeath;
    public int currentHealth;

    public Rigidbody[] childrenRigidbodies;


    private void Awake()
    {
        childrenRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        isDeath = false;

        currentHealth = maxHealth;

        foreach (var rigidbody in childrenRigidbodies)
            rigidbody.freezeRotation = true;
    }

    public void TakeDamage(int damage)
    {
        if (isDeath) return;
        
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDeath = true;
            CubeDeath();
        }
    }

    public void CubeDeath()
    {
        Debug.Log($"{gameObject.name} died!");

        DropGun();

        ActivateRagdoll();
    }

    private void ActivateRagdoll()
    {
        foreach (var rigidbody in childrenRigidbodies)
            rigidbody.freezeRotation = false;
    }

    public void DropGun()
    {
        enemyGun.DropGun();

        //Debug.Log($"{gameObject.name} dropped!");

        enemyGun.GetComponentInChildren<MeshCollider>().enabled = true;

        Rigidbody gunRigidbody = enemyGun.gameObject.AddComponent<Rigidbody>();
        gunRigidbody.mass = 10;

        enemyGun.transform.parent = null;
    }
}
