namespace Assets.Scripts.WindowOnTopMost
{
  using System;
  using System.Collections;
  using System.Diagnostics.CodeAnalysis;

  using MediaLab.Common.WindowOnTop;

  using UnityEngine;

  [Serializable]
  public class WindowOnTopMostController : MonoBehaviour
  {
    private static WindowOnTopMostController singleton;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
    [Tooltip("Start a coroutine for attempt to bring window to most top")]
    [SerializeField]
    private bool isStartCoroutine = true;

    [SerializeField]
    [Range(0.0f, 60.0f)]
    [Tooltip("Waiting time between attempts for bring window to most top. Expressed in seconds")]
    private float waitTime = 1.0f;

    [SerializeField]
    [Tooltip("Maximum time attempting to bring window to most top. Expressed in seconds")]
    [Range(0.0f, 60.0f)]
    private float maximumTime = 10.0f;

    private float timeStarted;

    public bool IsStartCoroutine
    {
      get
      {
        return this.isStartCoroutine;
      }

      set
      {
        this.isStartCoroutine = value;
      }
    }

    public float WaitTime
    {
      get
      {
        return this.waitTime;
      }

      set
      {
        this.waitTime = value;
      }
    }

    public float MaximumTime
    {
      get
      {
        return this.maximumTime;
      }

      set
      {
        this.maximumTime = value;
      }
    }

    protected void Awake()
    {
      if (singleton != null)
      {
        // ReSharper disable once ArrangeStaticMemberQualifier
        GameObject.DestroyImmediate(this.gameObject);
        return;
      }

      singleton = this;

      // ReSharper disable once ArrangeStaticMemberQualifier
      GameObject.DontDestroyOnLoad(this.gameObject);
    }

    protected void Start()
    {
      if (this.isStartCoroutine)
      {
        this.timeStarted = Time.realtimeSinceStartup;
        this.StartCoroutine(this.BringToTop(WindowFunctions.GetActiveWindow()));
      }
    }

    private IEnumerator BringToTop(IntPtr handleWindow)
    {
      while ((Time.realtimeSinceStartup - this.timeStarted) < this.maximumTime)
      {
        WindowOnTopMostHelper.BringWindowToTop(handleWindow);

        yield return new WaitForSeconds(this.waitTime);
      }
    }
  }
}