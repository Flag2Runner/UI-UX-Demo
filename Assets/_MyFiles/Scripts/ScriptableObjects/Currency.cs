using UnityEngine;

[CreateAssetMenu(fileName = "New Currency", menuName = "Economy/Currency")]
public class Currency : ScriptableObject
{
    public string displayName;
    public Sprite icon;
}