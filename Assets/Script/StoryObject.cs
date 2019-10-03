using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStory",menuName = "Story")]
public class StoryObject : ScriptableObject
{
    public bool Controlable;
    public bool Persistent;
    public List<StoryData> Stories;

    public float TotalLength
    {
        get
        {
            if (Math.Abs(_totalLength - (-1)) < float.Epsilon)
            {
                _totalLength = Stories.Sum(story => story.StoryLength);
            }

            return _totalLength;
        }
    }

    private float _totalLength = -1;
}
