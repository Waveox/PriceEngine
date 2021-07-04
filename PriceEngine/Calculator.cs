using PriceEngine.Interfaces;
using System;

namespace PriceEngine
{
    public static class Calculator
    {
        public static decimal? Calc(IProduct product, int units)
        {
            decimal? price;
            int singleUnits;

            if (units <= 0) return 0m;

            if (units % product.CartonSize == 0)
            {
                price = CartonsPrice((units / product.CartonSize), product.BoxFullCost);
            }
            else
            {
                var fullBoxes = Math.DivRem(units, product.CartonSize, out singleUnits);
                price = CartonsPrice(fullBoxes, product.BoxFullCost) + (singleUnits * SingleUnitCost(product));
            }

            return price;
        }

        public static decimal? SingleUnitCost(IProduct product)
        {
            var markupCoeff = (decimal)product.SingleUnitMarkupPercentage / 100;
            var unitPriceWithoutMarkup = product.BoxFullCost / product.CartonSize;
            return unitPriceWithoutMarkup + (unitPriceWithoutMarkup * markupCoeff);
        }

        public static decimal? CartonsPrice(int fullBoxes, decimal? boxPrice)
        {
            const int discUnits = 3;
            const int discAmount = 10;

            return discUnits >= 3 ? (fullBoxes * boxPrice) - (fullBoxes * boxPrice * discAmount / 100) : fullBoxes * boxPrice;
        }
    }
}
