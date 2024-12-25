using System;
using UnityEngine;

public class TaggleLoginController
{
    public static TaggleLoginController Api { get; set; }
    public Action<string> OnErrorEvent { get; set; }
    public Action<string> OnLoginEvent { get; set; }
    private string m_UserName;
    public string UserName => m_UserName;
    #region TestingOnly
    private const string VALID_USERNAME = "taggletester";
    private const string VALID_PASSWORD = "Aa123123!@#";
    #endregion

    public void DispatchLoginView()
    {
        SceneHelper.LoadScene(callback: () =>
        {
           GameObject.FindFirstObjectByType<TaggleLoginUserNameView>().Init();
        }, scenes: TaggleConstant.SCENE_LOGIN_USERNAME_PASSWORD);
    }

    public void LoadSceneLogin()
    {
        SceneHelper.LoadScene(callback: () =>
        {
            GameObject.FindFirstObjectByType<TaggleLoginView>().Init();
        }, scenes: TaggleConstant.SCENE_LOGIN);
    }

    public void UserLogin(UserInfoDTO userInfo)
    {
        if (string.IsNullOrEmpty(userInfo.GetUserName()) || string.IsNullOrEmpty(userInfo.GetPassword()))
        {
            OnErrorEvent?.Invoke("Please enter your username and password");
        }
        else if (!userInfo.GetUserName().Equals(VALID_USERNAME) || !userInfo.GetPassword().Equals(VALID_PASSWORD))
        {
            OnErrorEvent?.Invoke("Username or password is incorrect");
        }
        // Check valid username and password 
        else if (userInfo.GetUserName().Equals(VALID_USERNAME) && userInfo.GetPassword().Equals(VALID_PASSWORD))
        {
            m_UserName = userInfo.GetUserName();
            LoadSceneHomgePage();
        }
    }

    public void LoadSceneHomgePage()
    {
        SceneHelper.LoadScene(callback: () =>
        {
            GameObject.FindFirstObjectByType<TaggleHomePageView>().Init();
        }, scenes: TaggleConstant.SCENE_HOMEPAGE);
    }
}
