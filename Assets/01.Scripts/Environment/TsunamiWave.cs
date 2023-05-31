using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Actors.Characters.Player;
using Actors.Characters;
using System.Linq;
using Actors.Bases;
using Core;
using Managements.Managers;

public class TsunamiWave : Actor
{
    [SerializeField]
    private float lifeTime = 1f;
    [SerializeField]
    private Vector3 velocity = new Vector3(0, 0, 6);
    [SerializeField]
    private float checkTime = 0.01f;

    private float timer = 0;

    [SerializeField]
    private GameObject moveCharacter;
    [SerializeField]
    private VisualEffect particle;
    [SerializeField]
    private UnitSpawnerController spawnerController;

    private bool playTsunami = false;

    private List<CharacterActor> inSideUnits = new List<CharacterActor>();
    private List<CharacterActor> currentUnits = new List<CharacterActor>();

    protected override void Start()
    {
        particle.Stop();
    }                                                                                                    


    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && !playTsunami)    
        {
            timer = 0f;
            moveCharacter.transform.localPosition = Vector3.zero;
            playTsunami = true;
            inSideUnits.Clear();
            currentUnits = spawnerController.Units.ToList();
            particle.gameObject.SetActive(true);
            particle.Play();   
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("particle");
            particle.Stop();
        }
        if (playTsunami)
        {
            if (timer >= lifeTime)
            {
                AdaptPos();
                playTsunami = false;
                return;
            }
            timer += Time.deltaTime;

            var map = Define.GetManager<MapManager>();

            Vector3 checkMap = Vector3Int.CeilToInt(moveCharacter.transform.position);
            if (!map.GetBlock(checkMap.SetY(0)))
            {
                AdaptPos();
                particle.gameObject.SetActive(false);
                playTsunami = false;
                return;
            }

            moveCharacter.transform.localPosition = moveCharacter.transform.localPosition.SetX((timer / lifeTime) * velocity.z);


            for(int i = 0; i < currentUnits.Count; i++)
            {
                if (currentUnits[i] != null)
                {
                    if (Vector3.Distance(currentUnits[i].transform.position.Flattened(), moveCharacter.transform.position.Flattened()) <= 0.5f)
                    {
                        Debug.Log(currentUnits[i].name + "이 존재합니다.");
                        inSideUnits.Add(currentUnits[i]);
                        currentUnits.Remove(currentUnits[i]);
                        i--;
                    }
                }
            }

            foreach (CharacterActor unit in inSideUnits)
            {
                if (unit != null)
                    unit.transform.position = moveCharacter.transform.position.SetY(1);
            }
        }
    }

    private void AdaptPos()
    {
        foreach (CharacterActor unit in inSideUnits)
        {
            if (unit != null)
            {
                unit.transform.position = unit.Position.SetY(1);
                if (inSideUnits.Count > 1 && !(unit is PlayerActor))
                {
                    unit.OnKnockBack?.Invoke(1, this);
                }
            }
        }
    }
}
