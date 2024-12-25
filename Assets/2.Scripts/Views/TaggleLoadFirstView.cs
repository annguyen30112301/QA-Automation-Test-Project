using UnityEngine;
using DG.Tweening;

public class TaggleLoadFirstView : MonoBehaviour
{
    void Start()
    {
        TaggleController.Api.Init();
        DOVirtual.DelayedCall(1f, () => 
            { 
                SceneHelper.LoadScene(callback: () =>
                {
                    FindFirstObjectByType<TaggleLoginView>().Init();
                }, 
                scenes: TaggleConstant.SCENE_LOGIN);
            });
    }
}
