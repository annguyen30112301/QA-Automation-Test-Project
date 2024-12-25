public class UserInfoDTO
{
    private string m_Username = "";
    private string m_Password = "";

    public string GetUserName () => m_Username;
    public string GetPassword () => m_Password;

    public UserInfoDTO(string username, string password)
    {
        m_Username = username;
        m_Password = password;
    }
}
