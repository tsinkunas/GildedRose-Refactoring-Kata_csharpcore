using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace csharpcore.test
{
    public class NormalItemTest
    {
        //At the end of each day our system lowers both values for every item
        [Fact]
        public void ShouldDecreaseBothValues()
        {
            const int startSellIn = 10;
            const int startQuality = 10;

            var item = new Item { Name = "+5 Dexterity Vest", SellIn = startSellIn, Quality = startQuality };
            var app = new GildedRose(new List<Item> { item });

            app.UpdateQuality();
            Assert.Equal("+5 Dexterity Vest", item.Name);
            Assert.Equal(startSellIn - 1, item.SellIn);
            Assert.Equal(startQuality - 1, item.Quality);
        }

        //Once the sell by date has passed, Quality degrades twice as fast
        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, 1)]
        [InlineData(0, 2)]
        [InlineData(-1, 2)]
        public void ShouldDecreaseQualityBySellInValue(int sellIn, int expectedDecrease)
        {
            const int startQuality = 15;

            var item = new Item { Name = "+5 Dexterity Vest", SellIn = sellIn, Quality = startQuality };
            var app = new GildedRose(new List<Item> { item });

            app.UpdateQuality();

            Assert.Equal(startQuality - expectedDecrease, item.Quality);
        }

        //The Quality of an item is never negative
        [Fact]
        public void ShouldNeverDecreaseQualityToNegativeValue()
        {
            const int startQuality = 0;

            var item = new Item { Name = "+5 Dexterity Vest", SellIn = 3, Quality = startQuality };
            var app = new GildedRose(new List<Item> { item });

            app.UpdateQuality();
            app.UpdateQuality();
            app.UpdateQuality();
            app.UpdateQuality();

            Assert.Equal(0, item.Quality);
        }
    }
}
