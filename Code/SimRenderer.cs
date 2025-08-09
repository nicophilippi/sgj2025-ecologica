using Godot;
using Microsoft.VisualBasic;

public partial class SimRenderer : Node
{
    [Export] private Texture2D _tex;
    private Sprite2D[,] _sprites;


    public override void _EnterTree()
    {
        base._EnterTree();

        _sprites = new Sprite2D[LoseReferences.SimSize.X, LoseReferences.SimSize.Y];

        for (var x = 0; x < LoseReferences.SimSize.X; x++)
        for (var y = 0; y < LoseReferences.SimSize.Y; y++)
        {
            var sprite = new Sprite2D();
            AddChild(sprite);
            sprite.Position = new Vector2(x, y) * _tex.GetSize();
            sprite.Texture = _tex;
            _sprites[x, y] = sprite;
        }
    }


    public override void _Process(double delta)
    {
        base._Process(delta);


        var visibleRect = GetViewport().GetVisibleRect();
        var visibleIndices = new Rect2I((Vector2I) (visibleRect.Position / _tex.GetSize()),
            (Vector2I) (visibleRect.Size / _tex.GetSize()) + Vector2I.One);
        
        for (var x = Mathf.Max(visibleIndices.Position.X, 0); x <= Mathf.Min(visibleIndices.End.X, _sprites.GetLength(0) - 1); x++)
        for (var y = Mathf.Max(visibleIndices.Position.Y, 0); y <= Mathf.Min(visibleIndices.End.Y, _sprites.GetLength(1) - 1); y++)
        {
            var sprite = _sprites[x, y];
            var data = LoseReferences.GetTile(x, y);
            DrawCell(sprite, data);
        }
    }


    private void DrawCell(Sprite2D sprite, LoseReferences.WhateverTheFuckTileDataLooksLike tileData)
    {
        sprite.Modulate = Colors.Green;
    }
}