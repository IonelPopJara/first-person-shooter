using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPosition : MonoBehaviour
{
    public Transform mainBone;
    public Vector3 offset;

    private void Start()
    {
        offset = transform.position;
    }

    private void Update()
    {
        transform.position = mainBone.localPosition + offset;
    }
}
