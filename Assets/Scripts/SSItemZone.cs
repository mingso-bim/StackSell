using System.Collections;
using UnityEngine;

public class SSItemZone : MonoBehaviour
{
    [SerializeField]
    private SSItem itemPrefab;

    [SerializeField]
    private float productionInterval = 0.2f;

    private Coroutine productionCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning($"{name}: itemPrefab이 설정되지 않아 아이템을 생성할 수 없습니다.");
            return;
        }

        if (productionCoroutine != null)
            return;

        SSPlayerCollector collector = other.GetComponent<SSPlayerCollector>();

        if (collector == null)
            return;

        productionCoroutine = StartCoroutine(ProduceItems(collector));
    }

    private void OnTriggerExit(Collider other)
    {
        SSPlayerCollector collector = other.GetComponent<SSPlayerCollector>();

        if (collector == null)
            return;

        if (productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            productionCoroutine = null;
        }
    }

    private IEnumerator ProduceItems(SSPlayerCollector collector)
    {
        while (true)
        {
            if (!collector.IsFull())
            {
                SSItem item = Instantiate(itemPrefab);

                collector.Collect(item);
            }

            yield return new WaitForSeconds(productionInterval);
        }
    }
}
