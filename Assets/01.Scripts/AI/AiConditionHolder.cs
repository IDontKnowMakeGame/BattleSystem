using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Characters.Enemy;
using Acts.Characters.Enemy;
using AI;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AiConditionHolder : MonoBehaviour
{
    public string Target;
    public string Goal;
    [HideInInspector] public List<AiCondition> Conditions = new ();

    private void Start()
    {
        var enemy = GetComponent<EnemyActor>();
        var ai = enemy.GetAct<EnemyAI>();
        foreach (var condition in Conditions)
        {

            var currentStateType = Type.GetType("AI.States." + Target + "State");
            var nextStateType = Type.GetType("AI.States." + Goal + "State");
            if (currentStateType == null || nextStateType == null)
            {
                Debug.LogError("State not found");
                return;
            }
            var currentState = ai._states[currentStateType];
            var nextTransition = currentState.Transitions.Find((x) => x.NextState == nextStateType);
            nextTransition.ConditionHolder = this;
            nextTransition.Init();
        }
    }
}
