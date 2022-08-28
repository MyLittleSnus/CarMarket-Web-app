using System;
using Razor_Test.Models;

namespace Razor_Test.AdditionalEntities
{
    public class NewOfferWithImageBox
    {
        public IFormFile File { get; set; }
        public Offer Offer { get; set; }
    }
}

