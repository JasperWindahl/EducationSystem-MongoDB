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
    public class CoursesController : ControllerBase
    {
        private readonly DataAccess db;
        const string collection = "Courses";

        public CoursesController()
        {
            db = new DataAccess();
        }

        [HttpGet]
        public IEnumerable<Course> GetAll()
        {
            return db.GetDocuments<Course>(collection);
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult GetDocumentById(string id)
        {
            var result = db.GetDocumentById<Course>(collection, new ObjectId(id));
            if (result == null) { return NotFound(); }
            return new ObjectResult(result);
        }

        [HttpPost]
        public IActionResult InsertDocument([FromBody] Course document)
        {
            db.InsertDocument(collection, document);
            return new OkResult();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult ReplaceDocument(string id, [FromBody] Course document)
        {
            var result = db.GetDocumentById<Course>(collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            db.ReplaceDocument(collection, new ObjectId(id), document);
            return new OkResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteDocument(string id)
        {
            var result = db.GetDocumentById<Course>(collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            db.DeleteDocument<Course>(collection, new ObjectId(id));
            return new OkResult();
        }
    }
}
