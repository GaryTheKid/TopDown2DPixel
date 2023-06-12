using UnityEngine;

public class UI_SocialInteractionEquipmentSlot : MonoBehaviour
{
    public UI_SocialView ui_SocialView;
    public int socialInteractionIndex;
    public Transform anchorTransform;
    public GameObject SocialInteractionVisual;

    private void Start()
    {
        socialInteractionIndex = -1;
    }

    public void Equip(int index)
    {
        // check if index valid
        if (index < 0)
        {
            Unequip();
            return;
        }

        // show visual
        SocialInteractionVisual = Instantiate(PlayerAssets.singleton.SocialInteractionList[index].staticObj, anchorTransform);

        // set index
        socialInteractionIndex = index;

        // select the option
        ui_SocialView.ShowSelectedHightlight(socialInteractionIndex);
    }

    public void Unequip()
    {
        // hide visual
        Destroy(SocialInteractionVisual);

        // deselect the option
        ui_SocialView.HideSelectedHightlight(socialInteractionIndex);

        // reset index
        socialInteractionIndex = -1;
    }
}
