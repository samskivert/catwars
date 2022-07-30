namespace catwars {

using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using React;

public class ClanController : MonoBehaviour {
  private GameController gctrl;

  public Icons icons;
  public MessagesController messages;
  public TMP_Text clanLabel;
  public GameObject cats;
  public GameObject catPrefab;
  public PlaceController[] places;
  public HerbController[] herbs;
  public GameObject freshKill;
  public GameObject preyPrefab;
  public Button doneButton;

  public GameState game { get; private set; }
  public ClanState clan { get; private set; }

  public readonly IMutable<object> draggedItem = Values.Mutable<object>(null);
  public readonly IMutable<object> hoveredTarget = Values.Mutable<object>(null);
  public readonly IValue<(object, object)> dragInfo;

  private ClanController () {
    dragInfo = Values.Join(draggedItem, hoveredTarget);
  }

  private void Awake () {
    gctrl = GetComponentInParent<GameController>();
  }

  public void Init (GameState game, ClanState clan) {
    this.game = game;
    this.clan = clan;
    clanLabel.text = clan.name;
    foreach (var cat in clan.cats) AddCat(cat);

    var hidx = 0; foreach (var herbctl in herbs) {
      var herb = (Herb)hidx++;
      herbctl.SetHerb(herb);
      clan.herbs.GetValue(herb).OnValue(herbctl.SetCount);
    }

    clan.freshKill.OnEntries((idx, prey, oprey) => {
      var tx = freshKill.transform;
      if (idx < tx.childCount) tx.GetChild(idx).GetComponent<PreyController>().SetPrey(prey);
      else Instantiate(preyPrefab, tx).GetComponent<PreyController>().SetPrey(prey);
    });
    clan.freshKill.OnRemove((idx, oprey) => {
      Destroy(freshKill.transform.GetChild(idx).gameObject);
    });

    clan.messages.OnEmit(messages.Show);
    clan.caught.OnEmit(caught => FloatIcon(caught.Item1, icons.Prey(caught.Item2)));
    clan.found.OnEmit(caught => FloatIcon(caught.Item1, icons.Herb(caught.Item2)));

    dragInfo.OnChange((info, oinfo) => {
      var (item, tgt) = info;
      var (oitem, otgt) = oinfo;
      // if we transition from item to no item while hovering over the same target, it's a drop
      if (tgt == otgt && item == null && oitem != null) HandleDrop(oitem, tgt);
    });

    game.phaseDone.ContainsValue(clan).OnValue(done => {
      doneButton.interactable = !done;
    });
    doneButton.onClick.AddListener(() => clan.Done());
  }

  private void FloatIcon (Place place, Sprite icon) {
    var pctrl = places.Where(pp => pp.place == place).FirstOrDefault();
    gctrl.floater.Float(pctrl.gameObject, icon, 1);
  }

  private void HandleDrop (object item, object tgt) {
    // if we transition from cat to no cat while hovering over a place, we dropped a cat there
    if (tgt is Place place && item is CatState cati) cati.OnDrop(place);
    else if (tgt is CatState tcat && item is Prey prey) tcat.OnFed(prey);
    else Debug.Log($"Invalid drop [tgt={tgt}, item={item}]");
  }

  private void AddCat (CatState cat) {
    var catObj = Instantiate(catPrefab, cats.transform);
    catObj.GetComponent<CatController>().Init(cat);
    catObj.SetActive(true);
  }
}

}
