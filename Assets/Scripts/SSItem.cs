using UnityEngine;

public class SSItem : MonoBehaviour
{
    [SerializeField]
    private int sellPrice;

    public int SellPrice => sellPrice;
}
