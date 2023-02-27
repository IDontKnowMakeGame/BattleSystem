using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Unit;

public class Torch : MonoBehaviour
{
    private Light torchLight;
    public Light TorchLight => torchLight;

    private void Awake()
    {
        torchLight = this.GetComponent<Light>();
    }

    public void ChangeTorchLight(bool isEnable)
    {
        torchLight.enabled = isEnable;
    }
}
