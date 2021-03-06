using System;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MessageBox : MarshalByRefObject {
    const int lineWidth = 30;

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

    TextLabel text;

    ContentManager contentManager;

    public MessageBox(ContentManager contentManager, SpriteRenderer spriteRenderer) {
        this.contentManager = contentManager;
        this.spriteRenderer = spriteRenderer;

        registerSprites();
    }

    public void registerSprites() {
        buildMessageBackground();

        text = TextLabel.create(spriteRenderer, contentManager, "sierra_agi_font_black");
        text.alignCenter = true;
        text.worldPosition = new Vector3(0.5f, 0.5f, -9.5f);
        text.hide();

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
        leftBorder.setDepth(-9f);
        rightBorder.setDepth(-9f);
        topBorder.setDepth(-9f);
        bottomBorder.setDepth(-9f);

        hide();
    }

    void buildMessageBackground() {
        messageBackground = spriteRenderer.add(new Sprite(), "1px");

        messageBackground.visible(false);
        messageBackground.setDepth(-8.75f);
    }

    public void setMessage(String message) {
        text.setText(wrap(message));
        Debug.Log(message); // print it since we can't draw it yet

        var textRect = new Rectangle((int) text.screenPosition.X - text.width / 2,
                                     (int) text.screenPosition.Y - text.height / 2,
                                     text.width,
                                     text.height);

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

        text.show();
    }

    public void hide() {
//        textLabel.active = false;
        messageBackground.visible(false);
        leftBorder.visible(false);
        rightBorder.visible(false);
        topBorder.visible(false);
        bottomBorder.visible(false);

        text.hide();
    }

}
