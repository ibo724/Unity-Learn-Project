using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool doubleJumpUnlocked = false;
    public bool dashUnlocked = false;
    public Vector3 respawnPoint;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            respawnPoint = new Vector3(0, 0, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}