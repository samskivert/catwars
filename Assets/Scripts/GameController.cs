namespace catwars {

using System;
using UnityEngine;
using Util;

public class GameController : MonoBehaviour {

  private class UnityEnviron : Environ {
    private MonoBehaviour owner;
    public UnityEnviron (MonoBehaviour owner) {
      this.owner = owner;
    }
    public override void RunLater (Action action) => owner.RunIn(1, action);
    public override void RunAfter (int seconds, Action action) => owner.RunAfter(seconds, action);
  }

  public readonly GameState state;

  public MessagesController messages;
  public ClanController[] clans;

  public GameController () {
    state = new GameState(new UnityEnviron(this));
  }

  private void Start () {
    state.messages.OnEmit(messages.Show);
    clans[0].Init(state, state.shadow);
    clans[1].Init(state, state.thunder);
    clans[2].Init(state, state.river);
    clans[3].Init(state, state.wind);
    state.Start();
  }
}

}
