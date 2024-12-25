using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TaggleHomePageView : MonoBehaviour
{
    private TextMeshProUGUI m_TxtWelcome;
    private GameObject m_PrefItem;

    public void Init()
    {
        m_TxtWelcome = transform.Find("TopBar/TxtWelcome").GetComponent<TextMeshProUGUI>();
        m_PrefItem = transform.Find("Body/Viewport/Content/CategorySection/CategoryItem").gameObject;
        m_PrefItem.SetActive(false);
        InitView();

        TaggleController.Api.InitCategoryEvent += OnInitCategoryEvent;
        TaggleController.Api.InitHomePage();
    }

    private void OnDestroy()
    {
        TaggleController.Api.InitCategoryEvent -= OnInitCategoryEvent;
    }

    private void InitView()
    {
        m_TxtWelcome.text = $"Welcome {TaggleLoginController.Api.UserName}, \nHave a nice day";
    }

    private void OnInitCategoryEvent(List<HomePageCategoryDTO> categoryData)
    {
        if(categoryData != null) 
        {
            TaggleRenderUtils.RenderItems(categoryData, m_PrefItem, m_PrefItem.transform.parent, (data, go, action) =>
            {
                TaggleHomePageCategoryitemView viewItem = go.GetComponent<TaggleHomePageCategoryitemView>();
                viewItem.Init(data);
            });
        }
    }
}
