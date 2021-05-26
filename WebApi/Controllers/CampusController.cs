using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using WebApi.DatabaseHelper;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CampusController : ControllerBase
    {
        private readonly DataAccess db;
        const string collection = "Campus";

        public CampusController()
        {
            db = new DataAccess();
        }

        [HttpGet]
        public IEnumerable<Campus> GetAll()
        {
            return db.GetDocuments<Campus>(collection);
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult GetDocumentById(string id)
        {
            var result = db.GetDocumentById<Campus>(collection, new ObjectId(id));
            if (result == null) { return NotFound(); }
            return new ObjectResult(result);
        }

        [HttpPost]
        public IActionResult InsertDocument([FromBody] Campus document)
        {
            db.InsertDocument(collection, document);
            return new OkResult();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult ReplaceDocument(string id, [FromBody] Campus document)
        {
            var result = db.GetDocumentById<Campus>(collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            db.ReplaceDocument(collection, new ObjectId(id), document);
            return new OkResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteDocument(string id)
        {
            var result = db.GetDocumentById<Campus>(collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            db.DeleteDocument<Campus>(collection, new ObjectId(id));
            return new OkResult();
        }
    }
}
