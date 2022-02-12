using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworkController : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private PlayerMovementController movementController;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject audioListener;
    [SerializeField] private SpriteRenderer characterSprite;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(movementController);
            Destroy(playerCamera);
            Destroy(audioListener);
        }
        else 
        {
            //characterSprite.color = Color.green;
        }
    }
}