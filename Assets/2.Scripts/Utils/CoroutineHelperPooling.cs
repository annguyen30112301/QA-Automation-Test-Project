using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelperPooling : MonoBehaviour
{
    #region Singleton
    private static CoroutineHelperPooling m_instance;
    public static CoroutineHelperPooling Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject clone = new GameObject("Coroutine Helper Pooling");
                m_instance = clone.AddComponent<CoroutineHelperPooling>();
            }

            return m_instance;
        }
    }
    #endregion

    private Queue<CoroutineHelper> m_coroutineHelperPooling = new Queue<CoroutineHelper>();

    public CoroutineHelper GetCoroutineHelperFromPool()
    {
        if (m_coroutineHelperPooling.Count == 0)
        {
            GameObject clone = new GameObject("coroutine");
            clone.transform.SetParent(transform);
            CoroutineHelper coroutineHelper = clone.AddComponent<CoroutineHelper>();
            return coroutineHelper;
        }

        return m_coroutineHelperPooling.Dequeue();
    }

    public void ReturnToPool(CoroutineHelper coroutineHelper)
    {
        m_coroutineHelperPooling.Enqueue(coroutineHelper);
    }
}
