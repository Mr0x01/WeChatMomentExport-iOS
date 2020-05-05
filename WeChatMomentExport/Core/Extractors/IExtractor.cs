using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Core.Extractors
{
    public abstract class IExtractor<T> where T : class
    {
        protected NSArray _object { get; set; }
        protected int i { get; set; }
        public IExtractor(NSArray _object, int i)
        {
            this._object = _object;
            this.i = i;
        }

        public abstract T Extract();
    }
}
