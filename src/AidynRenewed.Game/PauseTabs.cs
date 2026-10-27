using AidynRenewed.Core.Ui;
using Godot;

namespace AidynRenewed.Game;

// The tabs' interface is in AidynRenewed.Views, beside the Party tab, which the editor shows too.

public partial class SystemTabView : IPauseTab<SystemMenu>
{
    SystemMenu IPauseTab<SystemMenu>.Model { set => Menu = value; }

    void IPauseTab.Activate(bool showing)
    {
        ProcessMode = showing ? ProcessModeEnum.Inherit : ProcessModeEnum.Disabled;
        MouseFilter = showing ? MouseFilterEnum.Pass : MouseFilterEnum.Ignore;
    }
}

/// <remarks>The World tab steps from the menu's own frame (<see cref="WorldMenu.Step"/>), so showing changes nothing here.</remarks>
public partial class WorldTabView : IPauseTab<WorldMenu>
{
    WorldMenu IPauseTab<WorldMenu>.Model { set => Menu = value; }

    void IPauseTab.Activate(bool showing)
    {
    }
}
