using UnityEngine;

public class FogWall : MonoBehaviour
{
    public GameObject triggerObject;    
    public GameObject targetObject;     
    public GameObject targetObject2;
    public GameObject bossPack;     

    public AudioClip musicClip;

    private bool triggerActivated = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character") && !triggerActivated)
        {
            audioSource.Play();
            ActivateTargetObject();
            triggerActivated = true;
        }
    }

    private void ActivateTargetObject()
    {
        targetObject.SetActive(true);
        targetObject2.SetActive(true);
        if(bossPack != null)
        {
            bossPack.SetActive(true);
        }
        
    }
}