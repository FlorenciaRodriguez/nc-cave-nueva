namespace Assets.Scripts.Exercise.Helpers
{
  using System.IO;
  using System.Text;

  using MediaLab.NetworkEntities.Entities;
  using MediaLab.NetworkEntities.Helpers;
  using MediaLab.NetworkEntities.Simpa.Exercises;

  using Newtonsoft.Json;

  using UnityEngine;

  public static class ExerciseMockHelper
  {
    private static readonly string ExerciseFilePathFormat = Path.Combine(Application.streamingAssetsPath, "Exercise", "Mocks", "Exercise{0:D2}", "Exercise{0:D2}.json");

    public static ExerciseEntity LoadExerciseEntity(int id)
    {
      return JsonHelper.DeserializeJsonEntity(File.ReadAllText(string.Format(ExerciseFilePathFormat, id))) as ExerciseEntity;
    }

    public static string SaveExerciseEntity(ExerciseEntity exerciseEntity)
    {
      var filePath = string.Format(ExerciseFilePathFormat, exerciseEntity.Id);

      var directoryInfo = Directory.GetParent(filePath);
      if (directoryInfo != null && !directoryInfo.Exists)
      {
        Directory.CreateDirectory(directoryInfo.FullName);
      }

      File.WriteAllText(
        filePath,
        SerializeToJsonNetEntity(exerciseEntity),
        Encoding.UTF8);

      return filePath;
    }

    private static string SerializeToJsonNetEntity(JsonEntity jsonEntity)
    {
      return JsonConvert.SerializeObject(
        new JsonNetEntity
        {
          Type = jsonEntity.GetType().FullName,
          JsonEntity = jsonEntity
        },
        Formatting.Indented,
        new JsonSerializerSettings
        {
          NullValueHandling = NullValueHandling.Ignore
        });
    }
  }
}
