﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatalogMicroS.Models;
using System.Net;
using CatalogMicroS.DL;

namespace CatalogMicroS.Controllers
{
    public class CatalogController : Controller
    {
        ICatalogRepository _catalogRepository;

        public CatalogController(ICatalogRepository repository)
        {
            _catalogRepository = repository;
        }

        [HttpGet]
        [Route("Item/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemById(long id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item =  await _catalogRepository.GetItemById(id);

            if (item != null)
            {
                return Ok(item);
            }

            return NotFound();
        }
    }
}