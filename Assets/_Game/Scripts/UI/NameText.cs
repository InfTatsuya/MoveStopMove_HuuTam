using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameText : MonoBehaviour
{
    private Quaternion startRotation;

    private void Start()
    {
        startRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = startRotation;
    }
}
