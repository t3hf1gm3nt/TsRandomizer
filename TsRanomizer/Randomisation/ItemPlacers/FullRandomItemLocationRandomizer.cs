﻿using System;
using System.Linq;
using Timespinner.GameAbstractions.Inventory;
using TsRandomizer.Extensions;
using TsRandomizer.IntermediateObjects;

namespace TsRandomizer.Randomisation.ItemPlacers
{
	class FullRandomItemLocationRandomizer : ItemLocationRandomizer
	{
		FullRandomItemLocationRandomizer(ItemUnlockingMap unlockingMap, ItemLocationMap itemLocationMap) : base(itemLocationMap, unlockingMap)
		{
		}

		public static void AddRandomItemsToLocationMap(Seed seed, ItemUnlockingMap unlockingMap, ItemLocationMap itemLocationMap)
		{
			var random = new Random(seed);

			new FullRandomItemLocationRandomizer(unlockingMap, itemLocationMap)
				.AddRandomItemsToLocationMap(random);
		}

		void AddRandomItemsToLocationMap(Random random)
		{
			FillTutorial(random);
			PlaceGassMaskInALegalSpot(random);

			var alreadyAssingedItems = ItemLocations
				.Where(l => l.IsUsed)
				.Select(l => l.ItemInfo)
				.ToArray();

			var itemsThatUnlockProgression = UnlockingMap.ItemsThatUnlockProgression
				.Where(i => i != ItemInfo.Get(EInventoryRelicType.AirMask))
				.Where(i => !alreadyAssingedItems.Contains(i))
				.ToList();

			while (itemsThatUnlockProgression.Count > 0)
			{
				var item = itemsThatUnlockProgression.PopRandom(random);

				var location = GetUnusedItemLocation(random);

				PutItemAtLocation(item, location);
			}

			FillRemainingChests(random);
		}

		ItemLocation GetUnusedItemLocation(Random random)
		{
			var locations = ItemLocations
				.Where(l => !l.IsUsed);

			return locations.SelectRandom(random);
		}

		protected override void PutItemAtLocation(ItemInfo itemInfo, ItemLocation itemLocation)
		{
			var itemUnlocks = UnlockingMap.GetAllUnlock(itemInfo);

			itemLocation.SetItem(itemInfo, itemUnlocks);
		}
	}
}
