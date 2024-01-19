using UnityEngine;
using System.Collections.Generic;

public abstract class UIController : MonoBehaviour
{
    protected List<ITweanable> _tweenObjects = new List<ITweanable>();

    public void AddTweenObjects(ITweanable tweanable)
    {
        _tweenObjects.Add(tweanable);
    }
}
