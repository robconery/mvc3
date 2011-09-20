using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace VidPub.Web.Model {
    public class VideoLog:DynamicModel {
        public VideoLog():base("Vidpub","VideoLog","ID") {

        }
    }
}