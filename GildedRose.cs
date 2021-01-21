using ApprovalUtilities.Utilities;
using System;
using System.Collections.Generic;

namespace csharpcore
{
    public class GildedRose
    {
        private const int MaxItemQuality = 50;

        private readonly IList<Item> _items;

        public GildedRose(IList<Item> items) => _items = items;

        public void UpdateQuality() => _items.ForEach(UpdateItemQuality);

        private static void UpdateItemQuality(Item item)
        {
            switch (item.Name)
            {
                case "Sulfuras, Hand of Ragnaros":
                    return;

                case "Aged Brie":
                    UpdateAgedBrie(item);
                    break;

                case "Backstage passes to a TAFKAL80ETC concert":
                    UpdateBackstagePasses(item);
                    break;

                case "Conjured Mana Cake":
                    UpdateConjuredCake(item);
                    break;

                //Once the sell by date has passed, Quality degrades twice as fast
                default:
                    var qualityDecrease = item.SellIn > 0 ? 1 : 2;
                    item.Quality = Math.Max(item.Quality - qualityDecrease, 0);
                    break;
            }

            item.SellIn -= 1;

        }

        private static void UpdateAgedBrie(Item item)
        {
            //"Aged Brie" actually increases in Quality the older it gets
            var qualityIncrement = item.SellIn > 0 ? 1 : 2;
            item.Quality = Math.Clamp(item.Quality + qualityIncrement, 0, MaxItemQuality);
        }

        private static void UpdateBackstagePasses(Item item)
        {
            int ComputeQualityIncrement(int daysRemaining)
            {
                // "Infinite" increment when sell date is passed
                if (daysRemaining <= 0) return int.MinValue;
                if (daysRemaining <= 5) return 3;
                if (daysRemaining <= 10) return 2;
                return 1;
            }

            item.Quality = Math.Clamp(item.Quality + ComputeQualityIncrement(item.SellIn), 0, MaxItemQuality);
        }

        private static void UpdateConjuredCake(Item item)
        {
            // Decrease quality twice as fast as normal items
            var qualityDecrement = item.SellIn > 0 ? 2 : 4;
            item.Quality = Math.Max(item.Quality - qualityDecrement, 0);
        }

    }
}
