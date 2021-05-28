using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration;
        private DataAccess _db;
        private string _collection = "Campus";

        public CampusController(IConfiguration configuration)
        {
            _configuration = configuration;
            _db = new DataAccess(_configuration);
        }

        [HttpGet]
        public IEnumerable<Campus> GetAll()
        {
            return _db.GetDocuments<Campus>(_collection);
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult GetDocumentById(string id)
        {
            var result = _db.GetDocumentById<Campus>(_collection, new ObjectId(id));
            if (result == null) { return NotFound(); }
            return new ObjectResult(result);
        }

        [HttpPost]
        public IActionResult InsertDocument([FromBody] Campus document)
        {
            _db.InsertDocument(_collection, document);
            return new OkResult();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult ReplaceDocument(string id, [FromBody] Campus document)
        {
            var result = _db.GetDocumentById<Campus>(_collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            _db.ReplaceDocument(_collection, new ObjectId(id), document);
            return new OkResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteDocument(string id)
        {
            var result = _db.GetDocumentById<Campus>(_collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            _db.DeleteDocument<Campus>(_collection, new ObjectId(id));
            return new OkResult();
        }
    }
}
