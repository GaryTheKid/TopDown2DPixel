using UnityEngine;
using Photon.Pun;

public class RPC_Well : MonoBehaviour
{
    private Well _well;

    private void Awake()
    {
        _well = GetComponent<Well>();
    }

    [PunRPC]
    void RPC_DrinkFromWell()
    {
        _well.isUsable = false;
        _well.animator.SetTrigger("Drink");
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger)
            {
                collider.enabled = false;
            }
        }
        _well.Restore();
    }
}