using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;

[System.Serializable]
public class PlayerAttack : UnitAttack
{
    private AttackCollider attackColParent;
    public AttackCollider AttackColParent
    {
        get
        {
            if (attackColParent == null)
            {
                Debug.LogError("attackColParent�� NULL�Դϴ�.");
                return new AttackCollider();
            }
            return attackColParent;
        }
    }
      
    public override void Start()
    {
        base.Start();
        attackColParent = GameObject.FindObjectOfType<AttackCollider>();
    }

    public override void Attack(Vector3 idx)
    {
        // To Do �ݶ��̴��� ���� Attack Check
    }
}
