namespace catwars {

using UnityEngine;

public class GameController : MonoBehaviour {

  public readonly GameState state = new GameState();

  public ClanController[] clans;

  private void Start () {
    clans[0].Init(state, state.shadow);
    clans[1].Init(state, state.thunder);
    clans[2].Init(state, state.river);
    clans[3].Init(state, state.wind);
    state.Start();
  }
}

}
