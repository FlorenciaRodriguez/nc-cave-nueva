namespace Assets.Scripts.Exercise.Helpers
{
  using System;
  using System.Collections.Generic;

  using Assets.Scripts.Helpers;

  using MediaLab.NetworkEntities.Simpa.Exercises;

  public static class ExerciseExtensionHelper
  {
    private static readonly List<int> LocationTourExerciseIds = new List<int> { 1, 2, 5 };

    public static ApplicationHelper.AppTypeEnum GetAppType(this ExerciseEntity exerciseEntity)
    {
      foreach (var exerciseId in LocationTourExerciseIds)
      {
        if (exerciseEntity.Id == exerciseId)
        {
          return ApplicationHelper.AppTypeEnum.LocationTour;
        }
      }

      throw new Exception("The application type could not be recognized");
    }
  }
}
