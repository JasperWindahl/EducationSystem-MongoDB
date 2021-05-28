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
    public class StaffController : ControllerBase
    {
        private readonly DataAccess db;
        const string _collection = "Staff";
        private IConfiguration _configuration;

        public StaffController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataAccess(_configuration);
        }

        [HttpGet]
        public IEnumerable<StaffMember> GetAll()
        {
            return db.GetDocuments<StaffMember>(_collection);
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult GetDocumentById(string id)
        {
            var result = db.GetDocumentById<StaffMember>(_collection, new ObjectId(id));
            if (result == null) { return NotFound(); }
            return new ObjectResult(result);
        }

        [HttpPost]
        public IActionResult InsertDocument([FromBody] StaffMember document)
        {
            db.InsertDocument(_collection, document);
            return new OkResult();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult ReplaceDocument(string id, [FromBody] StaffMember document)
        {
            var result = db.GetDocumentById<StaffMember>(_collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            db.ReplaceDocument(_collection, new ObjectId(id), document);
            return new OkResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteDocument(string id)
        {
            var result = db.GetDocumentById<StaffMember>(_collection, new ObjectId(id));
            if (result == null) { return new NotFoundResult(); }
            db.DeleteDocument<StaffMember>(_collection, new ObjectId(id));
            return new OkResult();
        }
    }
}
