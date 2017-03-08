using FutebolTempoRealApp.Model.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FutebolTempoRealApp.Model
{
    public class SectionList<T> : List<ItemSection<T>>
    {
        public SectionList(IEnumerable<IGrouping<string, T>> grupos)
        {
            foreach (var grupo in grupos)
            {
                if (string.IsNullOrWhiteSpace(grupo.Key)) continue;
                Add(new ItemSection<T>(grupo.Key));
                foreach (var item in grupo)
                {
                    Add(new ItemSection<T>(item));
                }
            }
        }
    }

    public class ItemSection<T>
    {
        public string Titulo { get; set; }
        public T Item { get; set; }

        public bool IsSection
        {
            get { return !string.IsNullOrWhiteSpace(Titulo); }
        }

        public ItemSection(string titulo)
        {
            Titulo = titulo;
        }

        public ItemSection(T item)
        {
            this.Item = item;
        }
        
    }
}
