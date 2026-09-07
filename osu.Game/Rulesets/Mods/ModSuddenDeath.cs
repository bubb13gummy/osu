// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModSuddenDeath : ModFailCondition
    {
        public override string Name => "Sudden Death";
        public override string Acronym => "SD";
        public override IconUsage? Icon => OsuIcon.ModSuddenDeath;
        public override ModType Type => ModType.DifficultyIncrease;
        public override LocalisableString Description => "Miss and fail.";
        public override bool Ranked => true;
        public override bool ValidForFreestyleAsRequiredMod => true;
        public override Type[] IncompatibleMods => base.IncompatibleMods.Append(typeof(ModPerfect)).ToArray();

        [SettingSource("Miss count", "Fail after this many misses (1 = instant death)")]
        public BindableInt MissCount { get; } = new BindableInt(1)
        {
            MinValue = 1,
            MaxValue = 50
        };

        private int currentMisses;

        protected override bool FailCondition(HealthProcessor healthProcessor, JudgementResult result)
        {
            if (!(result.Type.AffectsCombo() && !result.IsHit))
                return false;

            return ++currentMisses >= MissCount.Value;
        }
    }
}
