using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public int health = 1;
    public bool isDeath;

    public GameObject GFXRagdoll;
    public GameObject GFXNormal;

    public Rigidbody rb;
    public BoxCollider bc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        isDeath = false;
        GFXRagdoll.SetActive(false);
    }

    private void Update()
    {
        if(health <= 0 && !isDeath)
        {
            isDeath = true;
            CubeDeath();
        }
    }

    private void ToggleRagdoll(bool active)
    {
        GFXRagdoll.SetActive(active);
        GFXNormal.SetActive(!active);

        bc.enabled = !active;
    }

    private void CubeDeath()
    {
        Debug.Log("Cube died");
        Destroy(rb);
        Destroy(bc);
        ToggleRagdoll(true);
    }
}
