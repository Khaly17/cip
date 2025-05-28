using System;

namespace Gefco.CipQuai.Menu
{
    /// <summary>
    /// Interface of the menu page item model.
    /// </summary>
    public interface IMenuPageItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the icon source.
        /// </summary>
        /// <value>The icon source.</value>
        string IconSource { get; set; }

        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <value>The type of the target.</value>
        Type TargetType { get; set; }
    }
}