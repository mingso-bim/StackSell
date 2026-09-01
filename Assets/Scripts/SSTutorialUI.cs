using System.Collections;
using UnityEngine;

public class SSTutorialUI : MonoBehaviour
{
    [SerializeField]
    private float displayDuration = 5f;

    private void Start()
    {
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        gameObject.SetActive(false);
    }
}
