using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailOnOff : MonoBehaviour
{
    [SerializeField]
    private GameObject[] obj;
    void Start()
    {
        for(int i =0; i<obj.Length; i++)
        {
            obj[i].gameObject.SetActive(false);
        }

        Define.GetManager<EventManager>().StartListening(EventFlag.TraillOnOff, ObjSetActive);
    }

    private void ObjSetActive(EventParam eventParam)
    {
        if (eventParam.boolParam)
        {
            for (int i = 0; i < eventParam.intParam; i++)
            {
                obj[i].gameObject.SetActive(true);
            }
        }
        else
        {
			for (int i = 0; i < 3; i++)
			{
				obj[i].gameObject.SetActive(false);
			}
		}
    }

	private void OnDisable()
	{
		Define.GetManager<EventManager>().StopListening(EventFlag.TraillOnOff, ObjSetActive);
	}
}
