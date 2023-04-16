using UnityEngine;
using UnityEngine.UI;

public class SliderFollowCharacter : MonoBehaviour
{
    public Transform enemyTransform;
    public Slider slider;

    void LateUpdate()
    {
        // Get the enemy's head position
        Vector3 enemyHeadPosition = enemyTransform.position + Vector3.up * 2f;

        // Set the slider's position to the enemy's head position
        slider.transform.position = Camera.main.WorldToScreenPoint(enemyHeadPosition);
    }
}