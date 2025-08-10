using System;
using System.Linq;
using Godot;

public partial class CardManager : Node2D
{
    [Export] private Node2D[] _cardPivots;
    [Export] private Node2D[] _selectedCardPivots;
    [Export] private Node2D _newCardsAt;
    [Export] private PackedScene[] _startCards;
    [Export] private PackedScene[] _allCards;
    [Export] public float SelectDistance = 150f;
    [Export] public float PositionFix = 1f;


    private Card[] _cards;
    private Vector2[] _targetPositions;
    private Card _selected;
    private int _selectedIndex;
    private int _tier;


    public override void _EnterTree()
    {
        base._EnterTree();

        _cards = _startCards.Select(s => s.Instantiate<Card>()).ToArray();
        if (_cards.Length != _cardPivots.Length)
            GD.PrintErr($"_cards.Length ({_cards.Length}) != _cardPivots.Length ({_cardPivots.Length})");
        foreach (var c in _cards)
        {
            AddChild(c);
            c.GlobalPosition = _newCardsAt.GlobalPosition;
        }

        _targetPositions = new Vector2[_cards.Length];
    }


    public override void _Process(double delta)
    {
        base._Process(delta);

        foreach (var (c, i) in _cards.WithIndex())
        {
            c.GlobalRotation = _cardPivots[i].GlobalRotation;
            _targetPositions[i] = _cardPivots[i].GlobalPosition;
        }

        // Mouse Above Calculation
        var mousePos = GetGlobalMousePosition();
        var minI = _cards.WithIndex()
            .Where(c => c.el != null)
            .MinBy(int.MaxValue, (null, -1),
                t => (t.el.GlobalPosition - mousePos).LengthSquared(), out var lenToMinSqr).i;
        if (lenToMinSqr > SelectDistance * SelectDistance) minI = -1;

        // Mouse Above Upscale
        if (minI != -1) _targetPositions[minI] = (_cardPivots[minI].GlobalPosition + _selectedCardPivots[minI].GlobalPosition) / 2f;

        #region _selectedUpdates

        if (minI != -1 && Input.IsActionJustPressed("select card"))
        {
            _selected = _cards[minI];
            _selectedIndex = minI;
        }

        if (_selected != null && minI == -1 && Input.IsActionJustPressed("select card"))
        {
            _selected.Play();
            _selected.QueueFree();
            GD.Print("Selected: " + _selected.Name);
            _selected = null;
            GD.Print("Cards[SelectedIndex]: " + _cards[_selectedIndex].Name);
            GD.Print("Selected Type: " + _cards[_selectedIndex].GetType().Name);
            _cards[_selectedIndex] = NewCard();
        }
        if (Input.IsMouseButtonPressed(MouseButton.Right)) _selected = null;

        #endregion

        foreach (var (c, i) in _cards.WithIndex()
                     .Where(t => t.el != _selected))
        {
            c.GlobalPosition = c.GlobalPosition.MoveToward(_targetPositions[i], PositionFix * (float) delta);
        }
        
        // _selected Upscale
        if (_selected != null) _selected.GlobalPosition = _selected.GlobalPosition.MoveToward(
                _selectedCardPivots[_selectedIndex].GlobalPosition,
                PositionFix * (float) delta);
    }


    private Card NewCard()
    {
        if (_allCards.Length == 0) GD.PrintErr("NO CARDS!?\n" + Utils.NoBitches);
        var result = _allCards[GD.RandRange(0, _allCards.Length - 1)].Instantiate<Card>();
        AddChild(result);
        result.GlobalPosition = _newCardsAt.GlobalPosition;
        return result;
    }
}