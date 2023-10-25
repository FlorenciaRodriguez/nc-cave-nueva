#pragma warning disable 649
namespace Assets.UI.CaveDisplay.Scripts
{
  using System.Collections.Generic;

  using Assets.Scripts.Camera.Cave;

  using TMPro;
  using UnityEngine;

  public class CaveDisplaySettingUIController : MonoBehaviour
  {
    [SerializeField]
    private TMP_Dropdown dropdownFront;

    [SerializeField]
    private TMP_Dropdown dropdownLeft;

    [SerializeField]
    private TMP_Dropdown dropdownRight;

    [SerializeField]
    private TMP_Dropdown dropdownDown;

    protected void OnEnable()
    {
      this.dropdownFront.ClearOptions();
      this.dropdownLeft.ClearOptions();
      this.dropdownRight.ClearOptions();
      this.dropdownDown.ClearOptions();

      var options = new List<string>() { "1", "2", "3", "4" };

      this.dropdownFront.AddOptions(options);
      this.dropdownFront.onValueChanged.AddListener(this.HandleDropdownFrontValueChanged);

      this.dropdownLeft.AddOptions(options);
      this.dropdownLeft.onValueChanged.AddListener(this.HandleDropdownLeftValueChanged);

      this.dropdownRight.AddOptions(options);
      this.dropdownRight.onValueChanged.AddListener(this.HandleDropdownRightValueChanged);

      this.dropdownDown.AddOptions(options);
      this.dropdownDown.onValueChanged.AddListener(this.HandleDropdownDownValueChanged);

      this.SetDropdownValues();
    }

    protected void OnDisable()
    {
      this.dropdownFront.onValueChanged.RemoveListener(this.HandleDropdownFrontValueChanged);
      this.dropdownFront.Hide();

      this.dropdownLeft.onValueChanged.RemoveListener(this.HandleDropdownLeftValueChanged);
      this.dropdownLeft.Hide();

      this.dropdownRight.onValueChanged.RemoveListener(this.HandleDropdownRightValueChanged);
      this.dropdownRight.Hide();

      this.dropdownDown.onValueChanged.RemoveListener(this.HandleDropdownDownValueChanged);
      this.dropdownDown.Hide();
    }

    protected void OnValidate()
    {
      if (this.dropdownFront == null)
      {
        Debug.LogError($"The variable '{nameof(this.dropdownFront)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }

      if (this.dropdownLeft == null)
      {
        Debug.LogError($"The variable '{nameof(this.dropdownLeft)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }

      if (this.dropdownRight == null)
      {
        Debug.LogError($"The variable '{nameof(this.dropdownRight)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }

      if (this.dropdownDown == null)
      {
        Debug.LogError($"The variable '{nameof(this.dropdownDown)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.", this.gameObject);
      }
    }

    private void HandleDropdownFrontValueChanged(int value)
    {
      CaveDisplayManager.SetDisplay(CaveCameraEnum.Front, value);
      this.SetDropdownValues();
    }

    private void HandleDropdownLeftValueChanged(int value)
    {
      CaveDisplayManager.SetDisplay(CaveCameraEnum.Left, value);
      this.SetDropdownValues();
    }

    private void HandleDropdownRightValueChanged(int value)
    {
      CaveDisplayManager.SetDisplay(CaveCameraEnum.Right, value);
      this.SetDropdownValues();
    }

    private void HandleDropdownDownValueChanged(int value)
    {
      CaveDisplayManager.SetDisplay(CaveCameraEnum.Down, value);
      this.SetDropdownValues();
    }

    private void SetDropdownValues()
    {
      this.dropdownFront.SetValueWithoutNotify(CaveDisplayManager.GetDisplay(CaveCameraEnum.Front));
      this.dropdownLeft.SetValueWithoutNotify(CaveDisplayManager.GetDisplay(CaveCameraEnum.Left));
      this.dropdownRight.SetValueWithoutNotify(CaveDisplayManager.GetDisplay(CaveCameraEnum.Right));
      this.dropdownDown.SetValueWithoutNotify(CaveDisplayManager.GetDisplay(CaveCameraEnum.Down));
    }
  }
}
