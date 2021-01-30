using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Character Skin")]
public class CharacterSkin : ScriptableObject
{
    public string skinName;
    public RuntimeAnimatorController runtimeAnimator;
}
