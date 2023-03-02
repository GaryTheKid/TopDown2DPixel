using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Objective : MonoBehaviour
{
    public List<int> _myCapturingList;
    public List<int> _enemyCapturingList;
    public bool isActive;

    [SerializeField] private Image _progressUI;
    [SerializeField] private Text _capturePlayerNameUI;

    [SerializeField] private float captureProgress;
    [SerializeField] private int capturingPlayer;

    [SerializeField] private float captureRate;
    [SerializeField] private float captureResetRate;
    [SerializeField] private float captureErasionRate;

    private PhotonView _PV;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        captureProgress = 0f;
        capturingPlayer = -1;
        captureResetRate = 0.15f;
        captureRate = 0.25f;
        captureErasionRate = 0.4f;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // check if active
        if (!isActive)
        {
            return;
        }

        /// only me in the area
        if (_myCapturingList.Count >= 1 && _enemyCapturingList.Count <= 0)
        {
            // me capturing
            OnMeCapturing();
        }
        // me and more than 1 enemy, or, more than 2 enemy in the area
        else if (_myCapturingList.Count >= 1 && _enemyCapturingList.Count >= 1 || _enemyCapturingList.Count >= 2)
        {
            // contesting
            OnContesting();
        }
        // only 1 enemy in the area
        else if (_myCapturingList.Count <= 0 && _enemyCapturingList.Count == 1)
        {
            // enemy capturing
            OnEnemyCapturing();
        }
        // no one in the area
        else if (_myCapturingList.Count <= 0 && _enemyCapturingList.Count <= 0)
        {
            // idle
            OnIdle();
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

    private void OnIdle()
    {
        // reset the capture progess
        if (captureProgress > 0)
        {
            captureProgress -= Time.deltaTime * captureResetRate;
        }
        else
        {
            // reset capture player
            capturingPlayer = -1;
            captureProgress = 0f;
        }

        // update visual
        UpdateVisual();
    }

    private void OnMeCapturing()
    {
        // check if was previously captured
        if (capturingPlayer != _myCapturingList[0] && captureProgress > 0f)
        {
            // clear the progress
            captureProgress -= Time.deltaTime * captureErasionRate;
        }
        else
        {
            // claim new capture player 
            capturingPlayer = _myCapturingList[0];

            // increment capture progress based on time
            captureProgress += Time.deltaTime * captureRate;

            // check if the capture progress is complete
            if (captureProgress >= 1f)
            {
                OnObjectiveCaptured(capturingPlayer);
            }
        }

        // update visual
        UpdateVisual();

        print("me capturing");
    }

    private void OnEnemyCapturing()
    {
        // check if was previously captured
        if (capturingPlayer != _enemyCapturingList[0] && captureProgress > 0f)
        {
            // clear the progress
            captureProgress -= Time.deltaTime * captureErasionRate;
        }
        else
        {
            // claim new capture player 
            capturingPlayer = _enemyCapturingList[0];

            // increment capture progress based on time
            captureProgress += Time.deltaTime * captureRate;

            // check if the capture progress is complete
            if (captureProgress >= 1f)
            {
                OnObjectiveCaptured(capturingPlayer);
            }
        }

        // update visual
        UpdateVisual();

        print("enemy capturing");
    }

    private void OnContesting()
    {
        // reset the capture progess
        if (!_myCapturingList.Contains(capturingPlayer) && !_enemyCapturingList.Contains(capturingPlayer) && captureProgress > 0)
        {
            captureProgress -= Time.deltaTime * captureErasionRate;
        }
        else if(!_myCapturingList.Contains(capturingPlayer) && !_enemyCapturingList.Contains(capturingPlayer) && captureProgress <= 0)
        {
            // reset capture player
            capturingPlayer = -1;
            captureProgress = 0f;
        }

        // update visual
        UpdateVisual();

        print("contesting");
    }

    private void OnObjectiveCaptured(int playerActorNumber)
    {
        isActive = false;

        // only the capture player will send the networked sync to all
        if (playerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }

        _PV.RPC("RPC_SendObjectiveCaptureMessage", RpcTarget.AllBufferedViaServer, (byte)playerActorNumber);
    }

    private void ResetObjective()
    {
        isActive = true;
        captureProgress = 0f;
        capturingPlayer = -1;
    }

    private void UpdateVisual()
    {
        // update visual color
        if (_myCapturingList.Count >= 1 && _enemyCapturingList.Count <= 0)
        {
            // if only me
            if (capturingPlayer == _myCapturingList[0])
            {
                _progressUI.color = Color.green;
            }
            else
            {
                _progressUI.color = Color.white;
            }
        }
        else if (_myCapturingList.Count <= 0 && _enemyCapturingList.Count == 1)
        {
            // if only one enemy
            if (capturingPlayer == _enemyCapturingList[0])
            {
                _progressUI.color = Color.red;
            }
            else
            {
                _progressUI.color = Color.white;
            }
        }
        else if (_myCapturingList.Count <= 0 && _enemyCapturingList.Count <= 0)
        {
            // if no one
            _progressUI.color = Color.white;
        }
        else if (_myCapturingList.Count + _enemyCapturingList.Count >= 2)
        {
            // if contesting
            _progressUI.color = Color.yellow;
        }

        // update capture progress UI
        _progressUI.fillAmount = captureProgress;

        // update capture name UI
        if (capturingPlayer == -1)
        {
            _capturePlayerNameUI.text = "";
        }
        else
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.ActorNumber == capturingPlayer)
                {
                    _capturePlayerNameUI.text = player.NickName;
                }
            }
        }
    }
}
