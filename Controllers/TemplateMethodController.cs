using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_BanXeMoTo.Controllers
{
    public abstract class TemplateMethodController : Controller
    {
        protected abstract void PrintRoutes();
        protected abstract void PrintDIs();//depandentdeintraction

        //template Method
        public void PrintInfomation()
        {
            PrintRoutes();
            PrintDIs();
        }
    }
}
