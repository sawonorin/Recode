﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class Page<T>
    {
        public int PageSize { get; }
        public int PageNumber { get; }
        public int TotalSize { get; }
        public T[] Items { get; set; }

        public Page(T[] items, int totalSize, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            TotalSize = totalSize;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Items = items;
        }
    }
}
