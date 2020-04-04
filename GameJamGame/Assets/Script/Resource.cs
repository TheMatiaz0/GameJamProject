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

    public uint ResourceCount { get { return _ResourceCount; } set { if (_ResourceCount <= 0) { Destroy(this.gameObject); } _ResourceCount = value; } }
    private uint _ResourceCount = 10;

    protected void Start()
    {
        ResourceCount = resourceCount;
        ResourceList.Add(this);
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

    protected void OnMouseEnter()
    {
        WorldUI.Instance.FirstActivate(true);
    }

    protected void OnMouseExit()
    {
        WorldUI.Instance.FirstActivate(false);
    }

    protected void OnMouseOver()
    {
        Vector2 vect = new Vector2(this.transform.position.x, this.transform.position.y + 15);

        WorldUI.Instance.Move(Camera.main.WorldToScreenPoint(vect));
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