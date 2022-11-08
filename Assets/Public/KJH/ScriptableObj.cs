using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ClipsDataObj", menuName = "ScriptableObjects/AudioClipsDataObj", order = 1)]

public class ScriptableObj : ScriptableObject
{
    public AudioClip[] Sounds;
}
