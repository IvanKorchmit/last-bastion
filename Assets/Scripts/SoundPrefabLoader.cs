using UnityEngine;

public class SoundPrefabLoader : MonoBehaviour
{
    [SerializeField] private GameObject soundSource;
    private void Start()
    {
        SoundManager.soundSource = soundSource;
        Destroy(gameObject);
    }
}