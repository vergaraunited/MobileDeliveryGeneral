using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDeliveryGeneral.Definitions
{
    public static class enums
    {
        public enum stopstate { Loaded, Complete, Incomplete, InProgress}

        public enum SPCmds { INSERTMANIFESTDETAILS, INSERTMANIFEST, GETAVAILABLEDRIVERS, GETDRIVERMANIFEST, GETTRUCKS, GETSTOPS, GETORDERS, GETORDERDETAILS, GETMANIFEST, INSERTORDERDETAILS, INSERTORDER, INSERTORDEROPTIONS, COMPLETESTOP}

    }
}
