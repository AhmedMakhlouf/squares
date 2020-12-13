using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Player : MonoBehaviour, IPunObservable
{

    private Image healthBar;
    private int health = 3;
    private PhotonView view;
    private bool shield = true;
    private SpriteRenderer visuals;
    private float shieldTimer = 3.0f;
    private bool visualsBlining = false;
    private PlaySettings playSettings;
    private ArenaSettings arenaSettings;
    private GameObject killEffectPrefab;

    public int Health {
        set
        {
            health = value;

            if(view.IsMine)
            {
                ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
                hash.Add("Health", health);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }

            healthBar.fillAmount = (float)health / playSettings.initialHealth;

            if(visuals.enabled == false && health > 0)
            {
                visuals.enabled = true;
            }

            if (health > 0)
                return;

            if (PhotonNetwork.IsMasterClient)
                this.view.RPC("Spawn", RpcTarget.All);
        }
        get
        {
            return health;
        }
    }

    public bool Shield
    {
        set
        {
            shield = value;

            if (shield == true && visualsBlining == false)
            {
                StartCoroutine("BlinkVisuals");
            }
            
            if(shield == false && visualsBlining == true)
            {
                StopCoroutine("BlinkVisuals");
                visuals.enabled = true;
                visualsBlining = false;
            }
        }
        get
        {
            return shield;
        }
    }
    

    void Awake()
    {
        healthBar = GetComponentInChildren<Image>();
        view = GetComponent<PhotonView>();
        visuals = GetComponent<SpriteRenderer>();

        playSettings = Resources.Load<PlaySettings>("Settings/PlaySettings");
        arenaSettings = Resources.Load<ArenaSettings>("Settings/ArenaSettings");
        killEffectPrefab = Resources.Load<GameObject>("KillEffect");
    }

    private void Start()
    {
        if (view.IsMine == false)
        {
            if(view.Owner.CustomProperties.ContainsKey("Health") == false)
            {
                Health = playSettings.initialHealth;
                Shield = true;
            }
            else
            {
                Health = (int)view.Owner.CustomProperties["Health"];
            }

            return;
        }

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("Health", playSettings.initialHealth);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        Shield = true;
    }

    private void Update()
    {
        if (view.IsMine == false)
            return;

        if(shieldTimer > 0)
        {
            shieldTimer -= Time.deltaTime;

            if (shieldTimer <= 0)
            {
                Shield = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // TODO: Consider using RPCs for synchronizing the shield
        if (stream.IsWriting)
        {
            stream.SendNext(Shield);
        }
        else
        {
            Shield = (bool)stream.ReceiveNext();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Shield == true || collision.GetComponent<Player>().Shield == true)
            return;

        // This is a client-side prediction to fake an instant response
        float predictedHealth = Health - playSettings.damage;
        
        // Comment this out for to disable client-prediction
        healthBar.fillAmount = (float)predictedHealth / playSettings.initialHealth;

        // Comment this out for to disable client-prediction
        if (predictedHealth == 0) 
            visuals.enabled = false;

        // TODO: Hanlde client-side misprediction by overwriting from the server data


        // Force authoritatize server for Hit decision
        if (PhotonNetwork.IsMasterClient)
        {
            this.view.RPC("Hit", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void Hit()
    {
        Health -= playSettings.damage;
    }

    [PunRPC]
    private void ResetHealth()
    {
        Health = playSettings.initialHealth;
    }

    [PunRPC]
    private void Spawn()
    {
        Vector3 randomPosition = new Vector3
        {
            x = Random.Range(arenaSettings.horizontalLimits.x, arenaSettings.horizontalLimits.y),
            y = Random.Range(arenaSettings.verticalLimits.x, arenaSettings.verticalLimits.y)
        };

        while(Vector3.Distance(randomPosition, transform.position) < 3.0f)
        {
            randomPosition = new Vector3
            {
                x = Random.Range(arenaSettings.horizontalLimits.x, arenaSettings.horizontalLimits.y),
            y = Random.Range(arenaSettings.verticalLimits.x, arenaSettings.verticalLimits.y)
            };
        }

        // TODO: Use object pooling instead of spawning new objects, or force single particle system per player
        Instantiate(killEffectPrefab, transform.position, Quaternion.identity);


        Shield = true;
        shieldTimer = 3.0f;

        transform.position = randomPosition;


        Health = playSettings.initialHealth;
    }

    IEnumerator BlinkVisuals()
    {
        visualsBlining = true;
        while(true)
        {
            yield return new WaitForSeconds(0.15f);
            visuals.enabled = !visuals.enabled;
        }
    }
}
