using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Actors.Characters.Player;
using Actors.Characters;
using System.Linq;

public class TsunamiWave : MonoBehaviour
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

    private void Start()
    {
        particle.Stop();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            timer = 0f;
            playTsunami = true;
            inSideUnits.Clear();
            currentUnits = spawnerController.Units.ToList();
            particle.Play();
        }

        if (playTsunami)
        {
            if (timer >= lifeTime)
            {
                foreach(CharacterActor unit in inSideUnits)
                {
                    if(unit != null)
                        unit.transform.position = unit.Position.SetY(1);
                }
                playTsunami = false;
                return;
            }
            timer += Time.deltaTime;
            moveCharacter.transform.localPosition = moveCharacter.transform.localPosition.SetZ((timer / lifeTime) * velocity.x);

            foreach (CharacterActor unit in currentUnits)
            {
                if(unit != null)
                {
                    if(Vector3.Distance(unit.transform.position.Flattened(), moveCharacter.transform.position.Flattened()) <= 0.5f)
                    {
                        inSideUnits.Add(unit);
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
}
