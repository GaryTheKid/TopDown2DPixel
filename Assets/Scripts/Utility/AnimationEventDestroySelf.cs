using UnityEngine;

public class AnimationEventDestroySelf : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}