namespace BPSR_ZDPS.DataTypes
{
    /// <summary>
    /// Semantic color keys for theme-aware UI colors.
    /// Use with Theme.GetColor(ThemeColor.X) to get theme-appropriate colors.
    /// </summary>
    public enum ThemeColor
    {
        // Text colors
        PrimaryText,
        WarningText,
        SuccessText,

        // Button colors
        SelectedButton,
        DestructiveButton,
        ButtonRed,

        // Buff header colors (for EntityInspector buff events)
        BuffHeader_Default,      // buffTypeColor == 99
        BuffHeader_Debuff,       // buffTypeColor == 0
        BuffHeader_Buff,         // buffTypeColor == 1
        BuffHeader_Unknown,      // buffTypeColor == 2

        // Status colors
        ConnectionConnected,
        ConnectionDisconnected
    }
}
