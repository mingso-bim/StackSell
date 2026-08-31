using UnityEngine;

public class SSPlayerWallet : MonoBehaviour
{
    [SerializeField]
    private int gold;

    public int Gold => gold;

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public bool TrySpendGold(int amount)
    {
        if (gold < amount)
            return false;

        gold -= amount;

        return true;
    }
}
