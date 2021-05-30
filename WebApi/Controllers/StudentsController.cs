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
    public class StudentsController : ControllerBase
    {
        private DatabaseHelper _db;
        const string collection = "Students";
        private IConfiguration _configuration;

        public StudentsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _db = new DatabaseHelper(_configuration);
        }

        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return _db.GetDocuments<Student>(collection);
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult GetStudent(string id)
        {
            var student = _db.GetDocumentById<Student>(collection, new ObjectId(id));
            if (student == null) { return NotFound(); }
            return new ObjectResult(student);
        }

        [HttpPost]
        public IActionResult InsertStudent([FromBody] Student student)
        {
            _db.InsertDocument(collection, student);
            return new OkResult();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult ReplaceDocument(string id, [FromBody] Student student)
        {
            var document = _db.GetDocumentById<Student>(collection, new ObjectId(id));
            if (document == null) { return new NotFoundResult(); }
            _db.ReplaceDocument(collection, new ObjectId(id), student);
            return new OkResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteDocument(string id)
        {
            var document = _db.GetDocumentById<Student>(collection, new ObjectId(id));
            if (document == null) { return new NotFoundResult(); }
            _db.DeleteDocument<Student>(collection, new ObjectId(id));
            return new OkResult();
        }
    }
}
