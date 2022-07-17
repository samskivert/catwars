namespace catwars {

using System;
using System.Collections.Generic;
using React;
using Util;

public enum Phase { PreGame = 0, Hunt, Forage, Eat, Social }

public enum Place {
  Lake,  River,  Marsh,      Field,    // Riverclan
  Shore, Forest, TwoLegNest, Stream,   // Thunderclan
  Swamp, Woods,  Clearing,   Brambles, // Shadowclan
  Cliff, Moor,   HorsePlace, Wetlands, // Windclan
}

public enum Food {
  Fish,

  Finch,
  Sparrow,
  Starling,
  Thrush,
  Wren,

  Rabbit,
  Squirrel,

  Mouse,
  Rat,
  Shrew,
  Vole,
}

public static class PlaceUtil {

  public static IList<Food> FoodsFor (this Place place) {
    switch (place) {
    case  Place.Lake: return new [] { Food.Fish };
    case Place.River: return new [] { Food.Fish, Food.Wren };
    case Place.Marsh: return new [] { Food.Thrush, Food.Vole };
    case Place.Field: return new [] { Food.Starling, Food.Mouse };

    case      Place.Shore: return new [] { Food.Wren, Food.Shrew };
    case     Place.Forest: return new [] { Food.Sparrow, Food.Squirrel };
    case Place.TwoLegNest: return new [] { Food.Thrush, Food.Mouse };
    case     Place.Stream: return new [] { Food.Finch, Food.Vole };

    case    Place.Swamp: return new [] { Food.Wren, Food.Rat };
    case    Place.Woods: return new [] { Food.Sparrow, Food.Squirrel };
    case Place.Clearing: return new [] { Food.Starling, Food.Vole };
    case Place.Brambles: return new [] { Food.Finch, Food.Shrew };

    case      Place.Cliff: return new [] { Food.Starling, Food.Shrew };
    case       Place.Moor: return new [] { Food.Wren, Food.Rabbit };
    case Place.HorsePlace: return new [] { Food.Sparrow, Food.Mouse };
    case   Place.Wetlands: return new [] { Food.Thrush, Food.Vole };

    default:
      throw new Exception("Unknown place " + place);
    }
  }
}

public class GameState {

  public readonly IValue<int> day = Values.Mutable(0);
  public readonly IValue<Phase> phase = Values.Mutable(Phase.PreGame);

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
    // TEMP: random kitties
    var random = new System.Random();
    var faces = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
    random.Shuffle(faces);
    var ff = 0;
    cats.Add(new CatState(1, 0, 0, faces[ff++], CatState.Gender.Male));
    cats.Add(new CatState(2, 0, 0, faces[ff++], CatState.Gender.Female));
    cats.Add(new CatState(3, 0, 0, faces[ff++], CatState.Gender.Male));
    cats.Add(new CatState(4, 0, 0, faces[ff++], CatState.Gender.Female));
    cats.Add(new CatState(5, 0, 0, faces[ff++], CatState.Gender.Male));
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

  public override string ToString () => $"{id}/{gender}/{name.current}";
}
}
