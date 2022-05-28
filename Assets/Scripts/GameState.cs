namespace catwars {

using System.Collections.Generic;
using React;

public class GameState {

  public readonly IValue<int> day = Values.Mutable(0);

  public ClanState shadow  = new ClanState("Shadow Clan");
  public ClanState thunder = new ClanState("Thunder Clan");
  public ClanState river   = new ClanState("River Clan");
  public ClanState wind    = new ClanState("Wind Clan");
}

public class ClanState {

  public readonly string name;
  public readonly List<CatState> cats = new List<CatState>();
  public readonly IValue<int> freshKill = Values.Mutable(0);
  public readonly MutableMap<int, int> herbs = RMaps.LocalMutable<int, int>();

  public ClanState (string name) {
    this.name = name;
    for (var ii = 0; ii < 6; ii += 1) herbs.Add(ii, 0);
  }
}

public class CatState {

  public enum Gender { None = 0, Female, Male }
  public enum Role { Leader, Medicine, Warrior, Apprentice, Kit }
  public enum Hunger { Starving, Hungry, Full }
  public enum Injury { None = 0, Scratched, Poisoned }
  public enum Stomach { Normal = 0, BellyAche, Poisoned }
  public enum Illness { None = 0, WhiteCough, GreenCough }

  public readonly int id;
  public readonly int parentId;
  public readonly int birthday;
  public readonly int faceId;
  public readonly Gender gender;

  public readonly IValue<string> name = Values.Mutable("");
  public readonly IValue<Role> role = Values.Mutable(Role.Warrior);
  public readonly IValue<Hunger> hunger = Values.Mutable(Hunger.Full);
  public readonly IValue<Injury> injury = Values.Mutable(Injury.None);
  public readonly IValue<Stomach> stomach = Values.Mutable(Stomach.Normal);
  public readonly IValue<Illness> illness = Values.Mutable(Illness.None);

  public readonly IValue<int> pregnant = Values.Mutable(0);

  public CatState (int id, int parentId, int birthday, int faceId, Gender gender) {
    this.id = id;
    this.parentId = parentId;
    this.birthday = birthday;
    this.faceId = faceId;
    this.gender = gender;
  }
}
}
