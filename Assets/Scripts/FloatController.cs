namespace catwars {

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Util;

public class FloatController : MonoBehaviour {
  private RectTransform canvasRt;

  public Icons icons;
  // public Sprite hpIcon;
  // public Sprite gemIcon;

  public GameController game;
  public Canvas canvas;
  public GameObject floatPrefab;

  public void Float (GameObject over, Prey prey, int count) => Float(over, icons.Prey(prey), count);
  public void Float (GameObject over, Herb herb, int count) => Float(over, icons.Herb(herb), count);

  public void Float (GameObject over, Sprite sprite, int count) {
    FloatOver(sprite, null, count < 0 ? count.ToString() : $"+{count}", over);
  }

  // public void FloatHp (GameObject over, int count) {
  //   // Debug.Log("Float HP: " + count);
  //   FloatOver(hpIcon, null, count < 0 ? count.ToString() : $"+{count}", over);
  // }

  // public void Fling (
  //   GameObject fromObj, GameObject toObj, Die.Type type, int count, Action onArrive = null
  // ) => Fling(fromObj, toObj, icons.Die(type), null, count, onArrive);

  // public void Fling (
  //   GameObject fromObj, GameObject toObj, FaceData face, Action onArrive = null
  // ) => Fling(fromObj, toObj, face.image, icons.Die(face.dieType), face.amount, onArrive);

  // public void Fling (
  //   GameObject fromObj, GameObject toObj, Sprite sprite, Sprite back, int count,
  //   Action onArrive = null
  // ) => Fling(sprite, back, count.ToString(), fromObj, toObj, onArrive);

  // public void Fling (
  //   GameObject fromObj, GameObject toObj, Sprite sprite, Sprite back, Action onArrive = null
  // ) => Fling(sprite, back, null, fromObj, toObj, onArrive);

  private void FloatOver (Sprite sprite, Sprite back, string text, GameObject over) {
    var start = canvasRt.InverseTransformPoint(over.transform.position);
    var finish = start;
    finish.y += 50;
    Tween(sprite, back, text, start, finish, Interps.QuadOut, 50, null);
  }

  private void Fling (
    Sprite sprite, Sprite back, string text, GameObject fromObj, GameObject toObj, Action onArrive
  ) {
    var start = canvasRt.InverseTransformPoint(fromObj.transform.position);
    var finish = canvasRt.InverseTransformPoint(toObj.transform.position);
    Tween(sprite, back, text, start, finish, Interps.QuadIn, 2400, onArrive);
  }

  private void Tween (
    Sprite sprite, Sprite back, string text,
    Vector3 start, Vector3 finish, Interp interp, float velocity, Action onArrive
  ) {
    GameObject obj = null;
    RectTransform rect = null;
    var duration = Vector3.Distance(start, finish) / velocity;
    game.anim.Add(Anim.Serial(
      Anim.Action(() => {
        obj = Instantiate(floatPrefab, canvas.transform);
        rect = obj.GetComponent<RectTransform>();
        var backImage = obj.transform.Find("Back").GetComponent<Image>();
        if (back == null) backImage.enabled = false;
        else backImage.sprite = back;
        obj.transform.Find("Back/Icon").GetComponent<Image>().sprite = sprite;
        if (text == null) obj.transform.Find("Count").gameObject.SetActive(false);
        else obj.transform.Find("Count").GetComponent<TMP_Text>().text = text;
        rect.anchoredPosition = start;
      }),
      Anim.TweenVector3(pos => rect.anchoredPosition = pos, start, finish, duration, interp),
      Anim.Action(() => {
        onArrive?.Invoke();
        Destroy(obj);
      })));
    game.anim.AddBarrier();
  }

  private void Start () {
    canvasRt = canvas.GetComponent<RectTransform>();
  }
}
}
