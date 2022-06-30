using UnityEngine;

public class ReferenceCollection : MonoBehaviour
{
    public static ReferenceCollection Instance;

    public Cursor cursor;
    public SpriteCollection spriteCollection;
    public GameplayConfigs configs;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}