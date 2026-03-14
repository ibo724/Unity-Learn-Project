using UnityEngine;

public class BossRoomExit : MonoBehaviour
{
    public GameObject exitObject;
    public GameObject bossObject;

    void Start()
    {
        exitObject.SetActive(false);
    }

    void Update()
    {
        if (bossObject == null && exitObject != null)
            exitObject.SetActive(true);
    }
}