using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]

    public class TodoController : ControllerBase
    {
        private readonly PolicyContext _context;
     //   private readonly PolicyContext _pcontext;
        public TodoController(PolicyContext context)
        {
            _context = context;

            //if (_context.PolicyItems.Count() == 0)
            //{
            //    // Create a new TodoItem if collection is empty,
            //    // which means you can't delete all TodoItems.
            //    _context.PolicyItems.Add(new Policy { policyResult="SUCCESS", generalMessage= "Your Application was approved" });
            //    _context.PolicyItems.Add(new Policy { policyResult = "DECLINE", generalMessage = "Your Application was declined" });
            //    _context.SaveChanges();
            //}

            //if (_context.TodoItems.Count() == 0)
            //{
            //    Create a new TodoItem if collection is empty,
            //     which means you can't delete all TodoItems.
            //    _context.TodoItems.Add(new TodoItem { QuoteNumber = "229922", Result = "SUCCESS" });

            //    _context.SaveChanges();
            //}

        }

        [HttpGet]
        public List<Policy> GetAll()
        {
            return _context.PolicyItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.PolicyItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetByPol(string data)
        {

          //  DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(object));
            var policys = JsonConvert.DeserializeObject<object>(data);
            
            Boolean declineRules = false;
          
            // Stage 1 Check Decline Rules
           // declineRules = CheckDeclineRules(policys);
            // Stage 2 Prepare Quote
            if (!declineRules)
            {
        //        policys.policyResult= processQuote(policys);

            }


                

        
            var item = _context.PolicyItems.Take(1);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        private string processQuote(Policy policy)
        {
           return policy.drivers.Count().ToString();
        }

        private bool CheckDeclineRules(Policy policy)
        {
            throw new NotImplementedException();
        }
    }
}