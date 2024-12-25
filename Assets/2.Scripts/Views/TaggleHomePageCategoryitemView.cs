using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaggleHomePageCategoryitemView : MonoBehaviour
{
    private RawImage m_RImgIcon;
    private TextMeshProUGUI m_Title;
    private HomePageCategoryDTO m_CategoryDTO;

    public void Init(HomePageCategoryDTO data)
    {
        m_CategoryDTO = data;
        m_RImgIcon = transform.Find("Icon").GetComponent<RawImage>();
        m_Title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        InitView();
    }

    private void InitView()
    {
        m_RImgIcon.texture = Resources.Load(m_CategoryDTO.Icon) as Texture2D;
        m_Title.text = m_CategoryDTO.Title;
    }
}
