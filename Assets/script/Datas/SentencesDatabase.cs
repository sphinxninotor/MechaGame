using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSentencesDatabase", menuName = "Database/Dialogue/Sentence", order = 0)]
public class SentencesDatabase : ScriptableObject
{
    public List<SentenceData> sentenceDatas = new();

    public SentenceData GetSentenceData(string id) => sentenceDatas.Find(x => x.ID == id);
}
