using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlobalAnnouncementManager : MonoBehaviour
{
    #region Fields
    // singleton
    public static GlobalAnnouncementManager singleton;

    // private fields
    [SerializeField] private Transform _announcementTemplate;
    [SerializeField] private Transform _centerTextTemplate;
    #endregion

    private void Awake()
    {
        singleton = this;
    }

    public void PlayAnnouncement(string announcement)
    {
        // instantiate announcement text
        var announcementText = Instantiate(_announcementTemplate, transform);
        announcementText.gameObject.SetActive(true);

        // set text
        announcementText.GetComponent<TextMeshProUGUI>().text = announcement;
    }

    public void PlayCenterAnnouncement(string announcement)
    {
        // instantiate announcement text
        var announcementText = Instantiate(_centerTextTemplate, transform);
        announcementText.gameObject.SetActive(true);

        // set text
        announcementText.GetComponent<TextMeshProUGUI>().text = announcement;
    }
}
