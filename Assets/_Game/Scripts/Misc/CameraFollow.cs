using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    private Vector3 offset;

    private void Start()
    {
        offset = followTarget.position - transform.position;
    }

    private void LateUpdate()
    {
        transform.position = followTarget.position - offset;
    }
}
