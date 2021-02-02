using UnityEngine;

public class RagdollDamage : MonoBehaviour, IDamageable
{
    public EnemyCubeHealth cubeHealth;

    public void TakeDamage(int damage)
    {
        cubeHealth.TakeDamage(damage);
        //Debug.Log($"{gameObject.name} has taken damage");
    }
}
