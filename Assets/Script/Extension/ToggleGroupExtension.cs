using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ToggleGroupExtensio
{

    public static int GetActivatedToggleIndex(this ToggleGroup group)
    {
        int value = 0;
        foreach (Toggle activeToggle in group.ActiveToggles())
        {
            if (activeToggle.isOn)
            {
                value = activeToggle.transform.GetSiblingIndex();
                break;
            }
        }

        return value;
    }

    public static Transform GetActivatedToggleTranform(this ToggleGroup group)
    {
        foreach (Toggle activeToggle in group.ActiveToggles())
        {
            if (activeToggle.isOn)
            {
                return activeToggle.transform;
            }
        }

        return null;
    }
}
