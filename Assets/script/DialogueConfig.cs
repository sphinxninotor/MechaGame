using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConfig : MonoBehaviour
{
    [Serializable]
    public struct DialogData
    {
        public enum POSITION
        {
            LEFT,
            MIDDLE,
            RIGHT,
        }

        [Serializable]
        public struct ChoiceWrapper
        {
            public string IDChoice;
            public string speaker;
            public string IDSentence;
        }

        [Header("Speaker")]
        public string IDSpeaker;
        public SpeakersData.STATE IDState;
        public POSITION position;

        [Header("Infos")]
        public string IDSentence;
        public List<ChoiceWrapper> choices;

        public DialogData(string speaker, string sentence, List<ChoiceWrapper> choices)
        {
            IDSpeaker = speaker;
            IDState = SpeakersData.STATE.IDLE;
            position = POSITION.LEFT;
            IDSentence = sentence;
            this.choices = choices;
        }
    }

    public List<DialogData> datas;

}
