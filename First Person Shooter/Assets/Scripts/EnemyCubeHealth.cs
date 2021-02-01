using UnityEngine;

public class EnemyCubeHealth : MonoBehaviour
{
    [Header("Enemy Components")]
    public Transform gunPosition;
    public EnemyProjectileGun gun;

    [Header("Enemy Stats")]
    public int maxHealth;

    [Header("Debug")]
    public bool isDeath;
    public int currentHealth;

    public Rigidbody[] childrenRigidbodies;
    private BoxCollider bc;


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

    private void Update()
    {
        if (currentHealth <= 0 && !isDeath)
        {
            isDeath = true;
            gun.gunDropped = true;
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

    private void DropGun()
    {
        Debug.Log($"gun dropped!");
        gunPosition.GetComponentInChildren<MeshCollider>().enabled = true;
        Rigidbody gun = gunPosition.gameObject.AddComponent<Rigidbody>();
        gun.mass = 10;
        gunPosition.parent = null;
    }

    public void TakeDamage(int damage)
    {
        if (!isDeath) currentHealth -= damage;
    }
}
