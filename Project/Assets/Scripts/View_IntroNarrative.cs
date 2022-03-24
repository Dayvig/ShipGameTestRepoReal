using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class View_IntroNarrative : MonoBehaviour
{
    public Model_Narrative narrative;

    public int _narrativeIndex;
    public bool _done;
    private float _timer;
    private int _stringIndex;
    private float _printDialogueTimer;
    private List<string> randomizedOptions = new List<string>();
    private int[] OptionIndexes = {8, 12};
    private float dialoguestartTimer = 0;
    public Model_Player playerModel;

    public void Start()
    {
        CleanupNarrative();

        bool testFail = false;
        if (narrative.portraitSequence.Length != narrative.dialogue.Length) testFail = true;
        if (narrative.portraitSequence.Length != narrative.intervals.Length) testFail = true;

        if (testFail) Debug.LogError("ViewController_IntroNarrative: ERROR! Narrative arrays are not the same length.");
        _done = true;
    }

    public void Update()
    {
        if (dialoguestartTimer > narrative.dialogueStartInterval)
        {
            StartDialogue();
            dialoguestartTimer = 0f;
        }

        if (!_done)
        {
            UpdateFromGameController();
            UpdateOptions();
        }
        else
        {
            dialoguestartTimer += Time.deltaTime;
        }
    }   

    public void StartDialogue()
    {
        _ChangePortrait();
        _done = false;
    }

    public bool UpdateFromGameController()
    {
        _timer += Time.deltaTime;
        if (!_done && !isOptionIndex(_narrativeIndex) && _timer >= narrative.intervals[_narrativeIndex])
            
        {
            _timer = 0;
            _narrativeIndex++;

            if (!isOptionIndex(_narrativeIndex))
            {
                _ChangePortrait();
                _ChangeDialogue();
            }
            else
            {
                _DisplayOptions(_narrativeIndex);
            }
        }

        if (!isOptionIndex(_narrativeIndex))
        {
            _IncreaseDisplayedString();
        }

        if (_narrativeIndex >= narrative.portraitSequence.Length)
        {
            _done = true;
            CleanupNarrative();
            _narrativeIndex = 0;
        }

        return _done;
    }

    private void _ChangePortrait()
    {
        for (int i = 0; i < narrative.portraits.Length; i++)
            narrative.portraits[i].enabled = (i == narrative.portraitSequence[_narrativeIndex]);
        narrative.dialogueUIBOX.enabled = true;
    }
    private void _ChangeDialogue()
    {
        _stringIndex = 0;
        narrative.dialogueDisplay.text = "";
    }
    private void _IncreaseDisplayedString()
    {
        _printDialogueTimer += Time.deltaTime;
        if (_printDialogueTimer >= narrative.textInterval)
        {
            _printDialogueTimer -= narrative.textInterval;
            _stringIndex++;
            _stringIndex = Mathf.Clamp(_stringIndex, 0, narrative.dialogue[_narrativeIndex].Length);
            narrative.dialogueDisplay.text = narrative.dialogue[_narrativeIndex].Substring(0, _stringIndex);
        }
    }

    private void _DisplayOptions(int o)
    {
        narrative.dialogueDisplay.text = "";
        int r1;
        int r2;
        switch (o)
        {
            case 8:
                r1 = 0;
                r2 = 3;
                break;
            case 12:
                r1 = 3;
                r2 = 6;
                break;
            default:
                r1 = 0;
                r2 = 3;
                break;
        }
        for (int i = r1; i < r2; i++)
        {
            randomizedOptions.Add(narrative.options[i]);
        }
        for (int i = 0; i < narrative.optionDisplay.Length; i++)
        {
            int rand = (int)Random.Range(0, randomizedOptions.Count);
            narrative.optionDisplay[i].text = randomizedOptions[rand];
            randomizedOptions.Remove(randomizedOptions[rand]);
        }

        int rand2 = (int) Random.Range(0, 2);
        Vector3 selectorPos = narrative.selector.transform.position;
        selectorPos.x = narrative.optionDisplay[rand2].transform.position.x;
        narrative.selector.transform.position = selectorPos;
        narrative.selectorOptionActive = rand2;
        narrative.selector.enabled = true;
    }

    private void HideOptions()
    {
        narrative.selector.enabled = false;
        for (int i = 0; i < narrative.optionDisplay.Length; i++)
            narrative.optionDisplay[i].text = "";
    }

    private void UpdateOptions()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            narrative.selectorOptionActive--;
            if (narrative.selectorOptionActive < 0)
            {
                narrative.selectorOptionActive = 2;
            }
            MoveSelector();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            narrative.selectorOptionActive++;
            if (narrative.selectorOptionActive > 2)
            {
                narrative.selectorOptionActive = 0;
            }
            MoveSelector();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            narrative.optionsSelected.Add(narrative.selectorOptionActive);
            if (_narrativeIndex < narrative.portraitSequence.Length-1)
            {
                _timer = 0;
                _narrativeIndex++;
            }
            else
            {
                _done = true;
                CleanupNarrative();
                
                if (narrative.optionsSelected.Count.Equals(narrative.correctOptions.Count))
                {
                    bool isCorrect = true;
                    for (int k = 0; k < narrative.correctOptions.Count; k++)
                    {
                        if (!(narrative.correctOptions[k] == narrative.optionsSelected[k]))
                        {
                            isCorrect = false;
                        }
                    }
                    if (isCorrect) { playerModel.fireRate /= 2; }
                    narrative.optionsSelected.Clear();
                }
                _narrativeIndex = 0;
            }

            HideOptions();
        }
    }

    private bool isOptionIndex(int q)
    {
        for (int i = 0; i < OptionIndexes.Length; i++)
        {
            if (q == OptionIndexes[i])
            {
                return true;
            }
        }

        return false;
    }

    private void MoveSelector()
    {
        Vector3 selectorPos = narrative.optionDisplay[narrative.selectorOptionActive].transform.position;
        selectorPos.y = narrative.optionDisplay[narrative.selectorOptionActive].transform.position.y + 20;
        narrative.selector.transform.position = selectorPos;
    }


    public void CleanupNarrative()
    {
        for (int i = 0; i < narrative.portraits.Length; i++)
            narrative.portraits[i].enabled = false;
        narrative.dialogueDisplay.text = "";
        narrative.selector.enabled = false;
        for (int i = 0; i < narrative.optionDisplay.Length; i++)
            narrative.optionDisplay[i].text = "";
        narrative.dialogueUIBOX.enabled = false;
    }
        
}
