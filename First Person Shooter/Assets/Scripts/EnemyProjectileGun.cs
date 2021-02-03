using UnityEngine;

public class EnemyProjectileGun : MonoBehaviour
{
    [Header("Bullet Prefab")]
    public GameObject bullet;

    [Header("Bullet Force Settings")]
    public float shootForce, upwardForce;

    [Header("Gun Stats")]
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShoots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    // bools
    private bool shooting, readyToShoot, reloading;

    [Header("References")]
    public Transform attackPoint;

    [Header("Graphics")]
    public GameObject muzzleFlash;

    [Header("Debug")]
    public bool allowInvoke = true;
    public bool gunDropped;
    public Vector3 targetPoint;

    private void Awake()
    {
        // make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        if (gunDropped) return;

        EnemyInput();
    }

    private void EnemyInput()
    {
        // reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        // shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            // set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = attackPoint.forward;
        //Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        // calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); // add spread to the last direction

        // instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        // rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        // add froces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(attackPoint.transform.up * upwardForce, ForceMode.Impulse);

        // instantiate muzzle flash if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked)
        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
        }

        // if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShoots);
    }

    private void ResetShot()
    {
        // allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public void DropGun()
    {
        gunDropped = true;
        transform.GetComponent<EnemyGunRotation>().IsDropped = true;
    }

    public void SetShooting(bool shootingValue)
    {
        shooting = shootingValue;
    }

    public void SetTargetPoint(Vector3 _targetPoint)
    {
        targetPoint = _targetPoint;
    }
}
