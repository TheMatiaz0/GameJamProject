﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cyberevolver.Unity;
using Cyberevolver;
using System;

public class Bridge : MonoBehaviourPlus
{
	public bool IsFixed { get { return _IsFixed; } private set { if (_IsFixed != value) { _IsFixed = value; RemoveTriggerOnFixed(); } } }
    private bool _IsFixed;

    public event EventHandler<SimpleArgs<Bridge>> OnBridgeBuilt = delegate { };

	[SerializeField]
	private uint needResourcesCount = 10;

    public uint ResourcesAddedToBuild { get { return _ResourcesAddedToBuild; } set { if (_ResourcesAddedToBuild != value) _ResourcesAddedToBuild = value; ChangeSprite(_ResourcesAddedToBuild); } }
    private uint _ResourcesAddedToBuild;

    [SerializeField]
    private SpriteRenderer spriteRender = null;

    [SerializeField]
    private Sprite fullyFixed = null;
    [SerializeField]
    private Sprite totallyBroken = null;
    [SerializeField]
    private Sprite underConstruction = null;

 
    // [field:SerializeField]
    // public Transform BulidPoint { get; private set; }

    [SerializeField]
    private BoxCollider2D triggerCollider = null;

    private void RemoveTriggerOnFixed ()
    {
        Destroy(triggerCollider);
    }

    protected void Start()
    {
        spriteRender.sprite = totallyBroken;
    }

    private void ChangeSprite (uint currentResources)
    {
        if (currentResources >= (needResourcesCount / 2))
        {
            spriteRender.sprite = underConstruction;
        }

        else if (currentResources == needResourcesCount)
        {
            spriteRender.sprite = fullyFixed;
            OnBridgeBuilt.Invoke(this, this);
            IsFixed = true;
        }
    }
}
