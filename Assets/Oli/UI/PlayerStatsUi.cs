﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cirrus.Unity.Objects;

namespace Tojam2022
{
	public class PlayerStatsUi : SingletonComponentBase<PlayerStatsUi>
	{
		[SerializeField]
		private StatUiItem _moneyStat;

		[SerializeField]
		private StatUiItem _healthStat;

		//[SerializeField]
		//private StatUiItem _hungerStat;

		//[SerializeField]
		//private StatUiItem _rageStat;

		public override void Start()
		{
			base.Start();

			//_hungerStat.Name = "Hunger";
			
		}



		public void Update()
		{


			if (Alien.Instance.State == AlienState.Game)
			{
				_healthStat.Value = Player.Instance.Health;
				_moneyStat.Value = Player.Instance.Money;
			}
		}
	}
}
