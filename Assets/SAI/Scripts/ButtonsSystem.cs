using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSystem : MonoBehaviour
{
    [Header("Home")]
    [SerializeField] GameObject HomeScreen;
    [Header("BookNIO")]
    [SerializeField] GameObject BookNIOScreen;
    [Header("CreateAccount")]
    [SerializeField] GameObject CreatePersonalAccountScreen;
    [Header("UserVerification")]
    [SerializeField] GameObject UserVerificationScreen;
    [Header("NameYourCluster")]
    [SerializeField] GameObject NameYourClusterScreen;

    [Header("Buttons")]
    public Button bookNio;
    public Button backBookNio;
    public Button acceptBookNio;
    public Button userVerifiction;
    public Button nameYourCluster;

    [Header("Create Personal Account UI")]
    [SerializeField] TMP_InputField EmailInput;
    [SerializeField] TMP_InputField CountryInput;

    [Header("User Verification UI")]
    [SerializeField] TMP_InputField VerificationCodeInput;

    [Header("Name Your Cluster UI")]
    [SerializeField] TMP_InputField ClusterNameInput;
    [SerializeField] TMP_InputField NodeNameInput;

    private bool isAnimating = false;

    void Start()
    {
        bookNio.onClick.AddListener(b_BookNio);
        backBookNio.onClick.AddListener(b_BackBookNio);
        acceptBookNio.onClick.AddListener(b_AcceptBookNio);
        userVerifiction.onClick.AddListener(b_userVerifiction);
        nameYourCluster.onClick.AddListener(b_nameYourCluster);
        BookNIOScreen.transform.position = new Vector3(2274.01f, 516.24f, -205f);
        BookNIOScreen.transform.rotation = new Quaternion(0f, 0.1760f, 0f, 0.98534f);
    }

    private void Update()
    {
        //Debug.Log(CreatePersonalAccountScreen.transform.position);
    }

    private IEnumerator MoveAndRotate(GameObject target, Vector3 targetPosition, Quaternion targetRotation)
    {
        isAnimating = true;

        float duration = 0.75f; // Adjust the duration of the animation
        float elapsedTime = 0;

        Vector3 startPosition = target.transform.position;
        Quaternion startRotation = target.transform.rotation;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            target.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            target.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.transform.position = targetPosition;
        target.transform.rotation = targetRotation;

        isAnimating = false;
    }

    private void b_BookNio()
    {
        if (isAnimating)
            return;

        StartCoroutine(MoveAndRotate(HomeScreen, new Vector3(-354.0f, 516.24f, -205f), new Quaternion(0f, -0.17429f, 0, 0.98470f)));
        StartCoroutine(MoveAndRotate(BookNIOScreen, new Vector3(960.01f, 516.24f, 0f), Quaternion.identity));
        StartCoroutine(MoveAndRotate(CreatePersonalAccountScreen, new Vector3(2274.01f, 516.24f, -205f), new Quaternion(0f, 0.1760f, 0f, 0.98534f)));
        
        //BookNIOScreen.SetActive(true);
        CreatePersonalAccountScreen.SetActive(true);
    }

    private void b_BackBookNio()
    {
        if (isAnimating)
            return;

        StartCoroutine(MoveAndRotate(HomeScreen, new Vector3(960.01f, 516.24f, 0f), Quaternion.identity));
        StartCoroutine(MoveAndRotate(BookNIOScreen, new Vector3(2274.01f, 516.24f, -205f), new Quaternion(0f, 0.1760f, 0f, 0.98534f)));
        StartCoroutine(MoveAndRotate(CreatePersonalAccountScreen, new Vector3(2718f, 516.15f, 0f), Quaternion.identity));
        
        CreatePersonalAccountScreen.SetActive(false);
    }

    private void b_AcceptBookNio()
    {
        if (isAnimating)
            return;

        StartCoroutine(MoveAndRotate(HomeScreen, new Vector3(-1418.0f, 516.24f, -594.0f), Quaternion.identity));
        StartCoroutine(MoveAndRotate(BookNIOScreen, new Vector3(-354.0f, 516.24f, -205f), new Quaternion(0f, -0.17429f, 0, 0.98470f)));
        StartCoroutine(MoveAndRotate(CreatePersonalAccountScreen, new Vector3(960.01f, 516.24f, 0f), Quaternion.identity));
        StartCoroutine(MoveAndRotate(UserVerificationScreen, new Vector3(2274.01f, 516.24f, -205f), new Quaternion(0f, 0.1760f, 0f, 0.98534f)));

        UserVerificationScreen.SetActive(true);
        HomeScreen.SetActive(false);

        
    }

    private void b_userVerifiction()
    {
        if (isAnimating)
            return;
        
        StartCoroutine(MoveAndRotate(BookNIOScreen, new Vector3(-1418.0f, 516.24f, -594.0f), Quaternion.identity));
        StartCoroutine(MoveAndRotate(CreatePersonalAccountScreen, new Vector3(-354.0f, 516.24f, -205f), new Quaternion(0f, -0.17429f, 0, 0.98470f)));
        StartCoroutine(MoveAndRotate(UserVerificationScreen, new Vector3(960.01f, 516.24f, 0f), Quaternion.identity));
        StartCoroutine(MoveAndRotate(NameYourClusterScreen, new Vector3(2274.01f, 516.24f, -205f), new Quaternion(0f, 0.1760f, 0f, 0.98534f)));

        BookNIOScreen.SetActive(false);
        NameYourClusterScreen.SetActive(true);
        BookNIOScreen.SetActive(false);
    }

    private void b_nameYourCluster()
    {
        StartCoroutine(MoveAndRotate(CreatePersonalAccountScreen, new Vector3(-1418.0f, 516.24f, -594.0f), Quaternion.identity));
        StartCoroutine(MoveAndRotate(UserVerificationScreen, new Vector3(-354.0f, 516.24f, -205f), new Quaternion(0f, -0.17429f, 0, 0.98470f)));
        StartCoroutine(MoveAndRotate(NameYourClusterScreen, new Vector3(960.01f, 516.24f, 0f), Quaternion.identity));
        
        CreatePersonalAccountScreen.SetActive(false);
    }

}