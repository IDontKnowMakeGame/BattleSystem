using System.Collections;
using System.Collections.Generic;
using Unit.Core;
using UnityEngine;

public interface IStatChange
{
	public UnitStats addstat { get; set; }
	public UnitStats multistat { get; set; }
}
