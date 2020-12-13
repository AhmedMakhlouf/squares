using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target { get; set; }
    private Transform trans;

    void Start()
    {
        trans = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        trans.position = Vector3.Lerp(trans.position, Target.position, Vector3.Distance(trans.position, Target.position) * Time.deltaTime);
    }
}
