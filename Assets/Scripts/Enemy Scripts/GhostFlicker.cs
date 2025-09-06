using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFlicker : MonoBehaviour
{
    public GameObject lightPanel;              // Reference to the light panel GameObject
    public SpriteRenderer ghostSpriteRenderer; // Sprite Renderer for the ghost
    public Transform player;             // Reference to the player's Transform
    public float closeDistance = 2.5f;     // Distance for close
    public float nearDistance = 10f;      // Distance for near
    public float mediumDistance = 20f;   // Distance for medium

    private void Start()
    {
        lightPanel.SetActive(false);
        ghostSpriteRenderer.enabled = true;

        // Start the flicker coroutine
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            // Distance ghost is from player
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= closeDistance)
            {
                lightPanel.SetActive(true);
                ghostSpriteRenderer.enabled = true;
                yield return null;
            }

            else if (distance <= nearDistance)
            {
                // Lights OFF
                lightPanel.SetActive(true);
                ghostSpriteRenderer.enabled = true; // Make ghost visible
                yield return new WaitForSeconds(0.7f);

                // Lights ON
                lightPanel.SetActive(false);
                ghostSpriteRenderer.enabled = false; // Make ghost invisible
                yield return new WaitForSeconds(0.7f);
            }

            else if (distance <= mediumDistance)
            {
                // Lights OFF
                lightPanel.SetActive(true);
                ghostSpriteRenderer.enabled = true; // Make ghost visible
                yield return new WaitForSeconds(0.7f);

                // Lights ON
                lightPanel.SetActive(false);
                ghostSpriteRenderer.enabled = false; // Make ghost invisible
                yield return new WaitForSeconds(3f);
            }

            else
            {
                // Outside ghost range
                lightPanel.SetActive(false);
                yield return null;
            }
        }
    }
}
