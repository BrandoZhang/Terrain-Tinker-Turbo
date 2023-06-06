using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownhillGem : MonoBehaviour {

    public ParticleSystem effect;
    public GameObject effPrefab;

    
    private void OnTriggerEnter(Collider other)
    {
        if (effPrefab)
        {
            //Instantiate(effPrefab, transform.position, Quaternion.identity);
            GameObject eff;
            eff = Instantiate(effPrefab, transform.position, Quaternion.identity);
            ParticleSystem peff = eff.GetComponent<ParticleSystem>();
            peff.Play();
            Destroy(gameObject);
        }
    }
}
