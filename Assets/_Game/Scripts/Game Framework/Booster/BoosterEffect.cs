using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoosterEffect : ScriptableObject
{
    public float duration;
    public Sprite icon;
    [TextArea] public string effectDescription;


    public abstract void ApplyEffect(Character character);
    public abstract void RemoveEffect(Character character);
}
