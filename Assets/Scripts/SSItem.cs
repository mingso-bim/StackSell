using UnityEngine;

public class SSItem : MonoBehaviour
{
    [SerializeField]
    private SSItemData itemData;

    public int SellPrice
    {
        get
        {
            if (itemData == null)
            {
                Debug.LogWarning($"{name}: itemData가 설정되지 않아 SellPrice로 0을 반환합니다.");
                return 0;
            }

            return itemData.SellPrice;
        }
    }
}
