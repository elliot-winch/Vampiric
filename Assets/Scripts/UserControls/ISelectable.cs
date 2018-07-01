
using System.Collections.Generic;

public interface ISelectionChangesControls {

    UserControlScheme GetControlScheme();
}

public interface IControlSchemeHolder
{
    UserControlScheme GetControlScheme();
}