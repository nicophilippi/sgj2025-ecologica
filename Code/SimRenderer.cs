using Godot;
using NewGameProject.sim;

public partial class SimRenderer : Node2D
{
    private struct RenderedCell
    {
        public Sprite2D Sprite;
    }


    [Export] private Texture2D _tex;
    [Export] private Vector2 _cellSize = new(128f, 128f);
    private RenderedCell[,] _sprites;


    public override void _EnterTree()
    {
        base._EnterTree();

        _sprites = new RenderedCell[Simulation.WorldSize, Simulation.WorldSize];

        for (var x = 0; x < Simulation.WorldSize; x++)
        for (var y = 0; y < Simulation.WorldSize; y++)
        {
            _sprites[x, y] = InitCell(new Vector2(x, y) * _cellSize);
        }
    }


    public override void _Process(double delta)
    {
        base._Process(delta);

        for (var x = 0; x < _sprites.GetLength(0); x++)
        for (var y = 0; y < _sprites.GetLength(1); y++)
        {
            var sprite = _sprites[x, y];
            var data = LoseReferences.GetTile(x, y);
            DrawCell(sprite, data);
        }
    }


    private RenderedCell InitCell(Vector2 at)
    {
        var o = new RenderedCell();

        o.Sprite = new Sprite2D();
        AddChild(o.Sprite); // IMPORTANT!
        o.Sprite.Position = at;
        o.Sprite.Texture = _tex;

        return o;
    }


    private void DrawCell(RenderedCell rendCell, LoseReferences.WhateverTheFuckTileDataLooksLike tileData)
    {
        rendCell.Sprite.Modulate = Colors.Green;
    }
}