using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashController : MonoBehaviour
{
    public Color flashColor;
    private Image flashImage;

    private void Awake()
    {
        flashImage = GetComponent<Image>();
        flashImage.enabled = false;
    }

    public void StartRandomizedFlash()
    {
        StartCoroutine(RandomizedFlash());
    }

    public void StartEEGFlash()
    {
        StartCoroutine(EEGFlash());
    }

    IEnumerator RandomizedFlash()
    {
        for (int i = 0; i < 10; i++) // 10 seconds
        {
            flashImage.color = flashColor;
            flashImage.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f)); // Random duration

            flashImage.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f)); // Random duration
        }
    }

    IEnumerator EEGFlash()
    {
        int[] hertzValues = { 1, 2, 8, 10, 15, 18, 20, 25, 40, 50, 60 };

        foreach (int hz in hertzValues)
        {
            float duration = 1f / hz; // Duration of each flash
            for (int i = 0; i < 5 * hz; i++) // 5 seconds of flashing
            {
                flashImage.color = flashColor;
                flashImage.enabled = true;
                yield return new WaitForSeconds(duration / 2);

                flashImage.enabled = false;
                yield return new WaitForSeconds(duration / 2);
            }

            yield return new WaitForSeconds(5f); // 5 second break
        }
    }
}
