using UnityEngine;

public class EnemyCubeHealth : MonoBehaviour
{
    [Header("Enemy Components")]
    public GameObject gfxRagdoll;
    public GameObject gfxNormal;
    public Transform gunPosition;

    [Header("Enemy Stats")]
    public int maxHealth;

    [Header("Debug")]
    public bool isDeath;
    public int currentHealth;

    private Rigidbody rb;
    private BoxCollider bc;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        isDeath = false;
        gfxRagdoll.SetActive(false);

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDeath)
        {
            isDeath = true;
            CubeDeath();
        }
    }

    private void CubeDeath()
    {
        Debug.Log($"{gameObject.name} died!");

        Destroy(rb);
        Destroy(bc);

        ActivateRagdoll();
        DropGun();
    }

    private void ActivateRagdoll()
    {
        gfxRagdoll.SetActive(true);
        gfxNormal.SetActive(false);
    }

    private void DropGun()
    {
        Rigidbody gun = gunPosition.gameObject.AddComponent<Rigidbody>();
        gun.mass = 10;
        gunPosition.parent = null;
    }

    public void TakeDamage(int damage)
    {
        if (!isDeath) currentHealth -= damage;
    }
}
