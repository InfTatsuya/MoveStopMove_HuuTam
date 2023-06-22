using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Indicator : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Transform target;
    [SerializeField] TextMeshProUGUI levelText;

    public void SetupIndicator(Character targetToTrack)
    {
        this.target = targetToTrack.AttachIndicatorPoint;
        levelText.text = targetToTrack.Level.ToString();
    }

    private void Update()
    {
        if (target == null) return;

        float minX = image.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = image.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector3 calculatePos = Camera.main.WorldToScreenPoint(target.position);

        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector3 pos = calculatePos;

        int i = 0;
        while(!IsOnScreen(pos, minX, maxX, minY, maxY) && i < 2)
        {
            if (pos.x < minX)
            {
                pos.x = minX;
                pos.y = ((minX - center.x) / (calculatePos.x - center.x)) * (calculatePos.y - center.y) + center.y;
            }
            else if (pos.x > maxX)
            {
                pos.x = maxX;
                pos.y = ((maxX - center.x) / (calculatePos.x - center.x)) * (calculatePos.y - center.y) + center.y;
            }
            else if (pos.y < minY)
            {
                pos.y = minY;
                pos.x = ((minY - center.y) / (calculatePos.y - center.y)) * (calculatePos.x - center.x) + center.x;
            }
            else if (pos.y > maxY)
            {
                pos.y = maxY;
                pos.x = ((maxY - center.y) / (calculatePos.y - center.y)) * (calculatePos.x - center.x) + center.x;
            }

            if(pos.z < 0)
            {
                pos *= -1;
            }
            i++;
        }   

        image.transform.position = pos;

        image.transform.rotation = Quaternion.identity;
        
        if ((calculatePos - pos).sqrMagnitude < 0.1f) return;

        Vector2 dir = new Vector2(pos.x, pos.y) - center;
        float angleToRotate = Vector2.Angle(Vector2.down, dir);
        image.transform.Rotate(0f, 0f, dir.x > 0 ? angleToRotate : -angleToRotate);

    }

    private bool IsOnScreen(Vector2 position, float minX, float maxX, float minY, float maxY)
    {
        return (position.x > minX && position.x < maxX && position.y > minY && position.y < maxY);
    }
}
