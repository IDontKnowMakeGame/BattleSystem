using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UISmithy : UIBase
{
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Smithy");
    }
}
