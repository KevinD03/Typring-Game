using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypableBTE : RandomTypingEvent
{
    void Start()
    {
       InitializeRTE(); 
    }

    public override void Pass() {}
    public override void Fail() {}
}

