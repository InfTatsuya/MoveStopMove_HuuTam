using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static event EventHandler<OnStickInputValueUpdatedArg> onStickInputValueUpdated;
    public class OnStickInputValueUpdatedArg : EventArgs
    {
        public Vector2 inputVector;

        public OnStickInputValueUpdatedArg(Vector2 input)
        {
            inputVector = input;
        }

        public OnStickInputValueUpdatedArg()
        {
            inputVector = Vector2.zero;
        }
    }


    [SerializeField] RectTransform thumbStickTransform;
    [SerializeField] Image thumbImage;
    [SerializeField] RectTransform backgroundTransform;
    [SerializeField] Image backgroundImage;
    [SerializeField] RectTransform centerTransform;


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = eventData.position;
        Vector2 centerPos = backgroundTransform.position;

        Vector2 localOffset = Vector2.ClampMagnitude(touchPos - centerPos, backgroundTransform.sizeDelta.x / 2);
        thumbStickTransform.position = centerPos + localOffset;

        Vector2 inputVector = localOffset / (backgroundTransform.sizeDelta.x / 2);
        onStickInputValueUpdated?.Invoke(this, new OnStickInputValueUpdatedArg(inputVector));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundTransform.position = eventData.position;
        thumbStickTransform.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        thumbStickTransform.localPosition = Vector2.zero;
        backgroundTransform.localPosition = Vector2.zero;
        onStickInputValueUpdated?.Invoke(this, new OnStickInputValueUpdatedArg());
    }
}
