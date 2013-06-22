using Microsoft.Xna.Framework;

public class Grid {
	public float cellHeight { get; private set; }
	public float cellWidth { get; private set; }
    public float horizontalOffset { get; set; }
    public float verticalOffset { get; set; }
	
	public Grid(float cellWidth, float cellHeight) {
		this.cellWidth = cellWidth;
		this.cellHeight = cellHeight;
	}
	
	public Vector2 PixelAtCell(int i, int j) {
        return new Vector2(horizontalOffset + cellWidth * i,
                           verticalOffset + cellHeight * j);
	}

}
