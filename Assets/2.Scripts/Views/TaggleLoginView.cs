using UnityEngine;
using UnityEngine.UI;

public class TaggleLoginView : MonoBehaviour
{
    private Button m_LoginBtn;

    public void Init()
    {
        m_LoginBtn = transform.Find("Body/Content/Login/BtnLogin").GetComponent<Button>();
        m_LoginBtn.onClick.AddListener(OnClickLoginBtn);
    }

    private void OnClickLoginBtn()
    {
        TaggleLoginController.Api.DispatchLoginView();
    }
}
