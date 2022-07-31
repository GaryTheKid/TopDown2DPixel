using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastValidator : MonoBehaviour
{
    [SerializeField] private CastIndicatorController castIndicatorController;
    [SerializeField] private List<SpriteRenderer> sprites;
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;

    private void OnEnable()
    {
        castIndicatorController.isCastValid = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        LayerMask targetLayer = (1 << collision.gameObject.layer);
        if ((castIndicatorController.invalidCastLayerMask & targetLayer) == targetLayer)
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = invalidColor;
            }
            castIndicatorController.isCastValid = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LayerMask targetLayer = (1 << collision.gameObject.layer);
        if ((castIndicatorController.invalidCastLayerMask & targetLayer) == targetLayer)
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = validColor;
            }
            castIndicatorController.isCastValid = true;
        }
    }
}
