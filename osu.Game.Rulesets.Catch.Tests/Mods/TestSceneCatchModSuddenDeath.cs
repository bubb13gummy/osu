// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Catch.Mods;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Catch.Tests.Mods
{
    public partial class TestSceneCatchModSuddenDeath : ModFailConditionTestScene
    {
        protected override Ruleset CreatePlayerRuleset() => new CatchRuleset();

        public TestSceneCatchModSuddenDeath()
            : base(new CatchModSuddenDeath())
        {
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        public void TestFailsExactlyAtMissCount(int missCount) => CreateModTest(new ModTestData
        {
            Mod = new CatchModSuddenDeath { MissCount = { Value = missCount } },
            PassCondition = () => ((ModFailConditionTestPlayer)Player).CheckFailed(true),
            Autoplay = false,
            CreateBeatmap = () => new Beatmap
            {
                HitObjects = createFruits(missCount)
            }
        });

        [TestCase(5)]
        public void TestDoesNotFailBeforeMissCountReached(int missCount) => CreateModTest(new ModTestData
        {
            Mod = new CatchModSuddenDeath { MissCount = { Value = missCount } },
            PassCondition = () => ((ModFailConditionTestPlayer)Player).CheckFailed(false),
            Autoplay = false,
            CreateBeatmap = () => new Beatmap
            {
                BeatmapInfo = { Difficulty = new BeatmapDifficulty { DrainRate = 0 } },
                HitObjects = createFruits(missCount - 1)
            }
        });

        private List<HitObject> createFruits(int count)
        {
            var objects = new List<HitObject>();

            for (int i = 0; i < count; i++)
            {
                objects.Add(new Fruit
                {
                    StartTime = 1000 + i * 1000,
                    X = 0
                });
            }

            return objects;
        }
    }
}
