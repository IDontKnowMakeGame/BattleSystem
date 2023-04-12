using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.States
{
	public class ShootState : AiState
	{
		public override void Init()
		{
			Name = "Shoot";
			base.Init();
		}
	}
}
