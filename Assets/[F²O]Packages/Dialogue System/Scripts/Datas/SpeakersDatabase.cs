using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeakersDatabase", menuName = "Database/Dialogue/Speakers", order = 0)]
public class SpeakersDatabase : ScriptableObject
{
    public List<SpeakersData> speakersDatas = new();

    public SpeakersData GetSpeakerData(string id) => speakersDatas.Find(x => x.ID == id);
}
