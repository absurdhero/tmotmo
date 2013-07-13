using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Prompt : MarshalByRefObject {
    SpriteRenderer spriteRenderer;
    ContentManager contentManager;

    TextLabel text;
    Sprite blackBox;

    public Prompt(SpriteRenderer spriteRenderer, ContentManager contentManager) {
        this.contentManager = contentManager;
        this.spriteRenderer = spriteRenderer;
        registerSprites();
    }

    void buildBlackBox(SpriteRenderer spriteRenderer) {
        var blackTexture = new Texture2D(spriteRenderer.graphics.GraphicsDevice, 1, 1);
        blackTexture.SetData(new byte[] { 0, 0, 0, 255 });

        blackBox = spriteRenderer.add(new Sprite(), blackTexture, new Point(1, 1));
        blackBox.setCenterToViewportCoord(0f, 0.25f);
        blackBox.setDepth(-8.75f);
        blackBox.transform.localScale = new Vector3(Camera.main.pixelWidth, 80, 1f);
        blackBox.isVisible = false;
    }

    public void Destroy() {
        spriteRenderer.remove(text);
        spriteRenderer.remove(blackBox);
    }

    public void registerSprites() {
        text = TextLabel.create(spriteRenderer, contentManager, "sierra_agi_font_white");
        text.isVisible = false;
        text.transform.localTranslation = new Vector3(0f, 100f, 0f);
        text.setDepth(-9.5f);

        buildBlackBox(spriteRenderer);
    }

    public void setText(string action) {
        text.setText(">" + action + "_");
    }

    public void show() {
        blackBox.isVisible = true;
        text.show();
    }

    public void hide() {
        blackBox.isVisible = false;
        text.hide();
    }
}

