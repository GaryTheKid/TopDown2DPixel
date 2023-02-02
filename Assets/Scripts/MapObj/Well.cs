using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Well : MonoBehaviour
{
    private const float WELL_RESTORE_TIME = 10f;

    [SerializeField] private GameObject interactionText;

    public bool isUsable;
    public Animator animator;

    private PhotonView _PV;

    private void Awake()
    {
        isUsable = true;
        animator = GetComponent<Animator>();
        _PV = GetComponent<PhotonView>();
    }

    public void DisplayInteractionText()
    {
        if(isUsable)
            interactionText.SetActive(true);
    }

    public void HideInteractionText()
    {
        interactionText.SetActive(false);
    }

    public void Drink()
    {
        NetworkCalls.MapObj_Network.DrinkFromWell(_PV);
    }

    public void Restore()
    {
        StartCoroutine(Co_Restore());
    }

    private IEnumerator Co_Restore()
    {
        yield return new WaitForSecondsRealtime(WELL_RESTORE_TIME);
        isUsable = true;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger)
            {
                collider.enabled = true; ;
            }
        }
        animator.SetTrigger("Restore");
    }
}
