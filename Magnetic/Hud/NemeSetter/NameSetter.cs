using Godot;
using System;

public class NameSetter : Control
{
    [Export] NodePath nameLineEditNP;
    LineEdit nameLineEdit;
    [Export] NodePath saveButtonNP;
    Button saveButton;


    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.ShowNameSetterPopup += OnShowNameSetterPopup;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.ShowNameSetterPopup -= OnShowNameSetterPopup;
    }

    void OnShowNameSetterPopup()
    {
        Visible = true;
        saveButton.Disabled = false;
        nameLineEdit.Editable = true;
    }

    public override void _Ready()
    {
        saveButton = GetNode<Button>(saveButtonNP);
        nameLineEdit = GetNode<LineEdit>(nameLineEditNP);
        saveButton.Disabled = true;
        nameLineEdit.Editable = false;

        saveButton.Connect("button_up", this, nameof(SavePressed));
    }

    public void SavePressed()
    {
        if(nameLineEdit.Text.Length>2)
        {
            GameEvents.PlayerSetName?.Invoke(nameLineEdit.Text);
            saveButton.Disabled = true;
            nameLineEdit.Editable = false;
            QueueFree();
        }
    }

    

}
