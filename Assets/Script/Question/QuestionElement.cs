using TMPro;
using UnityEngine;

public abstract class QuestionElement : MonoBehaviour
{
    public TextMeshProUGUI Title;

    public abstract int GetValue();
    public abstract bool IsComplete();
}
