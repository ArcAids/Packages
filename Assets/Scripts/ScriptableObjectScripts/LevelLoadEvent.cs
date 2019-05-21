using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelLoadEvent : ScriptableObject
{
    public int LoadLevelNumber;

    public List<string> tips;

    LevelLoader listener;

    public string GetRandomTip()
    {
        if (tips != null && tips.Count > 0)
            return tips[Random.Range(0, tips.Count - 1)];
        else
            return string.Empty;
    }

    public void Load(int levelIndex)
    {
        LoadLevelNumber= levelIndex;
        if(listener!=null)
        listener.OnEventRaised(this);
        
    }

    public void Register(LevelLoader passedEvent)
    {
        listener =passedEvent;
    }

    public void DeRegister(LevelLoader passedEvent)
    {
        if (listener == passedEvent)
            listener = null;
    }
}
