using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArenaSettings", menuName = "Game Settings/Arena Settings")]
public class ArenaSettings : ScriptableObject
{
    public Vector2 verticalLimits = new Vector2(-100, 100);
    public Vector2 horizontalLimits = new Vector2(-100, 100);
}
