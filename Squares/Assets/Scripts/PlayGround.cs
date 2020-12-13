using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGround : MonoBehaviour
{
    void Awake()
    {
        var arenaSettings = Resources.Load<ArenaSettings>("Settings/ArenaSettings");
        var trans = GetComponent<Transform>();

        Vector3 position = new Vector3
        {
            x = (arenaSettings.horizontalLimits.x + arenaSettings.horizontalLimits.y) / 2.0f,
            y = (arenaSettings.verticalLimits.x + arenaSettings.verticalLimits.y) / 2.0f,
            z = 10.0f
        };

        Vector3 extent = new Vector3
        {
            x = Mathf.Abs(arenaSettings.horizontalLimits.x) + Mathf.Abs(arenaSettings.horizontalLimits.y),
            y = Mathf.Abs(arenaSettings.verticalLimits.x) + Mathf.Abs(arenaSettings.verticalLimits.y),
            z = 1.0f
        };

        trans.position = position;
        trans.localScale = extent;
    }
    
}
