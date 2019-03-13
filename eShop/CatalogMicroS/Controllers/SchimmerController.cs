using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogMicroS.DL;
using Microsoft.AspNetCore.Mvc;

namespace CatalogMicroS.Controllers
{
    public class SchimmerController : Controller
    {
        private ISiteRepository _siteRepository;

        public SchimmerController(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }
        public object Index()
        {
            return _siteRepository.GetAllFromBase();
        }
    }
}