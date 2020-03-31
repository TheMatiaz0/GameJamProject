﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyberevolver;
using Cyberevolver.Unity;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HotBarElement : MonoBehaviourPlus
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image back;
    [SerializeField]
    private Color selectBackColor;

    private Color baseBackColor;
    private bool isSelect;
    public Item Item { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        baseBackColor = back.color;
    }
    public void SetItem(Item item)
    {

        Item = item;
        icon.sprite = item?.Icon;
        text.text = item?.Name ?? "";
        icon.SetAlpha((item == null) ? 0: 1); ;

    }
    public void SetSelect(bool value)
    {
        if (value != isSelect)
        {
            back.color = (value) ? selectBackColor : baseBackColor;
            if(Item!=null)
            {
                if (value)
                    Item.OnStartSelect();
                else
                    Item.OnEndSelect();
            }
        }
           
    }
  
}