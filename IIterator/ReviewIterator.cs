using System;
using System.Collections.Generic;
using Web_BanXeMoTo.IIterator;

namespace Web_BanXeMoTo.Models
{
    public class ReviewIterator : IIterator<ChiTietDg>
    {
        private List<ChiTietDg> reviews;
        private int currentIndex;

        public ReviewIterator(List<ChiTietDg> reviews)
        {
            this.reviews = reviews;
            currentIndex = 0;
        }

        public bool HasNext()
        {
            return currentIndex < reviews.Count;
        }

        public ChiTietDg Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No more reviews to iterate");
            }

            return reviews[currentIndex++];
        }
    }
}