using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Helpers;

namespace VidPub.Web.Infrastructure {

    public class DynamicModelBinder : DefaultModelBinder {

        public override object BindModel(ControllerContext controllerContext, 
            ModelBindingContext bindingContext) {
            
            if (controllerContext.Controller.GetType().BaseType == typeof(CruddyController)) {
                var request = controllerContext.HttpContext.Request;
                var controller = (CruddyController)controllerContext.Controller;
                if (request.Form.Keys.Count > 0) {
                    var table = controller._table;
                    return table.CreateFrom(request.Form);
                } else {
                    return base.BindModel(controllerContext, bindingContext);

                }
            } else {

                return base.BindModel(controllerContext, bindingContext);
            }
        }

    }
}