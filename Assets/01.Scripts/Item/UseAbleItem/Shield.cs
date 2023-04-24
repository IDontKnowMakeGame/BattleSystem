using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.Player;
using Core;
using Managements.Managers;

public class Shield : UseAbleItem
{
    private bool use = false;

    private Dictionary<Vector3, GameObject> ShieldPos = new Dictionary<Vector3, GameObject>();

    public override void UseItem()
    {
        // πÊ«‚
        use = !use;

        if (use)
            InputManager<Weapon>.OnAttackPress += SpawnShield;
        else
            InputManager<Weapon>.OnAttackPress -= SpawnShield;
    }

    private void SpawnShield(Vector3 dir)
    {
        if (InGame.Player.HasState(Actors.Characters.CharacterState.Everything & ~Actors.Characters.CharacterState.Attack)) return;
        Vector3 spawnPos = (InGame.Player.transform.position - dir).SetY(0.5f);

        if (ShieldPos.ContainsKey(spawnPos))
        {
            if (ShieldPos[spawnPos] == null)
                ShieldPos.Remove(spawnPos);
            else
                return;
        }


        GameObject shield = Define.GetManager<ResourceManager>().Instantiate("Shield");
        shield.transform.position = spawnPos;
        shield.transform.rotation = Quaternion.Euler(RotateShield(dir));

        use = false;
        InputManager<Weapon>.OnAttackPress -= SpawnShield;

        ShieldPos.Add(spawnPos, shield);
    }

    private Vector3 RotateShield(Vector3 dir)
    {
        Vector3 rotate = Vector3.zero;
        if (dir == Vector3.right)
            rotate = new Vector3(0, 90, 0);
        else if (dir == Vector3.left)
            rotate = new Vector3(0, -90, 0);
        else if (dir == Vector3.back)
            rotate = new Vector3(0, 180, 0);
        return rotate;
    }
}
