using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaySettings", menuName = "Game Settings/Play Settings")]
public class PlaySettings : ScriptableObject
{
    public int initialHealth = 3;
    public int damage = 1;
    public float speed = 10;
}
