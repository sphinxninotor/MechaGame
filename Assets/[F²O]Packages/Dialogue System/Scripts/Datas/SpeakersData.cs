using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpeakersData
{
    public enum STATE
    {
        IDLE,
        ANGRY,
        HAPPY,
        SAD,
    }

    [Serializable]
    public struct StateWrapper
    {
        public STATE state;
        public Sprite sprite;
        public AudioClip clip;
    }

    public string label;
    public string ID;

    public List<StateWrapper> states;

    public SpeakersData(string iD, string label)
    {
        ID = iD;
        this.label = label;
        states = new();
    }
}
