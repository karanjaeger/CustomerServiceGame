using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TextScriptableObject", menuName = "ScriptableObject/TextScriptableObject")]
public class TextScriptableObject : ScriptableObject
{
    public string[] playerTexts;
    public string[] customerTexts;
    public string[] options;   
}
