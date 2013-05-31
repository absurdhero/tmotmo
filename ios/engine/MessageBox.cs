using System;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

public class MessageBox : MarshalByRefObject {
    const int lineWidth = 20;

//    GameObject textLabel;
//    GUIText text;
    Sprite messageBackground;
    Sprite leftBorder;
    Sprite rightBorder;
    Sprite topBorder;
    Sprite bottomBorder;

    const float borderThickness = 8f;
    const float halfThickness = borderThickness / 2f;
    const float paddingX = 4f, paddingY = 4f;

    SpriteRenderer spriteRenderer;

    public MessageBox(SpriteRenderer spriteRenderer) {
        this.spriteRenderer = spriteRenderer; //, Font font) {
        buildMessageBackground();

//        textLabel = new GameObject("message text");
//        textLabel.active = false;
//        text = textLabel.AddComponent<GUIText>();
//        textLabel.transform.position = new Vector3(0.5f, 0.5f, -9.5f);
//        text.alignment = TextAlignment.Center;
//        text.anchor = TextAnchor.MiddleCenter;
//        text.font = font;
//        text.material.SetColor("_Color", Color.black);

        leftBorder = spriteRenderer.add(new Sprite(), "messageBorder");
        rightBorder = spriteRenderer.add(new Sprite(), "messageBorder");
        topBorder = spriteRenderer.add(new Sprite(), "messageBorder");
        bottomBorder = spriteRenderer.add(new Sprite(), "messageBorder");

        // these are positive because they are relative to the camera's placement
        // the world z coordinate will be negative
        leftBorder.setDepth(-9.5f);
        rightBorder.setDepth(-9.5f);
        topBorder.setDepth(-9.5f);
        bottomBorder.setDepth(-9.5f);

        hide();
    }

    void buildMessageBackground() {
        messageBackground = spriteRenderer.add(new Sprite(), "1px");

        messageBackground.visible(false);
        messageBackground.setDepth(-9f);
    }

    public void setMessage(String message) {
//        text.text = wrap(message);
        Debug.Log(message); // print it since we can't draw it yet
//
//        var textRect = text.GetScreenRect(Camera.main);
        // placeholder so it shows something until we have fonts
        var textRect = new Rectangle(Camera.main.pixelWidth / 2 - 100, Camera.main.pixelHeight / 2 - 40, 200, 80);

        expandBackgroundToSizeOf(textRect);

        leftBorder.setScreenPosition(textRect.Left - halfThickness - paddingX, textRect.Top - halfThickness - paddingY);
        rightBorder.setScreenPosition(textRect.Right + halfThickness + paddingX, textRect.Top - halfThickness - paddingY);

        var verticalBorderScale = new Vector3(1f, (textRect.Height + borderThickness + paddingY * 2 + 2f) / leftBorder.height, 1f);
        leftBorder.transform.localScale =  verticalBorderScale;
        rightBorder.transform.localScale = verticalBorderScale;

        topBorder.setScreenPosition(textRect.Left - halfThickness - paddingX, textRect.Top - halfThickness - paddingY);
        bottomBorder.setScreenPosition(textRect.Left - halfThickness - paddingX, textRect.Bottom + halfThickness + paddingY);

        var horizontalBorderScale = new Vector3((textRect.Width + borderThickness + paddingX * 2 + 2f) / leftBorder.width, 1f, 1f);
        topBorder.transform.localScale =  horizontalBorderScale;
        bottomBorder.transform.localScale = horizontalBorderScale;
    }

    void expandBackgroundToSizeOf(Rectangle textRect) {
        messageBackground.transform.localScale = new Vector3((textRect.Width + borderThickness * 2 + paddingX * 2 + 2f), (textRect.Height + borderThickness * 2 + paddingY * 2 + 2f), 1f);
        messageBackground.setScreenPosition(textRect.X - borderThickness - paddingX, textRect.Y - borderThickness - paddingY);
    }

    static public String wrap(String text) {
        StringBuilder output = new StringBuilder(text.Length + 1);
        int spaceLeft = lineWidth;
        foreach(String word in text.Split(' ')) {
            if (word.Length + 1 > spaceLeft) {
                output.Append('\n');
                output.Append(word);
                spaceLeft = lineWidth - word.Length;
            } else {
                spaceLeft -= word.Length + 1;
                output.Append(' ');
                output.Append(word);
            }
        }
        output.Remove(0, 1); // remove space that was inserted at the beginning of the text
        return output.ToString();
    }

    public void show() {
//        textLabel.active = true;
        messageBackground.visible(true);
        leftBorder.visible(true);
        rightBorder.visible(true);
        topBorder.visible(true);
        bottomBorder.visible(true);
    }

    public void hide() {
//        textLabel.active = false;
        messageBackground.visible(false);
        leftBorder.visible(false);
        rightBorder.visible(false);
        topBorder.visible(false);
        bottomBorder.visible(false);
    }

}
