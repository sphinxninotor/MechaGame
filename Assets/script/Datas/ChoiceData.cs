using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ChoiceData
{
    public string ID;
    public string label;
    public bool launchRoulette; // Indique si ce choix doit lancer la roulette

    public ChoiceData(string iD, string label, bool launchRoulette = false)
    {
        ID = iD;
        this.label = label;
        this.launchRoulette = launchRoulette;
    }
}
