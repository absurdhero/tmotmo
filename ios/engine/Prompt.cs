using System;
using System.Collections.Generic;

public class Prompt : MarshalByRefObject {
//    Sprite textLabel, blackBox;
//    GUIText text;

    public Prompt() { //Sprite textLabel, GUIText text) {
//        this.textLabel = textLabel;
//        this.text = text;
        buildBlackBox();
    }

    public void Draw()
    {
       
    }

    void buildBlackBox() {
//        blackBox = GameObject.CreatePrimitive(PrimitiveType.Plane);
//        blackBox.active = false;
//        blackBox.name = "prompt background";
//        blackBox.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
//        blackBox.transform.Rotate(new Vector3(270f, 0f, 0f));
//        blackBox.transform.position = new Vector3(0, -95, -9);
//        blackBox.transform.localScale = new Vector3(30f, 1f, 1.5f);
    }

    public void Destroy() {
//        Sprite.Destroy(textLabel);
    }

    public void setText(string action) {
//        text.text = ">" + action + "_";
    }

    public void show() {
//        blackBox.active = true;
//        textLabel.active = true;
    }

    public void hide() {
//        blackBox.active = false;
//        textLabel.active = false;
    }
}

