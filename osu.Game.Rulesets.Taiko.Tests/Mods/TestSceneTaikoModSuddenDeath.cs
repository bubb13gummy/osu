// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Taiko.Mods;
using osu.Game.Rulesets.Taiko.Objects;
using osu.Game.Rulesets.Taiko.Replays;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Taiko.Tests.Mods
{
    public partial class TestSceneTaikoModSuddenDeath : ModFailConditionTestScene
    {
        protected override Ruleset CreatePlayerRuleset() => new TaikoRuleset();

        public TestSceneTaikoModSuddenDeath()
            : base(new TaikoModSuddenDeath())
        {
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        public void TestFailsExactlyAtMissCount(int missCount) => CreateModTest(new ModTestData
        {
            Mod = new TaikoModSuddenDeath { MissCount = { Value = missCount } },
            PassCondition = () => ((ModFailConditionTestPlayer)Player).CheckFailed(true),
            Autoplay = false,
            CreateBeatmap = () => new Beatmap
            {
                HitObjects = createHitObjects(missCount)
            }
        });

        [TestCase(5)]
        public void TestDoesNotFailBeforeMissCountReached(int missCount) => CreateModTest(new ModTestData
        {
            Mod = new TaikoModSuddenDeath { MissCount = { Value = missCount } },
            PassCondition = () => ((ModFailConditionTestPlayer)Player).CheckFailed(false),
            Autoplay = false,
            CreateBeatmap = () => new Beatmap
            {
                HitObjects = createMixedHitObjects(missCount - 1, 10)
            },
            ReplayFrames = createReplayFramesSkippingFirst(missCount - 1, 10)
        });

        private List<HitObject> createHitObjects(int count)
        {
            var objects = new List<HitObject>();

            for (int i = 0; i < count; i++)
            {
                objects.Add(new Hit
                {
                    StartTime = 1000 + i * 1000,
                    Type = HitType.Centre
                });
            }

            return objects;
        }

        private List<HitObject> createMixedHitObjects(int missCount, int extraHits)
        {
            var objects = new List<HitObject>();

            for (int i = 0; i < missCount; i++)
            {
                objects.Add(new Hit
                {
                    StartTime = 1000 + i * 500,
                    Type = HitType.Centre
                });
            }

            for (int i = 0; i < extraHits; i++)
            {
                objects.Add(new Hit
                {
                    StartTime = 1000 + (missCount + i) * 500,
                    Type = HitType.Centre
                });
            }

            return objects;
        }

        private List<ReplayFrame> createReplayFramesSkippingFirst(int missCount, int extraHits)
        {
            var frames = new List<ReplayFrame>();

            for (int i = 0; i < extraHits; i++)
            {
                double time = 1000 + (missCount + i) * 500;
                frames.Add(new TaikoReplayFrame(time, TaikoAction.LeftCentre));
                frames.Add(new TaikoReplayFrame(time + 20));
            }

            return frames;
        }
    }
}
