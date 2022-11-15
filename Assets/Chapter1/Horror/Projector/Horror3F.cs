using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror3F : MonoBehaviour
{
    [SerializeField] private GameObject horrorObj = null;
    [SerializeField] private Light spot = null;
    [SerializeField] private GameObject shadow = null;
    [SerializeField] private GameObject real = null;
    private void Start()
    {
        horrorObj.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            horrorObj.SetActive(true);
            real.SetActive(false);
            StartCoroutine(StartHorror());
            Destroy(this.GetComponent<Collider>());
        }
    }
    private IEnumerator StartHorror()
    {
        yield return new WaitForSecondsRealtime(3f);
        spot.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        real.SetActive(true);
        shadow.SetActive(false);
        yield return new WaitForSecondsRealtime(1.5f);
        Destroy(horrorObj);
        Destroy(this.gameObject);
    }
}
