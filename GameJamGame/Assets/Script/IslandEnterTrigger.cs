﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandEnterTrigger : MonoBehaviour
{
	private bool hasActivated = false;

	[SerializeField]
	private BridgeSelection selection;

	[SerializeField]
	private Transform islandCarrierSpawnPoint = null;

	public Transform IslandCarrierSpawnPoint => islandCarrierSpawnPoint;

	[SerializeField]
	private Carrier carrier = null;

	public Carrier Carrier => carrier;

	[SerializeField]
	private Resource[] resourcesOnIsland;

	public Resource[] ResourcesOnIsland => resourcesOnIsland;

	[SerializeField]
	private Transform[] enemyPoints = null;

	public Transform[] EnemyPoints => enemyPoints;

	private void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.GetComponent<Player>())
		{
			if (hasActivated == false)
			{
				ActivateBridgeSelection();
				hasActivated = true;
				return;
			}

			else if (hasActivated == true && selection.gameObject.activeSelf == false)
			{
				PopupText.Instance.MainGameObject.SetActive(true);
				PopupText.Instance.BaseText.text = "Press 'F' to choose a bridge.";
			}

		}
	}

	private void ActivateBridgeSelection ()
	{
		selection.gameObject.SetActive(true);
		selection.Activate(this);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		PopupText.Instance?.MainGameObject?.SetActive(false);
	}

	public void Interaction ()
	{
		if (hasActivated == false || PopupText.Instance.MainGameObject.activeSelf == false)
		{
			return;
		}

		ActivateBridgeSelection();
		PopupText.Instance?.MainGameObject?.SetActive(false);
	}
}
