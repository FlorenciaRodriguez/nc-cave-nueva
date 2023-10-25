namespace Assets.Scripts.Player
{
  using UnityEngine;

  [RequireComponent(typeof(AudioSource))]
  [RequireComponent(typeof(CharacterController))]
  public class CharacterSoundController : MonoBehaviour
  {
    [SerializeField]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private AudioClip[] footstepAudioClips = new AudioClip[0];

    [SerializeField]
    private float stepInterval = 1;

    [SerializeField]
    private float stepMaximumVelocity = 2;

    private CharacterController characterController;

    private AudioSource audioSource;

    private float stepCycle;

    private float nextStep;

    private float velocity;

    protected void Awake()
    {
      this.characterController = this.GetComponent<CharacterController>();
      this.audioSource = this.GetComponent<AudioSource>();
    }

    protected void OnEnable()
    {
      this.stepCycle = 0.0f;
      this.nextStep = this.stepCycle + this.stepInterval;
    }

    protected void Update()
    {
      this.velocity = this.characterController.velocity.sqrMagnitude;

      if (this.velocity > 0 && this.characterController.isGrounded)
      {
        this.stepCycle += Mathf.Min(Mathf.Sqrt(this.velocity), this.stepMaximumVelocity) * Time.deltaTime;
        if (this.stepCycle > this.nextStep)
        {
          if (this.footstepAudioClips.Length > 1)
          {
            int index = Random.Range(1, this.footstepAudioClips.Length);
            this.audioSource.clip = this.footstepAudioClips[index];
            this.audioSource.PlayOneShot(this.audioSource.clip);

            this.footstepAudioClips[index] = this.footstepAudioClips[0];
            this.footstepAudioClips[0] = this.audioSource.clip;
          }
          else
          {
            this.audioSource.clip = this.footstepAudioClips[0];
            this.audioSource.PlayOneShot(this.audioSource.clip);
          }

          this.nextStep = this.stepCycle + this.stepInterval;
        }
      }
    }

    protected void OnDisable()
    {
      this.audioSource.Stop();
    }

    protected void OnValidate()
    {
      for (var i = 0; i < this.footstepAudioClips.Length; i++)
      {
        if (this.footstepAudioClips[i] == null)
        {
          Debug.LogError(
            $"The element {i} of the variable '{nameof(this.footstepAudioClips)}' of the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned",
            this.gameObject);
        }
      }
    }
  }
}
