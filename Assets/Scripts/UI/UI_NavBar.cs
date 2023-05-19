using UnityEngine;
using UnityEngine.UI;

public class UI_NavBar : MonoBehaviour
{
    [SerializeField] private Button[] _tabs;
    [SerializeField] private GameObject[] _views;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SwitchTab(int whichTab)
    {
        _animator.SetInteger("NavPos", whichTab);

        for (int i = 0; i < _tabs.Length; i++)
        {
            if (i == whichTab)
            {
                _tabs[i].interactable = false;
                _views[i].SetActive(true);
            }
            else
            {
                _tabs[i].interactable = true;
                _views[i].SetActive(false);
            }
        }
    }
}
