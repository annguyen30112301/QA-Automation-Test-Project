using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public static class TaggleRenderUtils
{
    private const int CONST_MAX_RENDER_TASK_SLOT = 20;

    public static void RenderItems<T>(List<T> datas, GameObject prefab, Transform parent, Action<T, GameObject, Action> onUpdateEvent, Action callback = null, bool enableAnimation = true)
    {
        if (datas == null)
        {
            callback?.Invoke();
            return;
        }

        CoroutineHelper.Call(IERenderItems(datas, prefab, parent, onUpdateEvent, callback, enableAnimation));
    }

    public static IEnumerator IERenderItems<T>(List<T> datas, GameObject prefab, Transform parent, Action<T, GameObject, Action> onUpdateEvent, Action callback = null, bool enableAnimation = true)
    {
        if (datas != null && datas.Count > 0)
        {
            int tasks = datas.Count;
            int availableTaskSlot = CONST_MAX_RENDER_TASK_SLOT;

            foreach (T item in datas)
            {
                if (availableTaskSlot <= 0)
                {
                    yield return new WaitUntil(() => availableTaskSlot > 0);
                }

                // Decrease available task slot by one
                availableTaskSlot--;

                RenderItem(
                    prefab, parent,
                    onUpdateEvent: (goItem, onComplete) =>
                    {
                        onUpdateEvent?.Invoke(item, goItem, () => {
                            onComplete?.Invoke();
                        });
                    },
                    callback: () =>
                    {
                        tasks--;
                        availableTaskSlot++;
                    },
                    enableAnimation: enableAnimation
                );
            }

            yield return new WaitUntil(() => tasks <= 0);
        }

        callback?.Invoke();
    }

    public static void RenderItem(GameObject prefab, Transform parent, Action<GameObject, Action> onUpdateEvent, Action callback = null, bool enableAnimation = true)
    {
        GameObject go = GameObject.Instantiate(prefab, parent);

        CanvasGroup cvgItem = go.GetComponent<CanvasGroup>();
        if (cvgItem == null) cvgItem = go.AddComponent<CanvasGroup>();

        go.SetActive(true);

        if (enableAnimation)
        {
            onUpdateEvent?.Invoke(go, () => {
                float curAlpha = cvgItem.alpha;
                cvgItem.alpha = 0f;
                cvgItem?.DOFade(curAlpha, 0.6f).SetDelay(0.3f).OnComplete(() =>
                {
                    callback?.Invoke();
                });
            });
        }
        else
        {
            onUpdateEvent?.Invoke(go, callback);
        }
    }


    public static void ClearItems<T>(Transform tfContent, Action callback = null, bool includeInactive = false, bool enableAnimation = false, params GameObject[] ignoreDestroy)
    {
        CoroutineHelper.Call(IEClearItemsWithAnimation<T>(
            callback: callback,
            tfContent: tfContent,
            includeInactive: includeInactive,
            enableAnimation: enableAnimation,
            ignoreDestroy: ignoreDestroy
        ));
    }

    private static IEnumerator IEClearItemsWithAnimation<T>(Action callback, Transform tfContent, bool includeInactive, bool enableAnimation, GameObject[] ignoreDestroy)
    {
        T[] targets = tfContent.GetComponentsInChildren<T>(includeInactive: includeInactive)?.Where(x => (x as Component).transform.parent == tfContent)?.ToArray() ?? new T[0];
        int tasks = targets.Length;

        Debug.Log($"Start ClearItems: {tasks}");
        foreach (T item in targets)
        {
            Component comp = item as Component;

            bool destroyable = comp && comp.gameObject != tfContent.gameObject && !ignoreDestroy.Contains(comp.gameObject);

            if (destroyable)
            {
                if (enableAnimation)
                {
                    GetOrAddComponent<CanvasGroup>(comp.gameObject)
                        .DOFade(0f, 0.3f).OnComplete(() =>
                        {
                            GameObject.Destroy(comp.gameObject);
                            tasks--;
                        });
                }
                else
                {
                    GameObject.Destroy(comp.gameObject);
                    tasks--;
                }
            }
            else
            {
                tasks--;
            }
        }

        if (callback != null)
        {
            yield return new WaitUntil(() => tasks <= 0);
            callback.Invoke();

            Debug.Log("Complete ClearItems");
        }
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        if (go?.GetComponent<T>() ?? false)
            return go?.GetComponent<T>() ?? null;
        else
            return go?.AddComponent<T>() ?? null;
    }
}
