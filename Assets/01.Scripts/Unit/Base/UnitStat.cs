using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Player
{
	public class UnitStat : Behaviour
	{
		[SerializeField]
		private UnitSO _unitSO;
		[SerializeField]
		private BasicHPSlider _hpSlider;

		private float _baseHP;

		#region �⺻ ���� �Լ���
		//protected virtual void OnEnable()
		//{

		//}
		#endregion

		public virtual void Damage(int damage)
		{
			_baseHP -= damage;
			_hpSlider.SetSlider(_baseHP);
			if (_baseHP <= 0)
			{
				Die();
			}
		}

		protected virtual void Die()
		{
			thisBase.enabled = true;
		}

		protected virtual void Reset()
		{
			_baseHP = _unitSO.BaseHP;
			_hpSlider.InitSlider(_baseHP);
		}

		public override void OnEnable()
		{
			Reset();
		}

	}
}
