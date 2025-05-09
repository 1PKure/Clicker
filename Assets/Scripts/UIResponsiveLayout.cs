using UnityEngine;
using UnityEngine.UI;

public class UIResponsiveLayout : MonoBehaviour
{
    private HorizontalOrVerticalLayoutGroup layoutGroup;

    private bool isPortrait;

    void Start()
    {
        UpdateLayout();
    }

    void Update()
    {
        bool currentPortrait = Screen.height > Screen.width;
        if (currentPortrait != isPortrait)
        {
            isPortrait = currentPortrait;
            UpdateLayout();
        }
    }

    void UpdateLayout()
    {
        var existing = GetComponent<HorizontalOrVerticalLayoutGroup>();
        if (existing != null) DestroyImmediate(existing);

        if (Screen.height > Screen.width)
        {
            layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
        }
        else
        {
            layoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
        }

        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.spacing = 20f;
        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.padding = new RectOffset(20, 20, 20, 20);

        var fitter = GetComponent<ContentSizeFitter>();
        if (fitter == null)
            fitter = gameObject.AddComponent<ContentSizeFitter>();

        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}
