using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    [Header("Set Dinamically")]
    public Text scoreGt;

    private void Start()
    {
        GameObject scoreGo = GameObject.Find("ScoreCounter");
        scoreGt = scoreGo.GetComponent<Text>();
        scoreGt.text = "0";

    }
    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
        
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject collideWith = coll.gameObject;
        if(collideWith.tag=="Apple")
        {
            Destroy(collideWith);
            int score = int.Parse(scoreGt.text);
            score += 100;
            scoreGt.text = score.ToString();

            if(score> HighScore.score)
            {
                HighScore.score = score;
            }
        }
    }
}
