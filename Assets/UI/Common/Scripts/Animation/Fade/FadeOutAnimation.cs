namespace Assets.UI.Common.Scripts.Animation.Fade
{
  using System.Collections;

  using UnityEngine;

  public class FadeOutAnimation : FadeAnimation
  {
    public override void StartAnimation()
    {
      this.StopAllCoroutines();
      this.StartCoroutine(this.StartAnimationCoroutine(this.IsScaleAnimationEnabled));
    }

    public override void StopAnimation()
    {
      this.StopAllCoroutines();

      this.OnAnimationFinished();
    }

    public override void ForceInitialState()
    {
      this.TargetCanvasGroup.alpha = 1.0f;
      this.TargetRectTransform.localScale = Vector3.one;
    }

    public override void ForceFinalState()
    {
      this.TargetCanvasGroup.alpha = 0.0f;
      this.TargetRectTransform.localScale = Vector3.zero;
    }

    private IEnumerator StartAnimationCoroutine(bool animateScale)
    {
      this.OnAnimationStarted();

      if (animateScale)
      {
        this.TargetRectTransform.localScale = Vector3.one;
      }

      var currentTime = 0f;
      while (currentTime <= this.AnimationTime)
      {
        var value = Mathf.InverseLerp(this.AnimationTime, 0, currentTime);

        if (animateScale)
        {
          Vector3 scale;
          scale.x = value;
          scale.y = value;
          scale.z = value;

          this.TargetRectTransform.localScale = scale;
        }

        this.TargetCanvasGroup.alpha = value;

        yield return null;

        currentTime += Time.unscaledDeltaTime;
      }

      this.TargetCanvasGroup.alpha = 0.0f;

      if (animateScale)
      {
        this.TargetRectTransform.localScale = Vector3.zero;
      }

      this.OnAnimationFinished();
    }
  }
}
