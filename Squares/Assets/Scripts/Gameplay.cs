using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Gameplay : MonoBehaviourPunCallbacks
{
    private GameObject playerPrefab;


    private void Awake()
    {
        playerPrefab = Resources.Load<GameObject>("Player");
    }

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}
