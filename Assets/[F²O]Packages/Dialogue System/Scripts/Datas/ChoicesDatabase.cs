using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChoicesDatabase", menuName = "Database/Dialogue/Choice", order = 0)]
public class ChoicesDatabase : ScriptableObject
{
    public List<ChoiceData> choiceDatas = new();
    public ChoiceData GetChoiceData(string id) => choiceDatas.Find(x => x.ID == id);
}
