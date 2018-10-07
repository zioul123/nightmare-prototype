using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : Character 
{
    public virtual void Deselect() 
    {

    }

    public virtual Transform Select()
    {
        return hitBox;
    }
}
