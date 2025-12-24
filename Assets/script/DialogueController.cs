using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DialogueConfig;

public class DialogueController : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private TMP_Text _txtSpeaker;
    [SerializeField] private TMP_Text _txtInfo;

    [Header("Choices")]
    [SerializeField] private GameObject _choicesParent;
    [SerializeField] private GameObject _choicePrefab;

    [Header("Sprites")]
    [SerializeField] private GameObject _spriteParent;
    private List<Image> _images = new();

    [Header("Roulette")]
    [SerializeField] private Roulette roulette; // <-- Ajout de la référence

    DialogueConfig _diagConfig;

    private void Awake()
    {
        if(!_diagConfig)
            gameObject.SetActive(false);
    }

    public void PlayDialogue(DialogueConfig diagConfig)
    {
        _diagConfig = diagConfig;

        InitSprite();
        InitDialog(diagConfig.datas[0]);

        gameObject.SetActive(true);
    }

    private void InitSprite()
    {
        if (_spriteParent == null)
            return;

        _images?.AddRange(_spriteParent?.GetComponentsInChildren<Image>());

        foreach (var s in _images)
            s.transform.parent.gameObject.SetActive(false);
    }

    private void InitDialog(DialogData data)
    {
        if (string.IsNullOrEmpty(data.IDSentence))
            return;

        var spkData = DialogueManager.Instance.SpeakerDB.speakersDatas.Find(x => x.ID == data.IDSpeaker);
        ShowSpeakerSprite(spkData, data.position, data.IDState);

        foreach (var sentences in DialogueManager.Instance.SentencesDBs)
        {
            if (sentences.sentenceDatas.Exists(x => x.ID == data.IDSentence))
            {
                _txtInfo.text = sentences.sentenceDatas.Find(x => x.ID == data.IDSentence).label;
                break;
            }
        }
        InitChoices(data);
    }

    private void InitChoices(DialogData data)
    {
        CleanDefaultChoices();

        foreach (var choice in data.choices)
        {
            var newgo = Instantiate(_choicePrefab, _choicesParent.transform);

            if (newgo.TryGetComponent<Button>(out var bt))
            {
                var choiceData = DialogueManager.Instance.ChoicesDB.GetChoiceData(choice.IDChoice);

                if (string.IsNullOrEmpty(choice.IDSentence))
                {
                    bt.onClick.AddListener(() => gameObject.SetActive(false));
                    // Si ce choix doit lancer la roulette
                    if (choiceData.launchRoulette && roulette != null)
                    {
                        bt.onClick.AddListener(() => roulette.StartRolling());
                    }
                }
                else
                {
                    var diagData = _diagConfig.datas.Find(x => x.IDSentence == choice.IDSentence && x.IDSpeaker == choice.speaker);
                    bt.onClick.AddListener(() => InitDialog(diagData));
                }

                newgo.GetComponentInChildren<TMP_Text>().text = choiceData.label;
            }
        }
    }
    
    private void CleanDefaultChoices()
    {
        foreach (var go in _choicesParent.GetComponentsInChildren<Button>())
            Destroy(go.gameObject);
    } 

    private void ShowSpeakerSprite(SpeakersData spkData, DialogData.POSITION pos, SpeakersData.STATE state)
    {
        foreach (var s in _images)
            s.transform.parent.gameObject.SetActive(false);

        _txtSpeaker.text = spkData.label;

        var img = _images.Find(x => x.transform.parent.name.ToLower().Contains(pos.ToString().ToLower()));

        if (img != null)
        {
            img.sprite = spkData.states.Find(x => x.state == state).sprite;
            img.transform.parent.gameObject.SetActive(true);
        }
    }
}
