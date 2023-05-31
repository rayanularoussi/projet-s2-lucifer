using UnityEngine;

public class FogTp : MonoBehaviour
{
    public GameObject triggerObject;    
    public GameObject targetObject;     
    public GameObject targetObject2;
    public GameObject bossPack;     
    public Vector3 tp = new Vector3(12, 120, -351);

    public Transform player;

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
            player.position = tp;
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