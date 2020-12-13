using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KillEffect : MonoBehaviour
{ 
    void Start()
    {
        Destroy(gameObject, 2.0f);
    }
}
