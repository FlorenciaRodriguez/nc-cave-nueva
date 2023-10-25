namespace Assets.Scripts.Input
{
  using System;

  using UnityEngine;

  [Serializable]
  public class ContinuousFloatInput
  {
    private const float Tolerance = 0.001f;

    [SerializeField]
    [Range(1, 100)]
    private float step = 1.0f;

    [SerializeField]
    [Range(1, 100)]
    private float speed = 1.0f;

    [SerializeField]
    [Range(0, 5)]
    private float waitingTime = 1.0f;

    private float currentTimePressed;

    private float previousInput;

    private float input;

    public float Input
    {
      get => this.input;
      set
      {
        this.previousInput = this.input;
        this.input = value;
      }
    }

    public float Step
    {
      get => this.step;
      set => this.step = value;
    }

    public float Speed
    {
      get => this.speed;
      set => this.speed = value;
    }

    public float WaitingTime
    {
      get => this.waitingTime;
      set => this.waitingTime = value;
    }

    public bool Update(float currentValue, float deltaTime, out float newValue)
    {
      var result = false;
      newValue = currentValue;

      if (Mathf.Abs(this.previousInput - this.input) > Tolerance)
      {
        newValue = currentValue + (this.input * this.step);
        this.previousInput = this.input;

        result = true;
      }
      else if (this.currentTimePressed > this.waitingTime)
      {
        newValue = currentValue + (this.input * this.speed * Time.deltaTime);
        result = true;
      }

      if (Mathf.Abs(this.input) > Tolerance)
      {
        this.currentTimePressed += Time.deltaTime;
      }
      else
      {
        this.currentTimePressed = 0f;
      }

      return result;
    }

    public void Reset()
    {
      this.currentTimePressed = 0f;
      this.input = 0f;
      this.previousInput = 0f;
    }
  }
}
