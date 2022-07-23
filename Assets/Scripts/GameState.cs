namespace catwars {

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using React;
using Util;

public enum Phase { PreGame = 0, Hunt, Forage, Eat, Social }

public enum Place {
  Lake,  River,  Marsh,      Field,    // Riverclan
  Shore, Forest, TwoLegNest, Stream,   // Thunderclan
  Swamp, Woods,  Clearing,   Brambles, // Shadowclan
  Cliff, Moor,   HorsePlace, Wetlands, // Windclan
}

public enum Prey {
  Fish,
  Finch,
  Starling,
  Wren,
  Rabbit,
  Squirrel,
  Mouse,
  Rat,
  Shrew,
  Vole,
}

public enum Herb {
  Cobweb,
  Burdock,
  Juniper,
  Goldenrod,
  Yarrow,
  Catmint,
}

public static class PlaceUtil {

  public static IList<Prey> Preys (this Place place) {
    switch (place) {
    case  Place.Lake: return new [] { Prey.Fish };
    case Place.River: return new [] { Prey.Fish, Prey.Wren };
    case Place.Marsh: return new [] { Prey.Finch, Prey.Vole };
    case Place.Field: return new [] { Prey.Starling, Prey.Mouse };

    case      Place.Shore: return new [] { Prey.Wren, Prey.Shrew };
    case     Place.Forest: return new [] { Prey.Starling, Prey.Squirrel };
    case Place.TwoLegNest: return new [] { Prey.Wren, Prey.Mouse };
    case     Place.Stream: return new [] { Prey.Starling, Prey.Vole };

    case    Place.Swamp: return new [] { Prey.Wren, Prey.Rat };
    case    Place.Woods: return new [] { Prey.Finch, Prey.Squirrel };
    case Place.Clearing: return new [] { Prey.Starling, Prey.Vole };
    case Place.Brambles: return new [] { Prey.Finch, Prey.Shrew };

    case      Place.Cliff: return new [] { Prey.Starling, Prey.Shrew };
    case       Place.Moor: return new [] { Prey.Wren, Prey.Rabbit };
    case Place.HorsePlace: return new [] { Prey.Finch, Prey.Mouse };
    case   Place.Wetlands: return new [] { Prey.Wren, Prey.Vole };

    default:
      throw new Exception("Unknown place " + place);
    }
  }
}

public class GameState {

  public readonly IMutable<int> day = Values.Mutable(0);
  public readonly IMutable<Phase> phase = Values.Mutable(Phase.PreGame);
  public readonly MutableSet<ClanState> phaseDone = RSets.LocalMutable<ClanState>();

  public readonly ClanState shadow;
  public readonly ClanState thunder;
  public readonly ClanState river;
  public readonly ClanState wind;

  public readonly Emitter<string> messages = new Emitter<string>();

  public readonly System.Random random = new System.Random();

  public GameState () {
    shadow  = new ClanState(this, "Shadow Clan");
    thunder = new ClanState(this, "Thunder Clan");
    river   = new ClanState(this, "River Clan");
    wind    = new ClanState(this, "Wind Clan");

    phaseDone.OnValue(cs => {
      if (cs.Count == 4) OnPhaseDone();
    });
  }

  public void Start () => StartNewDay();

  private void StartNewDay () {
    day.UpdateVia(d => d+1);
    messages.Emit($"Day {day.current}");
    phase.Update(Phase.Hunt);
    messages.Emit($"Time to Hunt");
  }

  private void OnPhaseDone () {
    if (phase.current == Phase.Hunt) {
      phase.Update(Phase.Eat);
      messages.Emit($"Time to Eat");
      phaseDone.Clear();
    }
  }
}

public class ClanState {

  public readonly GameState game;
  public readonly string name;
  public readonly List<CatState> cats = new List<CatState>();
  public readonly MutableMap<int, Prey> freshKill = RMaps.LocalMutable<int, Prey>();
  public readonly MutableMap<Herb, int> herbs = RMaps.LocalMutable<Herb, int>();
  public readonly MutableSet<int> acted = RSets.LocalMutable<int>();

  public readonly Emitter<string> messages = new Emitter<string>();

  public ClanState (GameState game, string name) {
    this.game = game;
    this.name = name;
    foreach (var herb in (Herb[])Enum.GetValues(typeof(Herb))) herbs.Add(herb, 0);

    // TEMP: random kitties
    var faces = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
    game.random.Shuffle(faces);
    var ff = 0;
    AddCat(1, 0, faces[ff++], CatState.Gender.Male);
    AddCat(2, 0, faces[ff++], CatState.Gender.Female);
    AddCat(3, 0, faces[ff++], CatState.Gender.Male);
    AddCat(4, 0, faces[ff++], CatState.Gender.Female);
    AddCat(5, 0, faces[ff++], CatState.Gender.Male);

    acted.CountValue.OnValue(ac => {
      if (ac >= CanAct) Done();
    });
  }

  public void Done () {
    game.phaseDone.Add(this);
  }

  public void AddFreshKill (Prey prey) {
    var index = freshKill.Count;
    freshKill.Add(index, prey);
  }

  private int CanAct => cats.Count(cc => cc.CanAct(game.phase.current));

  private void AddCat (int id, int parentId, int faceId, CatState.Gender gender) {
    var state = new CatState(this, id, parentId, game.day.current, faceId, gender);
    state.acted.OnEmit(acted => {
      if (acted) this.acted.Add(id);
      else this.acted.Remove(id);
    });
    cats.Add(state);
  }
}

public class CatState {

  public enum Gender { None = 0, Female, Male }
  public enum Role { Leader, Medicine, Warrior, Apprentice, Kit }
  public enum Hunger { Starving, Hungry, Full }
  public enum Injury { None = 0, Scratched, Poisoned }
  public enum Stomach { Normal = 0, BellyAche, Poisoned }
  public enum Illness { None = 0, WhiteCough, GreenCough }

  public readonly ClanState clan;
  public readonly int id;
  public readonly int parentId;
  public readonly int birthday;
  public readonly int faceId;
  public readonly Gender gender;

  public readonly IMutable<string> name = Values.Mutable("");
  public readonly IMutable<Role> role = Values.Mutable(Role.Warrior);
  public readonly IMutable<Hunger> hunger = Values.Mutable(Hunger.Full);
  public readonly IMutable<Injury> injury = Values.Mutable(Injury.None);
  public readonly IMutable<Stomach> stomach = Values.Mutable(Stomach.Normal);
  public readonly IMutable<Illness> illness = Values.Mutable(Illness.None);

  public readonly IMutable<bool> acted = Values.Mutable(false);
  public readonly IMutable<int> pregnant = Values.Mutable(0);

  public CatState (ClanState clan, int id, int parentId, int birthday, int faceId, Gender gender) {
    this.clan = clan;
    this.id = id;
    this.parentId = parentId;
    this.birthday = birthday;
    this.faceId = faceId;
    this.gender = gender;

    // TODO
    this.name.Update($"Cat {id}");
    this.role.Update(id == 1 ? Role.Leader : id == 2 ? Role.Medicine : Role.Warrior);
  }

  public bool CanAct (Phase phase) {
    if (phase == Phase.Hunt) return this.role.current != Role.Kit && this.pregnant.current == 0;
    else return true; // TODO
  }

  public void OnDrop (Place place) {
    Debug.Log("OnDrop " + this + " => " + place);
    if (acted.current) return; // cat already acted this turn

    // if cat is pregnant, they can't hunt
    if (pregnant.current > 0) {
      clan.messages.Emit($"{name.current} can't hunt. They are pregnant.");
      return;
    }

    var hunger = (int)this.hunger.current;
    switch (this.role.current) {
    case Role.Kit:
      clan.messages.Emit($"{name.current} can't hunt. They are just a Kit.");
      break;

    case Role.Medicine:
      List<Herb> found = null;
      for (var rr = 0; rr <= hunger; rr += 1) {
        if (clan.game.random.Next(4) == 0) {
          var herb = clan.game.random.Pick((Herb[])Enum.GetValues(typeof(Herb)));
          found ??= new List<Herb>();
          found.Add(herb);
          clan.herbs.Update(herb, h => h+1);
        }
      }
      clan.messages.Emit($"{name.current} found {FormatFound(found)}.");
      acted.Update(true);
      break;

    default:
      var preys = place.Preys();
      List<Prey> caught = null;
      var rolls = 4-hunger;
      if (this.role.current == Role.Apprentice) rolls -= 1;
      if (this.injury.current != Injury.None) rolls -= 1;
      if (this.stomach.current != Stomach.Normal) rolls -= 1;
      if (this.illness.current != Illness.None) rolls -= 1;
      // TODO: add bonuses
      for (var rr = 0; rr < rolls; rr += 1) {
        if (clan.game.random.Next(4) == 0) {
          var prey = clan.game.random.Pick(preys);
          caught ??= new List<Prey>();
          caught.Add(prey);
          clan.AddFreshKill(prey);
        }
      }
      clan.messages.Emit($"{name.current} caught {FormatCatch(caught)}.");
      acted.Update(true);
      break;
    }
  }

  private static string FormatCatch (List<Prey> caught) {
    if (caught == null) return "no prey";
    else if (caught.Count == 1) return $"a {caught[0]}";
    else if (caught.Count == 2) return $"a {caught[0]} and a {caught[1]}";
    else {
      var most = String.Join(", ", caught.Take(caught.Count-1));
      var last = caught[caught.Count-1];
      return $"a {most} and {last}";
    }
  }

  private static string FormatFound (List<Herb> found) {
    if (found == null) return "nothing";
    else if (found.Count == 1) return $"{found[0]}";
    else if (found.Count == 2) return $"{found[0]} and {found[1]}";
    else {
      var most = String.Join(", ", found.Take(found.Count-1));
      var last = found[found.Count-1];
      return $"{most} and {last}";
    }
  }

  public override string ToString () => $"{id}/{gender}/{name.current}";
}
}
