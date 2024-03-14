using EvilDICOM.Core;
using EvilDICOM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPPG
{
    internal class DcmReader
    {
        public string? Manufacturer { get; internal set; }

        public bool Read(DICOMObject dcm)
        {
            Manufacturer = dcm.FindFirst(TagHelper.Manufacturer).DData as string;
            return true;
        }
    }
}
