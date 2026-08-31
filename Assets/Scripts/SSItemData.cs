using UnityEngine;

[CreateAssetMenu(fileName = "SSItemData", menuName = "StackSell/SSItemData")]
public class SSItemData : ScriptableObject
{
    [SerializeField]
    private int sellPrice;

    public int SellPrice => sellPrice;
}
