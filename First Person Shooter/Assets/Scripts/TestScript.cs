using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform playerTarget;
    public float rotSpeed = 15f;

    private void Update()
    {
        if (!playerTarget) return;

        var lookPos = playerTarget.position - transform.position;
        //lookPos.y = 0;

        var targetRotation = Quaternion.LookRotation(lookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }
}
