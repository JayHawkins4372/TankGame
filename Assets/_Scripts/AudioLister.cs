using UnityEngine;

public class AudioLister : MonoBehaviour
{
    Transform player;
    Transform cam;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (cam == null && Camera.main != null) cam = Camera.main.transform;
        if (player == null) return;

        //This will handle distance between the player and enemy tank
        transform.position = player.position;
        if (cam != null)
            transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);

    }
}
