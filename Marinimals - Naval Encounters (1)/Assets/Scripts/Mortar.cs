using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelfDestruct(float delay)
    {
        StartCoroutine(Destruct(delay));
    }

    public IEnumerator Destruct(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
