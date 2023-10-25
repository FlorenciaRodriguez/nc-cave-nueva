#pragma warning disable 649
namespace Assets.Scripts.Environment
{
  using System.Collections.Generic;

  using Assets.Scripts.Environment.ScriptableObjects;

  using MediaLab.NetworkEntities.Simpa;

  using UnityEngine;
  using UnityEngine.Events;
  using UnityEngine.Rendering;
  using UnityEngine.Rendering.HighDefinition;

  public class EnvironmentManager : MonoBehaviour
  {
    public const string Tag = "Environment";

    [SerializeField]
    private Light directionalLight;

    [SerializeField]
    private LensFlareComponentSRP lensFlare;

    [SerializeField]
    private Volume postProcessVolume;

    [Space]

    [SerializeField]
    // ReSharper disable once CollectionNeverUpdated.Local
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private List<EnvironmentConditionProfile> environmentConditionProfiles = new List<EnvironmentConditionProfile>();

    private HDAdditionalLightData lightData;

    public event UnityAction<EnvironmentManager> EnvironmentConditionChanged;

    public Constants.MomentsOfTheDayEnum? CurrentDayTime { get; private set; }

    public bool SetEnvironmentCondition(Constants.MomentsOfTheDayEnum dayTime)
    {
      foreach (var profile in this.environmentConditionProfiles)
      {
        if (profile.DayTime == dayTime)
        {
          this.ApplyEnvironmentConditionProfile(profile);
        }
      }
     
      return false;
    }

    public void RequestShadowMapRendering()
    {
      this.lightData?.RequestShadowMapRendering();
    }

    protected void OnValidate()
    {
      if (this.directionalLight == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.directionalLight)}' of the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned",
          this.gameObject);
      }

      if (this.postProcessVolume == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.postProcessVolume)}' of the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned",
          this.gameObject);
      }

      for (var i = 0; i < this.environmentConditionProfiles.Count; i++)
      {
        if (this.environmentConditionProfiles[i] == null)
        {
          Debug.LogError(
            $"The element '{i}' of the list '{nameof(this.environmentConditionProfiles)}' of the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned",
            this.gameObject);
        }
      }
    }

    private void ApplyEnvironmentConditionProfile(EnvironmentConditionProfile profile)
    {
      this.ConfigurePostProcessing(profile);
      this.ConfigureLighting(profile);
      this.ConfigureLensFlare(profile);

      if (this.CurrentDayTime != profile.DayTime)
      {
        this.CurrentDayTime = profile.DayTime;

        this.EnvironmentConditionChanged?.Invoke(this);
      }
    }

    private void ConfigurePostProcessing(EnvironmentConditionProfile profile)
    {
      this.postProcessVolume.profile = profile.PostProcessingProfile;
    }

    private void ConfigureLighting(EnvironmentConditionProfile profile)
    {
      this.directionalLight.enabled = profile.EnableDirectionLight;

      this.lightData = this.directionalLight.GetComponent<HDAdditionalLightData>();
      this.lightData.transform.eulerAngles = profile.DirectionLightRotation;
      this.lightData.intensity = profile.LightIntensity;
      this.lightData.color = profile.LightColor;
    }

    private void ConfigureLensFlare(EnvironmentConditionProfile profile)
    {
      this.lensFlare.lensFlareData = profile.LensFlareData;
      this.lensFlare.intensity = profile.LensFlareIntensity;
      this.lensFlare.scale = profile.LensFlareScale;
      this.lensFlare.enabled = profile.LensFlareData != null;
    }
  }
}