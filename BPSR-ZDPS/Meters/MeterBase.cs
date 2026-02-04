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
        public string Name = "";

        protected Encounter? ActiveEncounter = null;

        // Cache for height calculation to avoid recalculating every frame
        private int _cachedEntryCount = -1;
        private float _cachedHeight = 0.0f;

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

            Log.Debug("[GetListHeight] entryCount={EntryCount}, scale={Scale:F2}, frameHeight={FrameHeight:F2}, entryHeight={EntryHeight:F2}, itemSpacing={ItemSpacing:F2}, listPadding={ListPadding:F2}, totalHeight={TotalHeight:F2}", entryCount, scale, frameHeight, entryHeight, itemSpacing, listPadding, totalHeight);

            return totalHeight;
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
                    var roleColor = DataTypes.Professions.RoleTypeColors(DataTypes.Professions.GetRoleFromBaseProfessionId(profession));

                    if (DataTypes.Settings.Instance.ColorClassIconsByRole)
                    {
                        ImGui.ImageWithBg((ImTextureRef)tex, new Vector2(texSize, texSize), new Vector2(0, 0), new Vector2(1, 1), new Vector4(0, 0, 0, 0), roleColor);
                    }
                    else
                    {
                        ImGui.Image((ImTextureRef)tex, new Vector2(texSize, texSize));
                    }
                }
            }

            ImGui.SameLine();
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + Math.Max(0.0f, ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(value).X));
            ImGui.TextUnformatted(value);

            return ret;
        }
    }
}
