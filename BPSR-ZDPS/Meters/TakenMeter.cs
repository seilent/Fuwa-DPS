using BPSR_ZDPS.DataTypes;
using BPSR_ZDPS.Windows;
using Hexa.NET.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZLinq;

namespace BPSR_ZDPS.Meters
{
    public class TakenMeter : MeterBase
    {
        // Cache for width calculation specific to NPC Taken meter
        private int _cachedWidthEntityCount = -1;
        private float _cachedWidthScale = -1.0f;
        private bool _cachedWidthShowTruePerSecond = false;
        private float _cachedWidth = 0.0f;

        public TakenMeter()
        {
            Name = "NPC Taken";
        }

        bool SelectableWithHint(string label, string hint)
        {
            ImGui.AlignTextToFramePadding(); // This makes the entries about 1/3 larger but keeps it nicely centered
            bool ret = ImGui.Selectable(label);
            ImGui.SameLine();
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + Math.Max(0.0f, ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(hint).X));
            ImGui.TextUnformatted(hint);
            return ret;
        }

        public override int GetEntryCount()
        {
            if (Settings.Instance.KeepPastEncounterInMeterUntilNextDamage)
            {
                if (ActiveEncounter?.BattleId != EncounterManager.Current?.BattleId)
                {
                    ActiveEncounter = EncounterManager.Current;
                }
                else if (ActiveEncounter?.EncounterId != EncounterManager.Current?.EncounterId)
                {
                    if (EncounterManager.Current.HasStatsBeenRecorded())
                    {
                        ActiveEncounter = EncounterManager.Current;
                    }
                }
            }
            else
            {
                if (ActiveEncounter?.EncounterId != EncounterManager.Current?.EncounterId || ActiveEncounter?.BattleId != EncounterManager.Current?.BattleId)
                {
                    ActiveEncounter = EncounterManager.Current;
                }
            }

            return ActiveEncounter?.Entities.AsValueEnumerable()
                .Where(x => x.Value.EntityType == Zproto.EEntityType.EntMonster)
                .Count() ?? 0;
        }

        /// <summary>
        /// Calculate minimum width for NPC Taken meter.
        /// This meter shows monster names (no class icons or professions) with HP data and damage taken.
        /// Uses caching to avoid recalculating when data just reorders.
        /// </summary>
        protected float GetMinMeterWidth()
        {
            float scale = DataTypes.Settings.Instance.WindowSettings.MainWindow.MeterBarScale;
            bool showTruePerSecond = Settings.Instance.DisplayTruePerSecondValuesInMeters;

            var entities = ActiveEncounter?.Entities.AsValueEnumerable()
                .Where(x => x.Value.EntityType == Zproto.EEntityType.EntMonster)
                .Select(x => x.Value)
                .ToList() ?? new List<Entity>();

            // Check cache
            if (_cachedWidthEntityCount == entities.Count &&
                _cachedWidthScale == scale &&
                _cachedWidthShowTruePerSecond == showTruePerSecond)
            {
                return _cachedWidth;
            }

            // Cache miss - recalculate
            ImGui.PushFont(HelperMethods.Fonts["Cascadia-Mono"], 14.0f * scale);

            // Find the longest entity name
            float maxNameWidth = 0.0f;
            if (entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    string name = !string.IsNullOrEmpty(entity.Name) ? entity.Name : $"[U:{entity.UID}]";
                    // Format: "Name [UID]"
                    string nameFormat = $"{name} [{entity.UID}]";
                    maxNameWidth = Math.Max(maxNameWidth, ImGui.CalcTextSize(nameFormat).X);
                }
            }
            else
            {
                // Default placeholder for empty encounters
                maxNameWidth = ImGui.CalcTextSize("Unknown Monster [12345678]").X;
            }

            // Right side: fixed width for HP data + damage values
            float rightWidth = RightSideWidth * scale;

            // Add padding
            float padding = ImGui.GetStyle().WindowPadding.X * 2;
            float itemSpacing = ImGui.GetStyle().ItemSpacing.X;

            ImGui.PopFont();

            float totalWidth = maxNameWidth + rightWidth + padding + itemSpacing;

            // Update cache
            _cachedWidthEntityCount = entities.Count;
            _cachedWidthScale = scale;
            _cachedWidthShowTruePerSecond = showTruePerSecond;
            _cachedWidth = totalWidth;

