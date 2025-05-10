using UnityEngine;
using UnityEngine.UI;

public class ResponsiveUILayoutSwitcher : MonoBehaviour
{
    [Header("Containers")]
    [SerializeField] private GameObject portraitContainer;
    [SerializeField] private GameObject landscapeContainer;

    private bool isPortrait;

    void Start()
    {
        UpdateLayout();
    }

    void Update()
    {
        if ((Screen.height > Screen.width) != isPortrait)
        {
            UpdateLayout();
        }
    }

    void UpdateLayout()
    {
        isPortrait = Screen.height > Screen.width;

        if (portraitContainer != null && landscapeContainer != null)
        {
            portraitContainer.SetActive(isPortrait);
            landscapeContainer.SetActive(!isPortrait);
        }
    }
}