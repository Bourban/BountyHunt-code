using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbleweed : MonoBehaviour
{
    [SerializeField]
    bool ShouldMove;
    [SerializeField, Range(0, 1)]
    float WindStrengthModifier;

    Rigidbody RB;

    Vector3 WindDirection = Vector3.zero;
    float WindSpeed;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();

        if (ShouldMove)
        {
            StartCoroutine(ChangeWindDirection());
        }
    }

    // Update is called once per frame
    void Update()
    {
        RB.AddForce((WindDirection * WindSpeed * WindStrengthModifier) * Time.deltaTime);
    }

    IEnumerator ChangeWindDirection()
    {
        //Vector3 next
        WindDirection = new Vector3(Random.Range(-180, 180), 0, Random.Range(-180, 180));

        //WindDirection = Vector3.Lerp(WindDirection, nextWindDirection, 0.4f);

        WindSpeed = Random.Range(0.01f, 0.2f);

        float windChangeTime = Random.Range(5, 10);

        yield return new WaitForSeconds(windChangeTime);

        yield return ChangeWindDirection();
    }

}
