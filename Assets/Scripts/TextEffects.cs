using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffects : MonoBehaviour
{
    [SerializeField]
    private Vector2 growthFactor;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(TextEffect(1.0f));
    }
    private IEnumerator TextEffect(float time)
    {
        yield return new WaitForSeconds(2f);
        if (anim != null)
        {
            anim.enabled = false;
        }

        Vector3 originScale;
        originScale = GetComponent<RectTransform>().localScale;
        Vector2 destinationScale;
        destinationScale = growthFactor;
        while (true)
        {
            
            float currentTime = 0;
            do
            {
                GetComponent<RectTransform>().localScale = Vector2.Lerp(originScale, destinationScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;

            } while (currentTime <= time);
            currentTime = 0;
            do
            {
                GetComponent<RectTransform>().localScale = Vector2.Lerp(destinationScale, originScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;

            } while (currentTime <= time);

        }

    }
}
