using UnityEngine;

// 항상 활성 상태인 Canvas에 부착한다.
// 비활성 상태의 GameClearOverlay를 참조해서, 두 업그레이드가 모두 MAX가 되는 순간 Overlay를 켠다.
[DefaultExecutionOrder(200)]
public class SSGameClearUI : MonoBehaviour
{
    [SerializeField]
    private SSPlayerUpgrade playerUpgrade;

    [SerializeField]
    private SSSaveSystem saveSystem;

    [SerializeField]
    private GameObject gameClearOverlay;

    private bool clearHandled;

    private void Start()
    {
        // 이미 클리어한 세이브라면 다시 띄우지 않는다.
        clearHandled = saveSystem.GameCleared;

        gameClearOverlay.SetActive(false);
    }

    private void Update()
    {
        if (clearHandled)
            return;

        if (!playerUpgrade.IsAllUpgradesMaxed)
            return;

        clearHandled = true;

        saveSystem.MarkGameCleared();
        gameClearOverlay.SetActive(true);
    }

    // GameClearOverlay의 ContinueButton OnClick에 연결한다.
    public void OnContinuePressed()
    {
        gameClearOverlay.SetActive(false);
    }
}
