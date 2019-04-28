using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public Image buttonImage;
    public TextMeshProUGUI label;
    public Transform worldspaceTarget;

    public void SetText(string message)
    {
        label.text = message;
    }

    // Update is called once per frame
    void Update()
    {
        buttonImage.transform.localScale = Vector3.one + Vector3.one * 0.1f * Mathf.Sin(Time.time);
        if (worldspaceTarget != null)
        {
            transform.localPosition = Utils.GetCanvasPostion(worldspaceTarget);
        }
    }

    public void Hide()
    {
        HUD.HideTooltip(this);
    }
}
