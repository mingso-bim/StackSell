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
}
