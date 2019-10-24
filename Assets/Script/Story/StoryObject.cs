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
    public bool HasSurveyOne;
    public List<StoryData> Stories;


    public int Id
    {
        get
        {
            if (_id == 0)
            {
                Stories.ForEach(story => _id += story.GetHashCode());
            }

            return _id;
        }
    }

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
    private int _id = 0;
}
