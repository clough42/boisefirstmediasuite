using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixControl
{
    /// <summary>
    /// A class that holds a string item and its associated index
    /// </summary>
    class IndexedItem
    {
        private string itemName;
        private int itemIndex;

        public IndexedItem(string itemName, int itemIndex)
        {
            this.itemName = itemName;
            this.itemIndex = itemIndex;
        }

        public string ItemName
        {
            get { return this.itemName; }
        }

        public int ItemIndex
        {
            get { return this.itemIndex; }
        }

        public override string ToString()
        {
            return this.ItemName;
        }

        public override bool Equals(object obj)
        {
            IndexedItem that = obj as IndexedItem;
            if (that == null)
            {
                return false;
            }

            return this.ItemIndex == that.ItemIndex;
        }

    }
}
