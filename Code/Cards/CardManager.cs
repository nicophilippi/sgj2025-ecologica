using System.Linq;
using Godot;

public partial class CardManager : Node2D
{
    [Export] private Node2D[] _cardPivots;
    [Export] private PackedScene[] _startCards;
    [Export] private PackedScene[] _allCards;
    [Export] public float SelectDistance = 150f;


    private Card[] _cards;
    private Card _selected;
    private int _tier;


    public override void _EnterTree()
    {
        base._EnterTree();

        _cards = _startCards.Select(s => s.Instantiate<Card>()).ToArray();
        if (_cards.Length != _cardPivots.Length)
            GD.PrintErr($"_cards.Length ({_cards.Length}) != _cardPivots.Length ({_cardPivots.Length})");
        foreach (var c in _cards) AddChild(c);
    }


    public override void _Process(double delta)
    {
        base._Process(delta);

        foreach (var (c, i) in _cards.WithIndex())
        {
            c.GlobalPosition = _cardPivots[i].GlobalPosition;
            c.GlobalRotation = _cardPivots[i].GlobalRotation;
            c.GlobalScale = new Vector2(0.5f, 0.5f);
        }

        // Mouse Above Calculation
        var mousePos = GetGlobalMousePosition();
        var minI = _cards.WithIndex()
            .Where(c => c.el != null)
            .MinBy(int.MaxValue, (null, -1),
                t => (t.el.GlobalPosition - mousePos).LengthSquared(), out var lenToMinSqr).i;
        if (lenToMinSqr > SelectDistance * SelectDistance) minI = -1;

        // Mouse Above Upscale
        if (minI != -1) _cards[minI].GlobalScale = new Vector2(0.75f, 0.75f);

        #region _selectedUpdates

        if (minI != -1 && Input.IsMouseButtonPressed(MouseButton.Left)) _selected = _cards[minI];
        if (minI == -1 && Input.IsMouseButtonPressed(MouseButton.Left)) _selected = null;
        if (Input.IsMouseButtonPressed(MouseButton.Right)) _selected = null;

        #endregion
        
        // _selected Upscale
        if (_selected != null) _selected.GlobalScale = Vector2.One;
    }
}