using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Core;
using WeChatMomentExport.Utils;

namespace WeChatMomentExport
{
    class Program
    {
        static void Main(string[] args)
        {
            MomentExporterFacade exporterFacade = new MomentExporterFacade("91ec3e7cf49aae1ce8a6437dc58aa759", true);
            exporterFacade.Start();
        }
    }
}
