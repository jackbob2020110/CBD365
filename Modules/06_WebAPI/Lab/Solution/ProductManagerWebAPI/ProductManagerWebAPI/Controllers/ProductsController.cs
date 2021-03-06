﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using ProductManagerWebAPI.Models;


namespace ProductManagerWebAPI.Controllers {
  public class ProductsController : ODataController {
    private ProductsDBEntities db = new ProductsDBEntities();

    // GET: odata/Products
    [EnableQuery]
    public IQueryable<Product> GetProducts() {
      return db.Products;
    }

    // GET: odata/Products(5)
    [EnableQuery]
    public SingleResult<Product> GetProduct([FromODataUri] int key) {
      return SingleResult.Create(db.Products.Where(product => product.Id == key));
    }

    // PUT: odata/Products(5)
    public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Product> patch) {
      Validate(patch.GetEntity());

      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      Product product = await db.Products.FindAsync(key);
      if (product == null) {
        return NotFound();
      }

      patch.Put(product);

      try {
        await db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) {
        if (!ProductExists(key)) {
          return NotFound();
        }
        else {
          throw;
        }
      }

      return Updated(product);
    }

    // POST: odata/Products
    public async Task<IHttpActionResult> Post(Product product) {
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      db.Products.Add(product);
      await db.SaveChangesAsync();

      return Created(product);
    }

    // PATCH: odata/Products(5)
    [AcceptVerbs("PATCH", "MERGE")]
    public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> patch) {
      Validate(patch.GetEntity());

      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      Product product = await db.Products.FindAsync(key);
      if (product == null) {
        return NotFound();
      }

      patch.Patch(product);

      try {
        await db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) {
        if (!ProductExists(key)) {
          return NotFound();
        }
        else {
          throw;
        }
      }

      return Updated(product);
    }

    // DELETE: odata/Products(5)
    public async Task<IHttpActionResult> Delete([FromODataUri] int key) {
      Product product = await db.Products.FindAsync(key);
      if (product == null) {
        return NotFound();
      }

      db.Products.Remove(product);
      await db.SaveChangesAsync();

      return StatusCode(HttpStatusCode.NoContent);
    }

    protected override void Dispose(bool disposing) {
      if (disposing) {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    private bool ProductExists(int key) {
      return db.Products.Count(e => e.Id == key) > 0;
    }
  }
}
