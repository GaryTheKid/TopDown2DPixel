using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Objective : MonoBehaviour
{
    [SerializeField] private List<int> _myCapturingList;
    [SerializeField] private List<int> _enemyCapturingList;

    private float captureProgress;
    private int capturingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        captureProgress = 0f;
        capturingPlayer = -1;
    }

    // Update is called once per frame
    void Update()
    {
        // only me in the area
        if (_myCapturingList.Count >= 1 && _enemyCapturingList.Count <= 0)
        {
            // me capturing
            OnMeCapturing();
        }
        // me and more than 1 enemy, or, more than 2 enemy in the area
        else if (_myCapturingList.Count >= 1 && _enemyCapturingList.Count >= 1 || _enemyCapturingList.Count >= 2)
        {
            // contesting
            OnEnemyCapturing();
        }
        // only 1 enemy in the area
        else if (_myCapturingList.Count <= 0 && _enemyCapturingList.Count == 1)
        {
            // enemy capturing
            OnContesting();
        }

        // if the point is complete captured by a player
        if (captureProgress >= 1f)
        {
            OnObjectiveCaptured(capturingPlayer);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // add me
        if (collision.gameObject.name == "CharacterCollider" && collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            _myCapturingList.Add(collision.GetComponentInParent<PhotonView>().OwnerActorNr);
        }

        // add enemy
        if (collision.gameObject.name == "CharacterCollider" && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _enemyCapturingList.Add(collision.GetComponentInParent<PhotonView>().OwnerActorNr);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // remove me
        if (collision.gameObject.name == "CharacterCollider" && collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            _myCapturingList.Remove(collision.GetComponentInParent<PhotonView>().OwnerActorNr);
        }

        // add enemy
        if (collision.gameObject.name == "CharacterCollider" && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _enemyCapturingList.Remove(collision.GetComponentInParent<PhotonView>().OwnerActorNr);
        }
    }

    private void OnMeCapturing()
    {
        // set the capturing player
        capturingPlayer = _myCapturingList[0];

        // increment capture progress based on time
        captureProgress += Time.deltaTime;

        // check if the capture progress is complete
        if (captureProgress >= 1f)
        {
            OnObjectiveCaptured(capturingPlayer);
        }
    }

    private void OnEnemyCapturing()
    {
        // set the capturing player to the enemy with the highest actor number
        capturingPlayer = _enemyCapturingList[_enemyCapturingList.Count - 1];

        // reset capture progress
        captureProgress = 0f;
    }

    private void OnContesting()
    {
        // reset capture progress
        captureProgress = 0f;

        // reset capturing player
        capturingPlayer = -1;
    }

    private void OnObjectiveCaptured(int playerActorNumber)
    {
        // do something when the objective is captured, such as updating the UI, awarding points, or ending the game
    }
}
