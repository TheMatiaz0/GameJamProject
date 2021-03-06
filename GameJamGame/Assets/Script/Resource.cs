﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyberevolver;
using Cyberevolver.Unity;
using UnityEngine;
using System.Collections;

[ShowCyberInspector]
public class Resource : MonoBehaviourPlus
{
    public static List<Resource> ResourceList { get; private set; } = new List<Resource>();

    [field: SerializeField]
    public Transform[] Points { get; private set; }

    [SerializeField]
    private GameObject pavementPrefab = null;

    private static GameObject lineObject = null;

    [SerializeField]
    private uint resourceCount = 10;

    [SerializeField]
    private ResourceUIActivator resourceUIActivator = null;

    public Cint ResourceCount { get { return _ResourceCount; } set { if (_ResourceCount != value) { _ResourceCount = value; } OnResourceChange(_ResourceCount); } }
    private Cint _ResourceCount = 10;

    protected void Start()
    {
        ResourceCount = resourceCount;
        ResourceList.Add(this);
        OnResourceChange(resourceCount);
    }

    private void OnResourceChange (uint resources)
    {
        resourceUIActivator.OnResourceChange(resources);

        if (_ResourceCount <= 0) 
        { 
            Destroy(this.gameObject);
        }
    }

    public void DrawLine ()
    {
        lineObject = Instantiate(pavementPrefab, this.transform);

        LineRenderer lineRender = lineObject.GetComponent<LineRenderer>();

        Color color = UnityEngine.Random.ColorHSV();

        lineRender.startColor = color;
        lineRender.endColor = color;

        lineRender.positionCount = Points.Length + 1;

        lineRender.SetPosition(0, (Vector2)this.transform.position);

        for (int x = 1; x < Points.Length + 1; x++)
        {
            lineRender.SetPosition(x, (Vector2)Points[x - 1].position);
        }
    }

    public static void RemoveAllLines ()
    {
        Destroy(lineObject);
    }

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.DrawLine(this.transform.position, Points[0].position);
            for (int x = 0; x < Points.Length - 1; x++)
            {
                Gizmos.DrawLine(Points[x].position, Points[x + 1].position);
            }
        }
        catch { }
       
    }

    public static Resource GetClosestResource(Vector2 currentPosition, float range)
    {
        Resource bestTarget = null;
        foreach (Resource resource in ResourceList)
        {
            if (resource == null)
            {
                continue;
            }

            Vector2 directionToTarget = (Vector2)resource.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < range)
            {
                range = dSqrToTarget;
                bestTarget = resource;
            }
        }

        return bestTarget;
    }
}