﻿using UnityEngine;

public class EnemyProjectileGun : MonoBehaviour
{
    // bullet
    public GameObject bullet;

    // bullet force
    public float shootForce, upwardForce;

    // gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShoots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    // bools
    public bool shooting, readyToShoot, reloading;

    // reference
    public Transform attackPoint;

    // graphics
    public GameObject muzzleFlash;

    // bug fixing
    public bool allowInvoke = true;

    public Transform mainTransform;

    public bool gunDropped;

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
}