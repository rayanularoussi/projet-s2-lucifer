using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalBoss : MonoBehaviour
{
    public EnemyStats heart1;
    public EnemyStats heart2;
    public EnemyStats heart3;

    public GameObject heart4;
    public GameObject god;
    public EnemyStats ESheart3;
    public GameObject final;
    private bool activated = false;
    public GameObject youWin;

    private void Update()
    {
        if(!heart1.bossOrNot && !heart2.bossOrNot && !heart3.bossOrNot && !activated)
        {
            heart4.SetActive(true);
            activated = true;
            final.SetActive(true);
        }
        else if(activated && !ESheart3.bossOrNot)
        {
            Destroy(god);
            final.SetActive(false);
            youWin.SetActive(true);
        }
    }
}
