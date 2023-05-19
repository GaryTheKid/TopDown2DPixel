using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using NetworkCalls;

public class Objective : MonoBehaviour
{
    public int captureValue;
    public List<int> _myCapturingList;
    public List<int> _enemyCapturingList;
    public bool isActive;
    public float captureProgress;
    public int capturingPlayer;

    [SerializeField] private Color _objectiveActiveColor;
    [SerializeField] private Color _objectiveInactiveColor;
    [SerializeField] private SpriteRenderer _objectiveSprite;
    [SerializeField] private SpriteRenderer _objectiveSprite_Minimap;
    [SerializeField] private Image _progressUI;
    [SerializeField] private Image _progressUI_Minimap;
    [SerializeField] private GameObject _contestingUI;
    [SerializeField] private GameObject _meCapturingUI;
    [SerializeField] private GameObject _enemyCapturingUI;
    [SerializeField] private Text _capturePlayerNameUI;

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
    }

    // Update is called once per frame
    void Update()
    {
        // update Visual
        UpdateVisual();

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

        Objective_Network.SendObjectiveCaptureMessage(_PV, (byte)playerActorNumber);
    }

    public void ResetAndActivateObjective()
    {
        Objective_Network.ResetAndActivateObjective(_PV);
    }

    public void UpdateVisual()
    {
        // update obj active/inactive color
        if (isActive)
        {
            _objectiveSprite.color = _objectiveActiveColor;
            _objectiveSprite_Minimap.color = _objectiveActiveColor;
        }
        else
        {
            capturingPlayer = -1;
            captureProgress = 0f;
            _capturePlayerNameUI.text = "";
            _progressUI.fillAmount = captureProgress;
            _progressUI_Minimap.fillAmount = captureProgress;
            _progressUI.color = Color.white;
            _progressUI_Minimap.color = Color.white;
            _contestingUI.SetActive(false);
            _meCapturingUI.SetActive(false);
            _enemyCapturingUI.SetActive(false);
            _objectiveSprite.color = _objectiveInactiveColor;
            _objectiveSprite_Minimap.color = _objectiveInactiveColor;
            return;
        }

        // update visual color
        if (_myCapturingList.Count >= 1 && _enemyCapturingList.Count <= 0)
        {
            // if only me
            if (capturingPlayer == _myCapturingList[0])
            {
                _progressUI.color = Color.green;
                _progressUI_Minimap.color = Color.green;
            }
            else
            {
                _progressUI.color = Color.grey;
                _progressUI_Minimap.color = Color.grey;
            }
            _contestingUI.SetActive(false);
            _meCapturingUI.SetActive(true);
            _enemyCapturingUI.SetActive(false);
        }
        else if (_myCapturingList.Count <= 0 && _enemyCapturingList.Count == 1)
        {
            // if only one enemy
            if (capturingPlayer == _enemyCapturingList[0])
            {
                _progressUI.color = Color.red;
                _progressUI_Minimap.color = Color.red;
            }
            else
            {
                _progressUI.color = Color.grey;
                _progressUI_Minimap.color = Color.grey;
            }
            _contestingUI.SetActive(false);
            _meCapturingUI.SetActive(false);
            _enemyCapturingUI.SetActive(true);
        }
        else if (_myCapturingList.Count <= 0 && _enemyCapturingList.Count <= 0)
        {
            // if no one
            _progressUI.color = Color.grey;
            _progressUI_Minimap.color = Color.grey;
            _contestingUI.SetActive(false);
            _meCapturingUI.SetActive(false);
            _enemyCapturingUI.SetActive(false);
        }
        else if (_myCapturingList.Count + _enemyCapturingList.Count >= 2)
        {
            // if contesting
            _progressUI.color = Color.yellow;
            _progressUI_Minimap.color = Color.yellow;
            _contestingUI.SetActive(true);
            _meCapturingUI.SetActive(false);
            _enemyCapturingUI.SetActive(false);
        }

        // update capture progress UI
        _progressUI.fillAmount = captureProgress;
        _progressUI_Minimap.fillAmount = captureProgress;

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