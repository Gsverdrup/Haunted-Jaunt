using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;

    public Image redFlashImage;
    private Color color;

    void Start() {
        redFlashImage = GameObject.Find("RedFlash")?.GetComponent<Image>();
        color = redFlashImage.color;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            
            if (Physics.Raycast (ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    gameEnding.CaughtPlayer ();
                }
            }
        }

        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 enemyForward = transform.forward;
        float dot = Vector3.Dot(enemyForward, toPlayer);
        float distPlayer = Vector3.Distance(player.position, transform.position);

        if (dot > 0.75f && distPlayer < 5.0f) {
            // When player is in line of sight and in a certain distance, flash red screen
            float flashAlpha = Mathf.PingPong(Time.time * 2f, 0.5f);
            redFlashImage.color = new Color(1f, 0f, 0f, flashAlpha);
        } else {
            // When player leaves, fade red screen
            Color currentColor = redFlashImage.color;
            float fadedAlpha = Mathf.Lerp(currentColor.a, 0f, Time.deltaTime * 2f);
            redFlashImage.color = new Color(1f, 0f, 0f, fadedAlpha);
        }
    }
}
