using UnityEngine;

public class CustomEnemyBullet : MonoBehaviour
{
    // assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    // stats
    [Range(0f, 1f)] public float bounciness;
    public bool useGravity;

    // damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    // lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        // when to explode
        if (collisions > maxCollisions) Explode();

        // count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }

    private void Explode()
    {
        // instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            // get component of enemy and call take damage
            if (enemies[i].GetComponent<PlayerHealth>())
                enemies[i].GetComponent<PlayerHealth>().TakeDamage(explosionDamage);

            // get component of enemy and call take damage
            if (enemies[i].GetComponent<EnemyCubeHealth>())
                enemies[i].GetComponent<EnemyCubeHealth>().TakeDamage(explosionDamage);

            Debug.Log($"{enemies[i].transform.name} damaged");

            if (enemies[i].GetComponent<Rigidbody>())
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        // add a little delay, just to make sure everything works fine
        Invoke(nameof(Delay), 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // count up collisions
        collisions++;

        // explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }

    private void Setup()
    {
        // create a new physics material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Maximum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        // assing material to collider
        GetComponent<SphereCollider>().material = physics_mat;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
