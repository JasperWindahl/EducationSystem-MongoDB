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
    public class StudentsController : ControllerBase
    {
        private readonly DataAccess db;
        const string collection = "Students";

        public StudentsController()
        {
            db = new DataAccess();
        }

        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return db.GetDocuments<Student>(collection);
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult GetStudent(string id)
        {
            var student = db.GetDocumentById<Student>(collection, new ObjectId(id));
            if (student == null) { return NotFound(); }
            return new ObjectResult(student);
        }

        [HttpPost]
        public IActionResult InsertStudent([FromBody] Student student)
        {
            db.InsertDocument(collection, student);
            return new OkResult();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult ReplaceDocument(string id, [FromBody] Student student)
        {
            var document = db.GetDocumentById<Student>(collection, new ObjectId(id));
            if (document == null) { return new NotFoundResult(); }
            db.ReplaceDocument(collection, new ObjectId(id), student);
            return new OkResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteDocument(string id)
        {
            var document = db.GetDocumentById<Student>(collection, new ObjectId(id));
            if (document == null) { return new NotFoundResult(); }
            db.DeleteDocument<Student>(collection, new ObjectId(id));
            return new OkResult();
        }
    }
}
