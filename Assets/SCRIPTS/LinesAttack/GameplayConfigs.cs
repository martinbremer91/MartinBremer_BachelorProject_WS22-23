using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Configs", fileName = "GameplayConfigs")]
public class GameplayConfigs : ScriptableObject
{
    public Color[] colors;
    public int activeColorsNumber = 3;
    public float sqRotateDuration;
}
