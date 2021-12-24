using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemCollector : MonoBehaviour
{
    private int collectibles=0;
    [SerializeField] 
    private Text paraText;

    
    [SerializeField]
    private AudioSource collectSoundEffect;

    [SerializeField]
    private AudioSource victorySoundEffect;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Collectible"));
        {
            Destroy(collision.gameObject);
            collectSoundEffect.Play();
            collectibles++;
            paraText.text = "Para: "+collectibles;
        }

        if(collectibles>=10)
        {
           victorySoundEffect.Play();
            new WaitForSeconds(3);
           SceneManager.LoadScene(2);

        }
        
    }
}
