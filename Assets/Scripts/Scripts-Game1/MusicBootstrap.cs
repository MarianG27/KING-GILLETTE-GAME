using UnityEngine;

public class MusicBootstrap : MonoBehaviour
{
    [SerializeField] private MusicManager musicManagerPrefab;

    void Awake()
    {
        if (MusicManager.Instance == null)
        {
            Instantiate(musicManagerPrefab);
        }
    }
}