using UnityEngine;

public class AnimationEventDisableSelf : MonoBehaviour
{
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
