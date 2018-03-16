using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// File Id List.
    /// </summary>
    public class FileIdList : IList<string>
    {
        List<string> innerList;
        Action<string> fileIdsUpdated;

        /// <summary>
        /// Initialize List from fileIdsString reference.
        /// </summary>
        /// <param name="fileIds"></param>
        /// <param name="fileIdsUpdated"></param>
        public FileIdList(string fileIds, Action<string> fileIdsUpdated)
        {
            this.fileIdsUpdated = fileIdsUpdated;
            var innerFileIds = fileIds ?? string.Empty;
            innerFileIds.Trim().Trim('|');
            this.innerList = new List<string>(innerFileIds.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
        }

        void UpdateFileIdsString()
        {
            if (this.innerList.Count == 0)
            {
                this.fileIdsUpdated(string.Empty);
                return;
            }

            var sb = new StringBuilder();
            foreach (var item in this.innerList)
            {
                sb.Append(item + "|");
            }
            sb.Remove(sb.Length - 1, 1);
            this.fileIdsUpdated(sb.ToString());
        }

        /// <summary>
        /// Access item with index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get => this.innerList[index];
            set
            {
                this.innerList[index] = value;
                this.UpdateFileIdsString();
            }
        }

        /// <summary>
        /// Get count.
        /// </summary>
        public int Count => this.innerList.Count;

        /// <summary>
        /// IsReadOnly.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Add item.
        /// </summary>
        /// <param name="item"></param>
        public void Add(string item)
        {
            this.innerList.Add(item);
            this.UpdateFileIdsString();
        }

        /// <summary>
        /// Clear all items.
        /// </summary>
        public void Clear()
        {
            this.innerList.Clear();
            this.UpdateFileIdsString();
        }

        /// <summary>
        /// Check wheather list contains item specificed.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(string item)
        {
            return this.innerList.Contains(item);
        }

        /// <summary>
        /// Copy list into array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(string[] array, int arrayIndex)
        {
            this.innerList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get Enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            return this.innerList.GetEnumerator();
        }

        /// <summary>
        /// Get index of item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(string item)
        {
            return this.innerList.IndexOf(item);
        }

        /// <summary>
        /// Insert item into list at position that specified.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, string item)
        {
            this.innerList.Insert(index, item);
            this.UpdateFileIdsString();
        }

        /// <summary>
        /// Remove Item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(string item)
        {
            var result = this.innerList.Remove(item);
            if (result)
                this.UpdateFileIdsString();
            return result;
        }

        /// <summary>
        /// Remove item at index specified.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            this.innerList.RemoveAt(index);
            this.UpdateFileIdsString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.innerList.GetEnumerator();
        }
    }
}
