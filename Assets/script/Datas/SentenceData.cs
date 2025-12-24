using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SentenceData
{
    public string label;
    public string ID;

    public SentenceData(string iD, string label)
    {
        ID = iD;
        this.label = label;
    }
}
