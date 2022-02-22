using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Consumables : Item
{
    public virtual void Consume(PhotonView PV)
    {
        // do something
        
        if (--amount <= 0)
        {
            DestroySelf();
        }
    }
}
