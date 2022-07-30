namespace catwars {

using UnityEngine;

[CreateAssetMenu(menuName = "Catwars/Icons", fileName = "Icons")]
public class Icons : ScriptableObject {

  [EnumArray(typeof(Prey))] public Sprite[] preyIcons;
  [EnumArray(typeof(Herb))] public Sprite[] herbIcons;

  public Sprite[] faces;

  public Sprite Prey (Prey prey) => preyIcons[(int)prey];
  public Sprite Herb (Herb herb) => herbIcons[(int)herb];

  public override string ToString () => name;
}
}
