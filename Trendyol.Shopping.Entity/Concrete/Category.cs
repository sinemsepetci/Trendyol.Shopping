using System;
using System.Collections.Generic;
using Trendyol.Shopping.Entity.Abstract;

namespace Trendyol.Shopping.Entity.Concrete
{
    public class Category : IEntity
    {
        public string Id { get; set; }
        public Category ParentCategory { get; set; }
        public string Name { get; set; }
        //public List<Campaign> Campaigns { get; set; }
    }
}