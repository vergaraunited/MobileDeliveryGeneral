using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDeliveryGeneral.Definitions
{
    public static class enums
    {
        public enum stopstate { Loaded, Complete, Incomplete, InProgress}

        public enum SPCmds { INSERTMANIFESTDETAILS, INSERTMANIFEST, GETAVAILABLEDRIVERS, GETDRIVERMANIFEST, GETTRUCKS, GETSTOPS, GETORDERS, GETORDERDETAILS, GETORDEROPTIONS, GETMANIFEST, INSERTORDERDETAILS, INSERTORDER, INSERTSCANFILE, INSERTORDEROPTIONS, COMPLETEORDER, COMPLETESTOP, GETDELIVEREDORDERS }

    }
}
