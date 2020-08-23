using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Player2 : MonoBehaviour
{

    

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Gameover")
        {
            SceneManager.LoadScene("1PWIN");
        }
        
    }

}
