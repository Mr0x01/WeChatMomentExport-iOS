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
            MomentExporterFacade exporterFacade = new MomentExporterFacade("49baa62c85d746efdb46eb2ff92a446b", true);
            exporterFacade.Start();
        }
    }
}
