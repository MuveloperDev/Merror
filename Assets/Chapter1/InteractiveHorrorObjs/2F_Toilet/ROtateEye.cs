using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROtateEye : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private void FixedUpdate() => transform.LookAt(target.transform.position);
}
