namespace catwars {

using System.Collections.Generic;

using UnityEngine;
using TMPro;

using React;
using Util;

public class ClanController : MonoBehaviour {
  private GameState game;
  private ClanState clan;
  private List<string> messages = new List<string>();

  public TMP_Text clanLabel;
  public GameObject cats;
  public GameObject catPrefab;
  public HerbController[] herbs;
  public FoodController food;

  public GameObject feedbackPanel;
  public TMP_Text feedbackLabel;

  public readonly IMutable<CatState> draggedCat = Values.Mutable<CatState>(null);
  public readonly IMutable<PlaceController> hoveredPlace = Values.Mutable<PlaceController>(null);
  public readonly IValue<(CatState, PlaceController)> dragInfo;

  private ClanController () {
    dragInfo = Values.Join(draggedCat, hoveredPlace);
  }

  public void Init (GameState game, ClanState clan) {
    this.game = game;
    this.clan = clan;
    clanLabel.text = clan.name;
    foreach (var cat in clan.cats) AddCat(cat);
    var idx = 0; foreach (var herbctl in herbs) {
      var herb = (Herb)idx++;
      herbctl.SetHerb(herb);
      clan.herbs.GetValue(herb).OnValue(herbctl.SetCount);
    }
    clan.freshKill.OnValue(food.Show);

    dragInfo.OnChange((info, oinfo) => {
      var (cat, place) = info;
      var (ocat, oplace) = oinfo;
      // if we transition from cat to no cat while hovering over a place, we dropped a cat there
      if (oplace != null && place == oplace &&
          cat == null && ocat != null) OnCatDropped(oinfo.Item1, info.Item2);
    });

    clan.messages.OnEmit(ShowFeedback);
  }

  public void ShowFeedback (string message) {
    if (feedbackPanel.activeSelf) {
      messages.Add(message);
      return;
    }
    feedbackLabel.text = message;
    feedbackPanel.SetActive(true);
    this.RunAfter(2, () => {
      feedbackPanel.SetActive(false);
      if (messages.Count > 0) {
        var message = messages[0];
        messages.RemoveAt(0);
        ShowFeedback(message);
      }
    });
  }

  private void AddCat (CatState cat) {
    var catObj = Instantiate(catPrefab, cats.transform);
    catObj.GetComponent<CatController>().Init(cat);
    catObj.SetActive(true);
  }

  private void OnCatDropped (CatState cat, PlaceController place) {
    cat.OnDrop(place.place);
  }
}

}
