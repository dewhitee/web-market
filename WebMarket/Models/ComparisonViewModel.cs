﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public class ComparisonViewModel
    {
        public static List<CatalogViewModel.Product> Products = new List<CatalogViewModel.Product>();
        public static CatalogViewModel.Product LeftProduct = new CatalogViewModel.Product();
        public static CatalogViewModel.Product RightProduct = new CatalogViewModel.Product();

    }
}