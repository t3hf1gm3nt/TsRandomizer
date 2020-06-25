﻿using NUnit.Framework;
using Timespinner.GameAbstractions.Inventory;
using TsRanodmizer;
using TsRanodmizer.IntermediateObjects;
using TsRanodmizer.Randomisation;
using TsRanodmizer.Randomisation.ItemPlacers;

namespace TsRandomizer.Tests
{
	[TestFixture]
	class ForwardFillingRandomizerFixture
	{
		[Test]
		public void Should_generate_beatable_seed_in_1_pass()
		{
			var itemLocations = new ItemLocationMap();

			var seed = new Seed(1);
			var unlockingMap = new ItemUnlockingMap(seed);

			ForwardFillingItemLocationRandomizer.AddRandomItemsToLocationMap(seed, unlockingMap, itemLocations);

			Assert.That(itemLocations.IsBeatable(), Is.True);
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		public void Should_fill_tuturial_with_melee_and_spellorb(int seedIndex)
		{
			var itemLocations = new ItemLocationMap();

			var seed = new Seed(seedIndex);
			var unlockingMap = new ItemUnlockingMap(seed);

			ForwardFillingItemLocationRandomizer.AddRandomItemsToLocationMap(seed, unlockingMap, itemLocations);

			Assert.That(itemLocations[ItemKey.TutorialMeleeOrb].ItemInfo.LootType, Is.EqualTo(LootType.Orb));
			Assert.That(itemLocations[ItemKey.TutorialMeleeOrb].ItemInfo.OrbSlot, Is.EqualTo(EOrbSlot.Melee));

			Assert.That(itemLocations[ItemKey.TutorialSpellOrb].ItemInfo.LootType, Is.EqualTo(LootType.Orb));
			Assert.That(itemLocations[ItemKey.TutorialSpellOrb].ItemInfo.OrbSlot, Is.EqualTo(EOrbSlot.Spell));
		}
	}
}