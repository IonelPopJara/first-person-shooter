using UnityEngine;

public class TestScript : MonoBehaviour
{
    public EnemyCubeHealth cubeHealth;

    public void TakeDamage()
    {
        cubeHealth.TakeDamage(1);
        Debug.Log($"{gameObject.name} has taken damage");
    }
}
