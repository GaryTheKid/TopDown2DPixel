using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FXPlayer_Projectile : MonoBehaviour
{
    public abstract void PlayInitializationFX();
    public abstract void PlayStickFX();
    public abstract void PlayCollisionFX();
    public abstract void PlayHitFX();
}
