namespace Assets.Scripts.Input
{
  using System;

  using UnityEngine;

  [Serializable]
  public class ContinuousIntInput
  {
    [SerializeField]
    [Range(1, 100)]
    private int step = 1;

    [SerializeField]
    [Range(1, 100)]
    private int speed = 1;

    [SerializeField]
    [Range(0, 5)]
    private float waitingTime = 1.0f;

    private float currentTimePressed;

    private float currentTimeStep;

    private int previousInput;

    private int input;

    public int Input
    {
      get => this.input;
      set
      {
        this.previousInput = this.input;
        this.input = value;
      }
    }

    public int Step
    {
      get => this.step;
      set => this.step = value;
    }

    public int Speed
    {
      get => this.speed;
      set => this.speed = value;
    }

    public float WaitingTime
    {
      get => this.waitingTime;
      set => this.waitingTime = value;
    }

    public bool Update(int currentValue, float deltaTime, out int newValue)
    {
      var result = false;
      newValue = currentValue;

      if (this.previousInput != this.input)
      {
        newValue = currentValue + (this.input * this.step);
        this.previousInput = this.input;

        this.currentTimeStep = 0f;

        result = true;
      }
      else if (this.currentTimePressed > this.waitingTime)
      {
        this.currentTimeStep += Time.deltaTime;

        if (this.currentTimeStep >= 1f / this.speed)
        {
          newValue = currentValue + this.input;
          result = true;

          this.currentTimeStep = 0f;
        }
      }

      if (this.input != 0)
      {
        this.currentTimePressed += Time.deltaTime;
      }
      else
      {
        this.currentTimePressed = 0f;
        this.currentTimeStep = 0f;
      }

      return result;
    }

    public void Reset()
    {
      this.currentTimePressed = 0f;
      this.currentTimeStep = 0f;
      this.input = 0;
      this.previousInput = 0;
    }
  }
}