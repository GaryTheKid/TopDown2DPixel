using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Well : MonoBehaviour
{
    private const float WELL_RESTORE_TIME = 5f;

    public bool isUsable;

    [SerializeField] private GameObject interactionText;

    private Animator animator;
    
    private void Awake()
    {
        isUsable = true;
        animator = GetComponent<Animator>();
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
        isUsable = false;
        animator.SetTrigger("Drink");
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger)
            {
                collider.enabled = false;
            }
        }
        Restore();
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
