using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Collections.Generic;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private DatabaseHelper _db;
        private string _collection = "Courses";
        private IConfiguration _configuration;

        public CoursesController(IConfiguration configuration)
        {
            _db = new DatabaseHelper(_configuration);
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Course> GetAll()
        {
            return _db.GetDocuments<Course>(_collection);
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult GetDocumentById(string id)
        {
            var result = _db.GetDocumentById<Course>(_collection, new ObjectId(id));
            if (result == null) { return NotFound(); }
            return new ObjectResult(result);
        }

        [HttpPost]
        public IActionResult InsertDocument([FromBody] Course document)
        {
            _db.InsertDocument(_collection, document);
            return new OkResult();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult ReplaceDocument(string id, [FromBody] Course document)
        {
            var result = _db.GetDocumentById<Course>(_collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            _db.ReplaceDocument(_collection, new ObjectId(id), document);
            return new OkResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteDocument(string id)
        {
            var result = _db.GetDocumentById<Course>(_collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            _db.DeleteDocument<Course>(_collection, new ObjectId(id));
            return new OkResult();
        }
    }
}
