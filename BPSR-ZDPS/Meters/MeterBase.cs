using BPSR_ZDPS.DataTypes;
using BPSR_ZDPS.Windows;
using Hexa.NET.ImGui;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BPSR_ZDPS.Meters
{
    public class MeterBase
    {
        // Width configuration constants - edit here to change meter widths across all meters
        public const float RightSideWidth = 100.0f;  // Right side width for damage/DPS/healing values (unscaled)
        public const float ClassWidthEnglish = 50.0f;  // Class/sub-profession width for English (unscaled)
        public const float ClassWidthWide = 35.0f;  // Class/sub-profession width for JP/CN (unscaled)
        public const float NameCharWidthRomaji = 20.0f;  // Per-character width for ASCII/romaji names (unscaled)
        public const float NameCharWidthWide = 20.0f;  // Per-character width for CJK names (unscaled)
        public const float UndefinedNameWidth = 80.0f;  // Total width for undefined names "[U:...]" (unscaled)
        public const float RankWidth = 40.0f;  // Rank number width (unscaled)

        public string Name = "";

        public Encounter? ActiveEncounter = null;

        // Cache for height calculation to avoid recalculating every frame
        private int _cachedEntryCount = -1;
        private float _cachedHeight = 0.0f;

        // Cache for width calculation to avoid recalculating every frame
        private int _cachedWidthEntityCount = -1;
        private float _cachedWidthScale = -1.0f;
        private bool _cachedWidthShowIcons = false;
        private bool _cachedWidthShowSubProfession = false;
        private bool _cachedWidthShowAbilityScore = false;
        private bool _cachedWidthShowSeasonStrength = false;
        private bool _cachedWidthShowTruePerSecond = false;
        private float _cachedWidth = 0.0f;

        public virtual void Draw(MainWindow mainWindow) { }

        /// <summary>
        /// Returns the number of entries that will be displayed in this meter.
        /// Used for calculating the exact height needed for auto-sizing the window.
        /// </summary>
        public virtual int GetEntryCount()
        {
            return 0;
        }

        /// <summary>
        /// Returns calculated height for the ListBox based on entry count.
        /// Using a simple per-entry height to avoid over-expansion.
        /// </summary>
        protected float GetListHeight(int entryCount)
        {
            if (entryCount <= 0)
                return 0.0f;

            // Return cached height if entry count hasn't changed
            if (entryCount == _cachedEntryCount)
                return _cachedHeight;

            // Calculate entry height based on actual font scale used in rendering
            // The meters push font: 14.0f * MeterBarScale, then use AlignTextToFramePadding()
            float scale = DataTypes.Settings.Instance.WindowSettings.MainWindow.MeterBarScale;

            // Push the same font that will be used during rendering to get accurate frame height
            ImGui.PushFont(HelperMethods.Fonts["Cascadia-Mono"], 14.0f * scale);
            float frameHeight = ImGui.GetFrameHeight();
            ImGui.PopFont();

            // Each entry: ProgressBar + Selectable with AlignTextToFramePadding
            // AlignTextToFramePadding makes the Selectable frame height larger
            float entryHeight = frameHeight;

            // ListBox has internal padding AND spacing between entries
            float listPadding = ImGui.GetStyle().WindowPadding.Y * 2;
            float itemSpacing = ImGui.GetStyle().ItemSpacing.Y * Math.Max(0, entryCount - 1);

            float totalHeight = (entryCount * entryHeight) + itemSpacing + listPadding;

            // Update cache
            _cachedEntryCount = entryCount;
            _cachedHeight = totalHeight;

            return totalHeight;
        }

        /// <summary>
        /// Invalidates the height calculation cache, forcing recalculation on next frame.
        /// Call this when settings change that affect the window height.
        /// </summary>
        public void InvalidateHeightCache()
        {
            _cachedEntryCount = -1;
        }

        /// <summary>
        /// Invalidates the width calculation cache, forcing recalculation on next frame.
        /// Call this when settings change that affect the window width.
        /// </summary>
        public void InvalidateWidthCache()
        {
            _cachedWidthEntityCount = -1;
            _cachedWidthScale = -1.0f;
        }

        /// <summary>
        /// Calculates minimum meter width based on entity data and current settings.
        /// This is a shared static method that can be called from anywhere (including MainWindow).
        /// Does NOT use caching - callers should handle their own caching if needed.
        /// </summary>
        /// <summary>
        /// Calculate name width based on character count and type (romaji vs wide/CJK).
        /// ASCII characters use NameCharWidthRomaji, others use NameCharWidthWide.
        /// </summary>
        static float CalculateNameWidth(string name, float scale)
        {
            float width = 0.0f;
            foreach (char c in name)
            {
                // ASCII (romaji/English) = narrow, CJK = wide
                if (c < 128)
                    width += NameCharWidthRomaji;
                else
                    width += NameCharWidthWide;
            }
            return width * scale;
        }

        public static float CalculateMinMeterWidth(List<Entity> entities)
        {
            float scale = DataTypes.Settings.Instance.WindowSettings.MainWindow.MeterBarScale;
            bool showIcons = Settings.Instance.ShowClassIconsInMeters;
            bool showSubProfession = Settings.Instance.ShowSubProfessionNameInMeters;
            bool showAbilityScore = Settings.Instance.ShowAbilityScoreInMeters;
            bool showSeasonStrength = Settings.Instance.ShowSeasonStrengthInMeters;

            ImGui.PushFont(HelperMethods.Fonts["Cascadia-Mono"], 14.0f * scale);

            // Left side: rank + icon + spacing
            float leftWidth = RankWidth * scale;
            if (showIcons)
                leftWidth += ImGui.GetFrameHeight() + ImGui.GetStyle().ItemSpacing.X * 2;
            else
                leftWidth += ImGui.GetStyle().ItemSpacing.X;

            // Right side: fixed width for damage/DPS/percentage values
            float rightWidth = RightSideWidth * scale;

            // Middle: find the longest actual name in the encounter (character-based)
            float maxNameWidth = 0.0f;
            float maxClassWidth = 0.0f;

            if (entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    float nameWidth;
                    string displayName;
                    if (string.IsNullOrEmpty(entity.Name))
                    {
                        nameWidth = UndefinedNameWidth * scale;
                        displayName = $"[U:{entity.UID}]";
                    }
                    else
                    {
                        nameWidth = CalculateNameWidth(entity.Name, scale);
                        displayName = entity.Name;
                    }
                    maxNameWidth = Math.Max(maxNameWidth, nameWidth);
                    Log.Debug($"MeterBase name: '{displayName}' | width: {nameWidth:F1}");
                }
            }
            else
            {
                // Default placeholder for empty encounters
                maxNameWidth = UndefinedNameWidth * scale;
            }

            // Use fixed class width based on language detection
            if (showSubProfession)
            {
                // Check if any entity has non-English sub-profession (simple detection)
                bool hasWideClass = entities.Any(e => !string.IsNullOrEmpty(e.SubProfession) && e.SubProfession.Any(c => c >= 128));
                maxClassWidth = (hasWideClass ? ClassWidthWide : ClassWidthEnglish) * scale;
            }

            // Add spacing between name and class
            float middleWidth = maxNameWidth + maxClassWidth;

            // Ability score width like " (12345)"
            if (showAbilityScore)
            {
                middleWidth += ImGui.CalcTextSize(" (99999)").X;
            }
            else if (showSeasonStrength)
            {
                middleWidth += ImGui.CalcTextSize(" (99999)").X;
            }

            // Add padding
            float padding = ImGui.GetStyle().WindowPadding.X * 2;
            float itemSpacing = ImGui.GetStyle().ItemSpacing.X;

            ImGui.PopFont();

            float totalWidth = leftWidth + middleWidth + rightWidth + padding + itemSpacing;

            // Debug: Log width breakdown
            Log.Debug($"Width[STATIC]: scale={scale:F2} | left={leftWidth:F1} | middle={middleWidth:F1} (name={maxNameWidth:F1}, class={maxClassWidth:F1}) | right={rightWidth:F1} | padding={padding:F1} | spacing={itemSpacing:F1} | TOTAL={totalWidth:F1}");

            return totalWidth;
        }

        /// <summary>
        /// Calculates minimum meter width with caching for meter instances.
        /// This prevents the window from collapsing when there's little content.
        /// Uses caching to avoid recalculating when data just reorders.
        /// </summary>
        protected float GetMinMeterWidth(List<Entity> entities)
        {
            float scale = DataTypes.Settings.Instance.WindowSettings.MainWindow.MeterBarScale;

            // Check if cache is valid
            bool showIcons = Settings.Instance.ShowClassIconsInMeters;
            bool showSubProfession = Settings.Instance.ShowSubProfessionNameInMeters;
            bool showAbilityScore = Settings.Instance.ShowAbilityScoreInMeters;
            bool showSeasonStrength = Settings.Instance.ShowSeasonStrengthInMeters;
            bool showTruePerSecond = Settings.Instance.DisplayTruePerSecondValuesInMeters;

            if (_cachedWidthEntityCount == entities.Count &&
                _cachedWidthScale == scale &&
                _cachedWidthShowIcons == showIcons &&
                _cachedWidthShowSubProfession == showSubProfession &&
                _cachedWidthShowAbilityScore == showAbilityScore &&
                _cachedWidthShowSeasonStrength == showSeasonStrength &&
                _cachedWidthShowTruePerSecond == showTruePerSecond)
            {
                // Cache hit - return cached width
                return _cachedWidth;
            }

            // Cache miss - use static calculation
            float totalWidth = CalculateMinMeterWidth(entities);

            // Update cache
            _cachedWidthEntityCount = entities.Count;
            _cachedWidthScale = scale;
            _cachedWidthShowIcons = showIcons;
            _cachedWidthShowSubProfession = showSubProfession;
            _cachedWidthShowAbilityScore = showAbilityScore;
            _cachedWidthShowSeasonStrength = showSeasonStrength;
            _cachedWidthShowTruePerSecond = showTruePerSecond;
            _cachedWidth = totalWidth;

            return totalWidth;
        }

        public static bool SelectableWithHintImage(string number, string name, string value, int profession)
        {
            var startPoint = ImGui.GetCursorPos();

            ImGui.AlignTextToFramePadding();

            float texSize = ImGui.GetItemRectSize().Y;
            float offset = ImGui.CalcTextSize(number).X;
            if (Settings.Instance.ShowClassIconsInMeters)
            {
                offset += (ImGui.GetStyle().ItemSpacing.X * 2) + (texSize + 2);
            }
            else
            {
                offset += (ImGui.GetStyle().ItemSpacing.X);
            }

            ImGui.SetCursorPosX(offset);

            // Add semi-transparent background for better text contrast against colored progress bar
            Vector4 bgColor = Settings.Instance.Theme == ETheme.Light
                ? new Vector4(1, 1, 1, 0.7f)  // Light theme: white with opacity
                : new Vector4(0, 0, 0, 0.5f); // Dark/Black theme: black with opacity
            ImGui.PushStyleColor(ImGuiCol.Header, bgColor);
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, bgColor);
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, bgColor);

            bool ret = ImGui.Selectable(name, false, ImGuiSelectableFlags.SpanAllColumns);

            ImGui.PopStyleColor(3);
            ImGui.SameLine();

            ImGui.SetCursorPos(startPoint);

            ImGui.TextUnformatted(number);

            if (Settings.Instance.ShowClassIconsInMeters)
            {
                ImGui.SameLine();
                var tex = ImageHelper.GetTextureByKey($"Profession_{profession}_Slim");
                if (tex == null)
                {
                    ImGui.Dummy(new Vector2(texSize, texSize));
                }
                else
                {
                    var roleIconColor = DataTypes.Professions.RoleTypeIconColors(
                        DataTypes.Professions.GetRoleFromBaseProfessionId(profession),
                        DataTypes.Settings.Instance.Theme
                    );

                    if (DataTypes.Settings.Instance.ColorClassIconsByRole)
                    {
                        ImGui.ImageWithBg((ImTextureRef)tex, new Vector2(texSize, texSize), new Vector2(0, 0), new Vector2(1, 1), new Vector4(0, 0, 0, 0), roleIconColor);
                    }
                    else
                    {
                        ImGui.Image((ImTextureRef)tex, new Vector2(texSize, texSize));
                    }
                }
            }

            ImGui.SameLine();
            // Right-align value by calculating position from window width
            float scale = DataTypes.Settings.Instance.WindowSettings.MainWindow.MeterBarScale;
            float valueWidth = ImGui.CalcTextSize(value).X;
            float windowWidth = ImGui.GetWindowWidth();
            float padding = ImGui.GetStyle().WindowPadding.X;
            float targetX = windowWidth - valueWidth - padding - 5.0f;  // Nudge 5px left
            float currentX = ImGui.GetCursorPosX();
            if (targetX > currentX)
                ImGui.SetCursorPosX(targetX);
            ImGui.TextUnformatted(value);

            return ret;
        }
    }
}
