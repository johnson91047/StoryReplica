using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStory",menuName = "Story")]
public class StoryObject : ScriptableObject
{
    public List<StoryData> Stories;
}
