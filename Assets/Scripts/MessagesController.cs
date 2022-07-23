namespace catwars {

using System.Collections.Generic;

using UnityEngine;
using TMPro;

using Util;

public class MessagesController : MonoBehaviour {
  private List<string> messages = new List<string>();

  public GameObject panel;
  public TMP_Text label;

  public void Show (string message) {
    if (panel.activeSelf) {
      messages.Add(message);
      return;
    }
    label.text = message;
    panel.SetActive(true);
    this.RunAfter(2, () => {
      panel.SetActive(false);
      if (messages.Count > 0) {
        var message = messages[0];
        messages.RemoveAt(0);
        Show(message);
      }
    });
  }
}
}
