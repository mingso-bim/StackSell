using UnityEngine;

// TutorialOverlay에 부착. 전체화면 투명 Image + Button이 터치를 받고,
// Button OnClick에서 CompleteTutorial()를 호출한다.
public class SSTutorialUI : MonoBehaviour
{
    private const string TutorialDoneKey = "SS_TutorialDone";

    [SerializeField] 
    private GameObject gameUIRoot;

    private void Awake()
    {
        // 씬 시작 시 표시 여부를 결정한다. 이미 완료했다면 Overlay를 표시하지 않는다.
        if (PlayerPrefs.GetInt(TutorialDoneKey, 0) == 1)
            gameObject.SetActive(false);
        else
            gameUIRoot.SetActive(false);
    }

    // TutorialOverlay의 Button OnClick에 연결한다.
    public void CompleteTutorial()
    {
        PlayerPrefs.SetInt(TutorialDoneKey, 1);
        PlayerPrefs.Save();

        gameObject.SetActive(false);
        gameUIRoot.SetActive(true);
    }
}
