using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalQuestion : QuestionElement
{
    public ToggleGroup ToggleGroup;
    public override int GetValue()
    {
        foreach (Toggle activeToggle in ToggleGroup.ActiveToggles())
        {
            if (activeToggle.isOn)
            {
                return activeToggle.transform.GetSiblingIndex();
            }
        }

        return -1;
    }

    public override bool IsComplete()
    {
        return ToggleGroup.AnyTogglesOn();
    }

    public override SurveyData GetData()
    {
        return new SurveyData
        {
            QuestionName = Title.text,
            Value = ToggleGroup.GetActivatedToggleIndex()
        };
    }
}
