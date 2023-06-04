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
        this.target = targetToTrack.transform;
        levelText.text = targetToTrack.Level.ToString();
    }

    private void Update()
    {
        if (target == null) return;

        float minX = image.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = image.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 calculatePos = Camera.main.WorldToScreenPoint(target.position);

        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 pos = calculatePos;

        if(calculatePos.x < minX)
        {
            pos.x = minX;
            pos.y = ((minX - center.x) / (calculatePos.x - center.x)) * (calculatePos.y - center.y) + center.y;
        }
        else if (calculatePos.x > maxX)
        {
            pos.x = maxX;
            pos.y = ((maxX - center.x) / (calculatePos.x - center.x)) * (calculatePos.y - center.y) + center.y;
        }

        if(calculatePos.y < minY)
        {
            pos.y = minY;
            pos.x = ((minY - center.y) / (calculatePos.y - center.y)) * (calculatePos.x - center.x) + center.x;
        }
        else if(calculatePos.y > maxY)
        {
            pos.y = maxY;
            pos.x = ((maxY - center.y) / (calculatePos.y - center.y)) * (calculatePos.x - center.x) + center.x;
        }


        image.transform.position = pos;

        image.transform.rotation = Quaternion.identity;
        
        if ((calculatePos - pos).sqrMagnitude < 0.1f) return;

        Vector2 dir = pos - center;
        float angleToRotate = Vector2.Angle(Vector2.down, dir);
        image.transform.Rotate(0f, 0f, dir.x > 0 ? angleToRotate : -angleToRotate);

    }
}
