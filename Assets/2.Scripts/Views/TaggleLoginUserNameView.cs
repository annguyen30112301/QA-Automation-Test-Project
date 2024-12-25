using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class TaggleLoginUserNameView : MonoBehaviour
{
    private Button m_BtnBack;
    private Button m_BtnHintPassword;
    private Button m_BtnLogin;
    private TMP_InputField m_IpfEmail;
    private TMP_InputField m_IpfPassword;
    private GameObject m_GoIconHintPassOn;
    private GameObject m_GoIconHintPassOff;
    private TextMeshProUGUI m_TxtError;

    public void Init()
    {
        m_BtnBack = transform.Find("TopBar/BtnBack").GetComponent<Button>();
        m_BtnHintPassword = transform.Find("Body/Content/Detail/Password/IpfPassword/TextArea/BtnHint").GetComponent<Button>();
        m_IpfEmail = transform.Find("Body/Content/Detail/Email/IpfEmail").GetComponent<TMP_InputField>();
        m_IpfPassword = transform.Find("Body/Content/Detail/Password/IpfPassword").GetComponent<TMP_InputField>();
        m_GoIconHintPassOn = m_BtnHintPassword.transform.Find("IconOn").gameObject;
        m_GoIconHintPassOff = m_BtnHintPassword.transform.Find("IconOff").gameObject;
        m_TxtError = transform.Find("Body/Content/Detail/TxtError").GetComponent<TextMeshProUGUI>();
        m_BtnLogin = transform.Find("Body/Content/Detail/Login/BtnLogin").GetComponent<Button>();

        m_BtnBack.onClick.AddListener(BackOnClick);
        m_BtnHintPassword.onClick.AddListener(HintPasswordOnClick);
        m_BtnLogin.onClick.AddListener(LoginOnClick);
        ClearError();
        UpdateHintView(m_IpfPassword.contentType);
        TaggleLoginController.Api.OnErrorEvent += OnErrorHandler;
    }

    private void OnDestroy()
    {
        TaggleLoginController.Api.OnErrorEvent -= OnErrorHandler;
    }

    private void BackOnClick()
    {
        TaggleLoginController.Api.LoadSceneLogin();
    }

    private void HintPasswordOnClick()
    {
        TMP_InputField.ContentType newContentType = m_IpfPassword.contentType == TMP_InputField.ContentType.Password ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        UpdateHintView(newContentType);
    }

    private void OnErrorHandler(string content)
    {
        m_TxtError.text = content;

        ResetButtons();
    }

    private void UpdateHintView(TMP_InputField.ContentType contentType)
    {
        m_IpfPassword.contentType = contentType;
        m_IpfPassword.ForceLabelUpdate();
        //
        m_GoIconHintPassOn.SetActive(contentType == TMP_InputField.ContentType.Password);
        m_GoIconHintPassOff.SetActive(contentType == TMP_InputField.ContentType.Standard);
    }

    private void ClearError()
    {
        OnErrorHandler(string.Empty);
    }

    private void ResetButtons()
    {
        m_BtnHintPassword.interactable = true;
        m_BtnLogin.interactable = true;
    }

    private void LoginOnClick()
    {
        m_BtnLogin.interactable = false;

        string username = m_IpfEmail.text;
        string password = m_IpfPassword.text;

        ClearError();

        UserInfoDTO userInfoDTO = new(username, password);
        TaggleLoginController.Api.UserLogin(userInfoDTO);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        try
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                m_IpfEmail.text = "taggletester";
                m_IpfPassword.text = "Aa123123!@#";
                UserInfoDTO userInfoDTO = new(m_IpfEmail.text, m_IpfPassword.text);
                TaggleLoginController.Api.UserLogin(userInfoDTO);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
#endif
    }
}
