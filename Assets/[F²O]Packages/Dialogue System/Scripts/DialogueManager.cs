using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [field : SerializeField] public SpeakersDatabase SpeakerDB { get; private set; }
    [field : SerializeField] public ChoicesDatabase ChoicesDB { get; private set; }
    [field : SerializeField] public List<SentencesDatabase> SentencesDBs { get; private set; }
    public DialogueController DialogueCtlr { get; private set; }
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
}
