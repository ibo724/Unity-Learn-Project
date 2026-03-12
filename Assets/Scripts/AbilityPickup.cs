using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public enum AbilityType { DoubleJump, Dash }
    public AbilityType abilityType;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (abilityType == AbilityType.DoubleJump)
            GameManager.Instance.doubleJumpUnlocked = true;
        else if (abilityType == AbilityType.Dash)
            GameManager.Instance.dashUnlocked = true;

        Destroy(gameObject);
    }
}