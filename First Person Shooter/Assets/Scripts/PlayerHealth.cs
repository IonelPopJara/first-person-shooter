using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Player Life Stats")]
    public int maxHealth;
    public int currentHealth;
    public bool isDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player damaged");
        if (currentHealth <= 0) return;
        currentHealth -= damage;
    }
}
