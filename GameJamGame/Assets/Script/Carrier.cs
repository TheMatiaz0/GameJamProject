﻿using Cyberevolver.Unity;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviourPlus, IHpable
{
    [SerializeField]
    private uint maxCapacity = 8;

    public uint CurrentResources { get; private set; }

    public Team CurrentTeam { get; private set; } = Team.Good;

    public Hp Hp { get; private set; }
    [Auto]
    public AIPath AIPath { get; private set; }


    [SerializeField]
    private Transform resourcesPoint = null;
    [SerializeField, RequiresAny]
    private Resource first;
    public Resource Current { get; private set; }

    [SerializeField]
    private uint startMaxHp;
    public bool IsLaunched { get; private set; }

    private Coroutine logic = null;

    private Bridge selectedBridge;

    [SerializeField]
    private float speed = 10;

    private Transform currentTarget = null;

    private bool bridgeOnTrigger = false;

    protected void Start()
    {
        Hp = new Hp(startMaxHp, 0, startMaxHp);
        Hp.OnValueChangeToMin += Hp_OnValueChangeToMin;
    }

    protected override void Awake()
    {
        base.Awake();
        Current = first;
    }

    [Button]
    public bool Launch()
    {
        if (IsLaunched)
            return false;
        if (BridgeSelection.SelectedBridge is null || BridgeSelection.SelectedBridge.IsFixed)
            return false;
        IsLaunched = true;
        selectedBridge = BridgeSelection.SelectedBridge;
        logic = StartCoroutine(Run());

        return true;
    }

    protected void Update()
    {
        if (currentTarget == null)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        //rb2D.MovePosition((Vector2)transform.position + (Vector2)currentTarget.position * speed * Time.fixedDeltaTime);
    }

    private IEnumerator Run()
    {
        yield return GoToResources();

        // gather resources
        yield return GatherResources();

        yield return GoPoints();
        currentTarget = null;

        yield return GoToBridge();

        // fix the bridge
        yield return FixBridge(selectedBridge);
    }

    /// <summary>
    /// Pathfinding method.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoToResources()
    {
        AIPath.canMove = true;
        AIPath.canSearch = true;
        AIPath.destination = Current.transform.position;
        yield return Async.NextFrame;
        yield return Async.Until(() => AIPath.reachedEndOfPath);
    }


    /// <summary>
    /// Pathfinding method.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoToBridge()
    {
        AIPath.canMove = true;
        AIPath.canSearch = true;
        AIPath.destination = selectedBridge.transform.position;
        Debug.Log("Coming to bridge...");
        yield return Async.NextFrame;
        yield return Async.Until(() => bridgeOnTrigger);
        Debug.Log("Bridge is here.");
    }

    private IEnumerator GoPoints(bool reverse = false)
    {
        AIPath.canMove = false;
        AIPath.canSearch = false;

        Transform[] transforms = Current.Points;

        if (reverse)
        {
            Array.Reverse(transforms);
        }

        foreach (Transform item in transforms)
        {
            currentTarget = item;
            Debug.Log("Coming to point...");
            yield return Async.Until(() => Vector2.Distance(this.transform.position, item.position) <= 2);
        }
    }

    private IEnumerator GatherResources()
    {
        while (CurrentResources < maxCapacity)
        {
            CurrentResources += 1;
            Debug.Log($"I have {CurrentResources} resources now");

            yield return Async.Wait(TimeSpan.FromMilliseconds(600));
        }
    }

    private IEnumerator FixBridge(Bridge bridge)
    {
        while (CurrentResources > 0)
        {
            bridge.ResourcesAddedToBuild += 1;
            CurrentResources -= 1;
            Debug.Log($"I added to the bridge {bridge.ResourcesAddedToBuild} resources. I have {CurrentResources} now");

            yield return Async.Wait(TimeSpan.FromMilliseconds(900));
        }

        Debug.Log("O cholibka, skończyły mi się surowce.");
        AIPath.canMove = false;
        AIPath.canSearch = false;
        yield return GoPoints(true);
        yield return Run();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Bridge bridge = null;
        if (bridge = collision.GetComponent<Bridge>())
        {
            if (bridge == selectedBridge)
            {
                bridgeOnTrigger = true;
            }
        }

        if (HpableExtension.IsFromWrongTeam(this, collision, out Bullet bullet))
        {
            this.Hp.TakeHp(bullet.Dmg, "Bullet");
            Destroy(bullet.gameObject);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        Bridge bridge = null;
        if (bridge = collision.GetComponent<Bridge>())
        {
            if (bridge == selectedBridge)
            {
                bridgeOnTrigger = true;
            }
        }
    }

    private void Hp_OnValueChangeToMin(object sender, Hp.HpChangedArgs e)
    {
        // game over!
        Debug.Log("R.I.P");
    }


}
