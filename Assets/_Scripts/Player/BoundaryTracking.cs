//Author: Wade lawler
//Last Modified: 11/17/26
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class BoundaryTracking : MonoBehaviour
{
    public float timer = 10.0f;
    private bool isOut = false;
    private float startTime;
    public TextMeshProUGUI timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = timer;
        timerText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {

            if (other.gameObject.CompareTag("Bounds") || other.transform.root.CompareTag("Bounds"))
            {
                Debug.Log("Collided with boundary");
            OutOfBounds(false);
            } else if (other.gameObject.CompareTag("Bounds2") || other.transform.root.CompareTag("Bounds2")) {
            Debug.Log("Collided with second boundary");
            OutOfBounds(true);
        }
    }

    private void OutOfBounds(bool outState)
    {
        isOut = outState;
        Timer(isOut);
    }

    private void Timer(bool run)
    {
        StopAllCoroutines();

        if (run)
        {
            timerText.gameObject.SetActive(true);
            StartCoroutine(RunTimer());
        }
        else
        {
            timerText.gameObject.SetActive(false);
            timer = startTime;
        }
    }

    private IEnumerator RunTimer()
    {
        while (timer > 0f)
        {
            Debug.Log("Time left: " + timer);
            timerText.text = "Return to bounds: " + timer.ToString("0");
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        timer = 0f;
        Debug.Log("Timer hit 0");
        SceneManager.LoadScene("GameOver");
    }
}
