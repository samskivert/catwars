namespace catwars {

using UnityEngine;

[CreateAssetMenu(menuName = "Catwars/Icons", fileName = "Icons")]
public class Icons : ScriptableObject {

  [EnumArray(typeof(Prey))] public Sprite[] preyIcons;
  public Sprite Prey (Prey prey) => preyIcons[(int)prey];

  [EnumArray(typeof(Herb))] public Sprite[] herbIcons;
  public Sprite Herb (Herb herb) => herbIcons[(int)herb];

  public Sprite[] faces;
  public Sprite crown;

  public override string ToString () => name;
}
}
