using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour {

    public bool isSuperdot = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            if (isSuperdot)
            {
                //TODO
                //让吃豆人吃掉，吃豆人变成超级吃豆人
                
                GameManager.Instance.OnEatPacdot(gameObject);
                
                GameManager.Instance.OnEatSuperPacdot();
                Destroy(gameObject);

            }
        
            else
            {
                GameManager.Instance.OnEatPacdot(gameObject);
                Destroy(gameObject);
            }
        }
    }

    void Start () {
		
	}
	
	void Update () {
		
	}
}
