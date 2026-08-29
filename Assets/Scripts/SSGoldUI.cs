using TMPro;
using UnityEngine;

public class SSGoldUI : MonoBehaviour
{
    [SerializeField]
    private SSPlayerWallet wallet;

    [SerializeField]
    private TMP_Text goldText;

    private int displayedGold = -1;

    private void Update()
    {
        if (wallet == null)
        {
            Debug.LogWarning($"{name}: wallet이 설정되지 않아 Gold를 표시할 수 없습니다.");
            return;
        }

        if (goldText == null)
        {
            Debug.LogWarning($"{name}: goldText가 설정되지 않아 Gold를 표시할 수 없습니다.");
            return;
        }

        if (wallet.Gold == displayedGold)
            return;

        displayedGold = wallet.Gold;

        goldText.text = $"Gold: {displayedGold}";
    }
}
