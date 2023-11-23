using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transparency : MonoBehaviour
{
    [Header("Canvas Group")]
    public CanvasGroup homeScreen;
    public CanvasGroup bookNIOScreen;
    public CanvasGroup createPersonalAccountScreen;
    public CanvasGroup userVerificationScreen;
    public CanvasGroup nameYourClusterScreen;


    [Header("Buttons")]
    public Button bookNio;
    public Button backBookNio;
    public Button acceptBookNio;
    public Button userVerifiction;
    public Button nameYourCluster;

    private bool isAnimating = false;

    void Start()
    {
        bookNio.onClick.AddListener(b_BookNio);
        backBookNio.onClick.AddListener(b_BackBookNio);
        acceptBookNio.onClick.AddListener(b_AcceptBookNio);
        userVerifiction.onClick.AddListener(b_userVerifiction);
        nameYourCluster.onClick.AddListener(b_nameYourCluster);
    }

    private IEnumerator changeTransparency(CanvasGroup panel,  float alphaTarget)
    {
        isAnimating = true;

        float duration = 0.75f; // Adjust the duration of the animation
        float elapsedTime = 0;

        float startAlpha = panel.alpha;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            panel.alpha = Mathf.Lerp(startAlpha, alphaTarget, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.alpha = alphaTarget;

        isAnimating = false;
    }

    private void b_BookNio()
    {
        if (isAnimating)
            return;

        StartCoroutine(changeTransparency(homeScreen, 0.4f));
        StartCoroutine(changeTransparency(bookNIOScreen, 1f));
        StartCoroutine(changeTransparency(createPersonalAccountScreen, 0.4f));
    }

    private void b_BackBookNio()
    {
        if (isAnimating)
            return;

        StartCoroutine(changeTransparency(homeScreen, 1f));
        StartCoroutine(changeTransparency(bookNIOScreen, 0.4f));
    }

    private void b_AcceptBookNio()
    {
        if (isAnimating)
            return;

        StartCoroutine(changeTransparency(createPersonalAccountScreen, 1f));
        StartCoroutine(changeTransparency(bookNIOScreen, 0.4f));
        StartCoroutine(changeTransparency(userVerificationScreen, 0.4f));
    }

    private void b_userVerifiction()
    {
        if (isAnimating)
            return;

        StartCoroutine(changeTransparency(createPersonalAccountScreen, 0.4f));
        StartCoroutine(changeTransparency(userVerificationScreen, 1f));
        StartCoroutine(changeTransparency(nameYourClusterScreen, 0.4f));
    }

    private void b_nameYourCluster()
    {
        if (isAnimating)
            return;

        StartCoroutine(changeTransparency(nameYourClusterScreen, 1f));
        StartCoroutine(changeTransparency(userVerificationScreen, 0.4f));
    }


}
