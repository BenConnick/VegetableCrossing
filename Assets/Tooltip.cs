using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public Image buttonImage;
    public TextMeshProUGUI label;

    private Transform worldspaceTarget;
    private RectTransform RTransform;

    public void SetTarget(Transform wsTarget)
    {
        worldspaceTarget = wsTarget;
        RTransform = (RectTransform)transform;
        MoveToTarget();
    }

    public void SetText(string message)
    {
        label.text = message;
    }

    // Update is called once per frame
    void Update()
    {
        buttonImage.transform.localScale = Vector3.one + Vector3.one * 0.1f * Mathf.Sin(Time.time);
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (worldspaceTarget != null)
        {
            RTransform.anchoredPosition = Utils.GetCanvasPostion(worldspaceTarget);
        }
    }

    public void Hide()
    {
        HUD.HideTooltip(this);
    }
}
