using System.IO;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        // Spawn the player object across the network
        Vector2 randomPosition = new(Random.Range(minX, maxX), Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), randomPosition, Quaternion.identity);
        
        // Spawn the camera object across the network
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Camera Manager"), Vector3.zero, Quaternion.identity);
    }
}