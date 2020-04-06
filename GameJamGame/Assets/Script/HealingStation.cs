﻿using Cyberevolver;
using Cyberevolver.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingStation : Building
{
	[SerializeField]
	private SerializedTimeSpan cooldown;

	[SerializeField]
	private Cint hpAdd = 1;

	private CooldownController cooldownController = null;


	protected void Start()
	{
		cooldownController = new CooldownController(this, cooldown.TimeSpan);
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		base.OnTriggerEnter2D(collision);

		Player player = null;
		Carrier carrier = null;

		if (cooldownController.Try())
		{
			if (carrier = collision.GetComponent<Carrier>())
			{
				Heal(carrier);
			}

			else if (player = collision.GetComponent<Player>())
			{
				Heal(player);
			}
		}

	}

	private void Heal (IHpable hpable)
	{
		hpable.Hp.GiveHp(hpAdd, "Healing Station");
	}
}