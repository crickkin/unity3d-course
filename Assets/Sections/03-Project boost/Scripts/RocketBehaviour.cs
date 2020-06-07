using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketBehaviour : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [Header("")]
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip deathSFX;

    [Header("")]
    [SerializeField] ParticleSystem mainEngineVFX;
    [SerializeField] ParticleSystem successVFX;
    [SerializeField] ParticleSystem deathVFX;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        state = State.Alive;
    }
    
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            Rotate();
        }
    }

    void OnCollisionEnter (Collision other)
    {
        if (state != State.Alive)
            return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                StartSuccessSequence();

                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        StopEffects(successSFX, successVFX);
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        StopEffects(deathSFX, deathVFX);
        Invoke("ReloadScene", levelLoadDelay);
    }

    private void StopEffects(AudioClip sfx, ParticleSystem vfx)
    {
        audioSource.Stop();
        mainEngineVFX.Stop();
        audioSource.PlayOneShot(sfx);
        vfx.Play();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex != SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(sceneIndex + 1);
        else
            SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
            mainEngineVFX.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * (Time.deltaTime * 100));

        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        mainEngineVFX.Play();
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }
}
