using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviour/*, IPunObservable*/
{
    Transform trans;
    [SerializeField] private float speed;
    private PhotonView view;
    private PlaySettings playSettings;
    private ArenaSettings arenaSettings;

    private void Awake()
    {
        playSettings = Resources.Load<PlaySettings>("Settings/PlaySettings");
        arenaSettings = Resources.Load<ArenaSettings>("Settings/ArenaSettings");
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(new Vector2 { x = transform.position.x, y = transform.position.y });
    //    }
    //    else
    //    {
    //        Vector2 position = (Vector2)stream.ReceiveNext();
    //        transform.position = new Vector3 { x = position.x, y = position.y };
    //    }
    //}

    void Start()
    {
        trans = GetComponent<Transform>();
        view = GetComponent<PhotonView>();

        if (view.IsMine == false)
            return;

        GetComponent<SpriteRenderer>().color = Color.green;

        FindObjectOfType<CameraFollow>().Target = trans;
    }

    void Update()
    {
        if (view.IsMine == false)
            return;

        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 offset = new Vector3
        {
            x = horizontal * playSettings.speed * Time.deltaTime,
            y = vertical * playSettings.speed * Time.deltaTime
        };

        if (trans.position.x + offset.x > arenaSettings.horizontalLimits.y || trans.position.x + offset.x < arenaSettings.horizontalLimits.x)
            offset.x = 0;

        if (trans.position.y + offset.y > arenaSettings.verticalLimits.y || trans.position.y + offset.y < arenaSettings.verticalLimits.x)
            offset.y = 0;

        trans.position += offset;
    }
}
