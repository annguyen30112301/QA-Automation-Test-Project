using UnityEngine;
using System;
using System.Collections.Generic;

public class TaggleController 
{
    public static TaggleController Api { get; set; } = new TaggleController();

    public Action<List<HomePageCategoryDTO>> InitCategoryEvent;

    public void Init()
    {
        TaggleLoginController.Api = new TaggleLoginController();
    }

    public void InitHomePage()
    {
        InitCategoryEvent?.Invoke(GetMockupData());
    }

    private List<HomePageCategoryDTO> GetMockupData()
    {
        List<HomePageCategoryDTO> dtos = new();

        HomePageCategoryDTO dto1 = new()
        {
            Id = "0",
            Title = "Covid",
            Icon = "Images/HomePage/covid"
        };

        HomePageCategoryDTO dto2 = new()
        {
            Id = "0",
            Title = "Doctor",
            Icon = "Images/HomePage/doctor"
        };

        HomePageCategoryDTO dto3 = new()
        {
            Id = "0",
            Title = "Hospital",
            Icon = "Images/HomePage/hospital"
        };

        HomePageCategoryDTO dto4 = new()
        {
            Id = "0",
            Title = "Medicine",
            Icon = "Images/HomePage/medicine"
        };

        dtos.Add(dto1);
        dtos.Add(dto2);
        dtos.Add(dto3);
        dtos.Add(dto4); 

        return dtos;
    }
}
