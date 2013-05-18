using System.Collections.Generic;
using System;

public class MessagePromptCoordinator
{
    public void hintWhenTouched(Action<Sprite> onCompleted, TouchSensor touch, float time, Dictionary<Sprite, ActionResponsePair[]> interactions)
    {
    }

    public void Update(float time)
    {
    }

    public void Reset()
    {
    }
}

public class ActionResponsePair {
    public string action {get;set;}
    public string[] responses {get;set;}
    
    public ActionResponsePair(string action, string[] responses) {
        this.action = action;
        this.responses = responses;
    }
}
