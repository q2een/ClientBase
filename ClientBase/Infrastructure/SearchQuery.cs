﻿using System.Collections.Generic;

namespace ClientBase.Infrastructure
{
    public class SearchQuery
    {
        public string Text { get; set; }

        public IList<int> Except { get; set; } = new int[0];

        public int Count { get; set; }

        public void Deconstruct(out string text, out IList<int> except, out int count)
        {
            text = Text;
            except = Except;
            count = Count;
        }
    }
}
