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
    public class StaffController : ControllerBase
    {
        private readonly DataAccess db;
        const string collection = "Staff";

        public StaffController()
        {
            db = new DataAccess();
        }

        [HttpGet]
        public IEnumerable<StaffMember> GetAll()
        {
            return db.GetDocuments<StaffMember>(collection);
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult GetDocumentById(string id)
        {
            var result = db.GetDocumentById<StaffMember>(collection, new ObjectId(id));
            if (result == null) { return NotFound(); }
            return new ObjectResult(result);
        }

        [HttpPost]
        public IActionResult InsertDocument([FromBody] StaffMember document)
        {
            db.InsertDocument(collection, document);
            return new OkResult();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult ReplaceDocument(string id, [FromBody] StaffMember document)
        {
            var result = db.GetDocumentById<StaffMember>(collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            db.ReplaceDocument(collection, new ObjectId(id), document);
            return new OkResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteDocument(string id)
        {
            var result = db.GetDocumentById<StaffMember>(collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            db.DeleteDocument<StaffMember>(collection, new ObjectId(id));
            return new OkResult();
        }
    }
}