            return totalWidth;
        }

        public override void Draw(MainWindow mainWindow)
        {
            // Calculate exact height based on entry count
            float listHeight = GetListHeight(GetEntryCount());
            // Calculate minimum width to prevent window collapse
            float minWidth = GetMinMeterWidth();

            if (ImGui.BeginListBox("##TakenMeterList", new Vector2(minWidth, listHeight)))
            {
                if (Settings.Instance.KeepPastEncounterInMeterUntilNextDamage)
                {
                    if (ActiveEncounter?.BattleId != EncounterManager.Current?.BattleId)
                    {
                        ActiveEncounter = EncounterManager.Current;
                    }
                    else if (ActiveEncounter?.EncounterId != EncounterManager.Current?.EncounterId)
                    {
                        if (EncounterManager.Current.HasStatsBeenRecorded())
                        {
                            ActiveEncounter = EncounterManager.Current;
                        }
                    }
                }
                else
                {
                    if (ActiveEncounter?.EncounterId != EncounterManager.Current?.EncounterId || ActiveEncounter?.BattleId != EncounterManager.Current?.BattleId)
                    {
                        ActiveEncounter = EncounterManager.Current;
                    }
                }

                var playerList = ActiveEncounter?.Entities.AsValueEnumerable().Where(x => x.Value.EntityType == Zproto.EEntityType.EntMonster).OrderByDescending(x => x.Value.TotalTakenDamage).ToArray();

                ulong topTotalValue = 0;

                for (int i = 0; i < playerList?.Count(); i++)
                {
                    var entity = playerList[i].Value;

                    if (i == 0 && Settings.Instance.NormalizeMeterContributions)
                    {
                        topTotalValue = entity.TotalTakenDamage;
                    }

                    string name = "Unknown";
                    if (!string.IsNullOrEmpty(entity.Name))
                    {
                        name = entity.Name;
                    }

                    if (AppState.PlayerUUID != 0 && AppState.PlayerUUID == entity.UUID)
                    {
                        AppState.PlayerMeterPlacement = i + 1;
                        AppState.PlayerTotalMeterValue = entity.TotalTakenDamage;
                        AppState.PlayerMeterValuePerSecond = entity.TakenStats.ValuePerSecond;
                    }

                    double contribution = 0.0;
                    double contributionProgressBar = 0.0;
                    if (ActiveEncounter.TotalNpcTakenDamage != 0)
                    {
                        contribution = Math.Round(((double)entity.TotalTakenDamage / (double)ActiveEncounter.TotalNpcTakenDamage) * 100, 4);

                        if (Settings.Instance.NormalizeMeterContributions)
                        {
                            contributionProgressBar = Math.Round(((double)entity.TotalTakenDamage / (double)topTotalValue) * 100, 4);
                        }
                        else
                        {
                            contributionProgressBar = contribution;
                        }
                    }
                    string truePerSecond = "";
                    if (Settings.Instance.DisplayTruePerSecondValuesInMeters)
                    {
                        truePerSecond = $"[{Utils.NumberToShorthand(entity.TakenStats.TrueValuePerSecond)}] ";
                    }
                    string totalTaken = Utils.NumberToShorthand(entity.TotalTakenDamage);
                    string totalTps = Utils.NumberToShorthand(entity.TakenStats.ValuePerSecond);
                    StringBuilder format = new();
                    var startPoint = ImGui.GetCursorPos();
                    // ImGui.GetTextLineHeightWithSpacing();

                    ImGui.PushFont(HelperMethods.Fonts["Cascadia-Mono"], 14.0f * Settings.Instance.WindowSettings.MainWindow.MeterBarScale);

                    bool hasHpData = entity.Hp >= 0 && entity.MaxHp > 0;
                    if (hasHpData && (Settings.Instance.MeterSettingsNpcTakenShowHpData || Settings.Instance.MeterSettingsNpcTakenUseHpMeter))
                    {
                        var healthPct = MathF.Round((float)entity.Hp / (float)entity.MaxHp, 4);

                        if (Settings.Instance.MeterSettingsNpcTakenShowHpData)
                        {
                            format.Append("[ ");

                            format.Append(Utils.NumberToShorthand(entity.Hp));

                            if (!Settings.Instance.MeterSettingsNpcTakenHideMaxHp)
                            {
                                format.Append($" / {Utils.NumberToShorthand(entity.MaxHp)}");
                            }

                            format.Append($" ({(healthPct * 100).ToString("00.00")}%)");

                            format.Append(" ]");
                        }

                        if (Settings.Instance.MeterSettingsNpcTakenUseHpMeter)
                        {
                            ImGui.SetCursorPos(startPoint);
                            //ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0, 0, 0, 0));
                            ImGui.PushStyleColor(ImGuiCol.PlotHistogram, Colors.DarkRed);

                            ImGui.ProgressBar(healthPct, new Vector2(-1, 0), $"##TakenEntryHealth_{i}");
                            ImGui.PopStyleColor();
                        }
                    }

                    if (!Settings.Instance.MeterSettingsNpcTakenUseHpMeter || !hasHpData)
                    {
                        ImGui.ProgressBar((float)contributionProgressBar / 100.0f, new Vector2(-1, 0), $"##TakenEntryContribution_{i}");
                    }

                    format.Append($" {totalTaken} {truePerSecond}({totalTps}) {contribution.ToString("F0").PadLeft(3, ' ')}%");

                    ImGui.SetCursorPos(startPoint);
                    if (SelectableWithHint($"{name} [{entity.UID.ToString()}]##TakenEntry_{i}", format.ToString()))
                    {
                        mainWindow.entityInspector = new EntityInspector();
                        mainWindow.entityInspector.LoadEntity(entity, ActiveEncounter.StartTime);
                        mainWindow.entityInspector.Open();
                    }

                    ImGui.PopFont();
                }

                ImGui.EndListBox();
            }
        }
    }
}
