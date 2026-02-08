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
        ConnectionDisconnected,

        // Banner colors (ModuleSolver)
        InfoBanner,
        WarningBanner,

        // Chat colors
        ChatLink,
        ChatTimestamp,

        // HP bar colors (SpawnTracker)
        HpBarCritical,  // < 30% HP (red)
        HpBarWarning,   // 30-60% HP (yellow)
        HpBarHealthy,   // > 60% HP (green)

        // Debug colors (NetDebug)
        DebugSeen,
        DebugUnseen,

        // Status/state colors
        EncounterPausedBackground,    // For encounter saving paused state
        EncounterPausedText,
        EncounterResumedButton,        // For resume button in paused state

        // Window state colors
        WindowPinned,                  // Pin icon color when window is topmost
        WindowUnpinned,                // Pin icon color when window is not topmost
        WindowCollapsed,               // Collapse button when collapsed
        WindowExpanded,                // Collapse button when expanded

        // Common UI colors
        QualityBasic,                  // Item quality: Basic (blue)
        QualityAdvanced,               // Item quality: Advanced (purple)
        QualityExcellent,              // Item quality: Excellent (gold)

        // Alpha helper colors (transparent variants)
        TransparentWhite,
        TransparentBlack,

        // Border/separator colors
        BorderSubtle,
        BorderStrong
    }
}
