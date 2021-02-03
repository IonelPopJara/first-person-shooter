using UnityEngine;

public class EnemyGunRotation : MonoBehaviour
{
    public float rotSpeed = 15f;
    
    Transform _playerTarget;
    bool _isDropped;

    // Set and Getter for Player Target
    public Transform PlayerTarget
    {
        get => _playerTarget;
        set => _playerTarget = value;
    }

    public bool IsDropped
    {
        get => _isDropped;
        set => _isDropped = value;
    }

    private void Update()
    {
        if (_isDropped) return;

        if (!_playerTarget) return;

        var lookPos = _playerTarget.position - transform.position;

        var targetRotation = Quaternion.LookRotation(lookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }
}
