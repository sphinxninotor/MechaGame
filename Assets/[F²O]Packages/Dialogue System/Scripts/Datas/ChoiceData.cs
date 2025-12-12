using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ChoiceData
{
    public string ID;
    public string label;

    public ChoiceData(string iD, string label)
    {
        ID = iD;
        this.label = label;
    }
}
