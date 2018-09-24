using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class PolicyProcessingController : Controller
    {

        public const double basePremium = 500;

        [HttpPost]
        [Route("getquote")]
        public IActionResult Post([FromBody] Policy _policy)
        {
            const int maxNumberOfClaimsPerDriver = 2;
            const int maxNumberOfClaimsPerPolicy = 3;
          
          

            Boolean declineRulesStartDate = false;
            String declineRulesYoungDriver;
            String declineRulesEldestDriver;
            String declineRulesDriverTooManyClaims;
            String declineRulesPolicyHasTooManyClaims;

            declineRulesStartDate = CheckDeclineRuleStartDateOfPolicy(_policy);
            declineRulesYoungDriver= CheckDeclineRuleYoungestDriver(_policy);
            declineRulesEldestDriver = CheckDeclineRuleEldestDriver(_policy);
            declineRulesDriverTooManyClaims = CheckDeclineRuleDriverClaims(_policy, maxNumberOfClaimsPerDriver);
            declineRulesPolicyHasTooManyClaims = CheckDeclineRulePolicyClaims(_policy, maxNumberOfClaimsPerPolicy);
            if (declineRulesStartDate)
            {
                DateTime now = DateTime.Now;
      
                _policy.policyResult = "Decline";
                _policy.generalMessage = "Start Date of Policy";
            }
            else if (declineRulesYoungDriver.Length>0) {
                _policy.policyResult = "Decline";
                _policy.generalMessage = "Age of Youngest Driver " + declineRulesYoungDriver;
            }

            else if (declineRulesEldestDriver.Length > 0)
            {
                _policy.policyResult = "Decline";
                _policy.generalMessage = "Age of Eldest Driver " + declineRulesEldestDriver;
            }
            else if (declineRulesDriverTooManyClaims.Length > 0)
            {
                _policy.policyResult = "Decline";
                _policy.generalMessage = "Driver has more than " + maxNumberOfClaimsPerDriver + " claims " + declineRulesYoungDriver;
            }
            else if (declineRulesPolicyHasTooManyClaims.Length > 0)
            {
                _policy.policyResult = "Decline";
                _policy.generalMessage = declineRulesPolicyHasTooManyClaims;
            }



            else
            {
                _policy.policyResult = "Success";
                _policy.generalMessage = "Your Quote is £" + processQuote(_policy);

            }
            return Json(_policy);
        }

        private string CheckDeclineRulePolicyClaims(Policy _policy, int maxNumberOfClaimsPerPolicy)
        {

            var response = "";
            int totalClaims = 0;


            foreach (Driver driver in _policy.drivers)
            {
                totalClaims += driver.claims.Count();
            }

            if (totalClaims> maxNumberOfClaimsPerPolicy)
            {
                response = "Policy has more than " + maxNumberOfClaimsPerPolicy + " claims.";
            }

            return response;
        }


        private string CheckDeclineRuleDriverClaims(Policy _policy, int maxNumberOfClaimsPerDriver)
        {
          
            var response = "";
            int testFailed = 0;


            foreach (Driver driver in _policy.drivers)
            {

        
                if (driver.claims.Count > maxNumberOfClaimsPerDriver)
                {
                    testFailed++;
                    if (testFailed == 1)
                    {
                        response += driver.name + "(" + driver.claims.Count + " claims)";
                    }
                    else
                    {
                        response += ", " + driver.name + "(" + driver.claims.Count + " claims)";
                    }

                }

            }

            return response;
        }

        private string CheckDeclineRuleEldestDriver(Policy _policy)
        {

            CultureInfo ci = new CultureInfo("en-GB");
            DateTime minDateofBirth = DateTime.Now.AddYears(-75);
            var response = "";
            int testFailed = 0;


            foreach (Driver driver in _policy.drivers)
            {

                DateTime currentDOB = DateTime.Parse(driver.dob, ci);
                if (currentDOB <= minDateofBirth)
                {
                    testFailed++;
                    if (testFailed == 1)
                    {
                        response += driver.name;
                    }
                    else
                    {
                        response += ", " + driver.name;
                    }

                }

            }

            return response;


        }

        private string CheckDeclineRuleYoungestDriver(Policy _policy)
        {

            CultureInfo ci = new CultureInfo("en-GB");
            DateTime minDateofBirth = DateTime.Now.AddYears(-21);
            var response="";
            int testFailed=0;


            foreach (Driver driver in _policy.drivers)
            {

                DateTime currentDOB = DateTime.Parse(driver.dob, ci);
                if (currentDOB >= minDateofBirth)
                {
                    testFailed++;
                    if (testFailed == 1)
                    {
                        response += driver.name;
                    } else
                    {
                        response += ", " + driver.name;
                    }
                 
                }
             
            }

            return response;
        
          
        }

        private string processQuote(Policy _policy)
        {
            // _policy.policyResult = "£500";
            //    _policy.policyResult = "Success...";
            double totalPremium =0;
            double calc = 0;
            double calc1 = 0;
            double calc2 = 0;

            double calculatedPremium =basePremium;
         

            foreach (Driver driver in _policy.drivers)
            {
                calc = 0;
                // Check Occupation Premiums
                switch (driver.occupation)
                {
                    case "1":
                        calc = calculatedPremium * 0.1;
                        break;
                    case "2":
                        calc = calculatedPremium * -0.1;
                        break;
                    default:
                        break;
                }
                calc1 = 0;
                // Check Age
                switch (ageInYears(driver.dob))
                {

                    case int n when (n > 21 && n < 25):
                        calc1 = calculatedPremium * 0.2;
                        break;
                    case int n when (n > 26 && n < 75):
                        calc1 = calculatedPremium * 0.1;
                        break;

                    default:
                        break;
                }
                // Check Claims
                calc2 = 0;
                foreach (Claim claim in driver.claims)
                {
                    switch (ageInYears(claim.claimdate))
                    {
                        case int n when (n >= 0 && n <= 1):
                            calc2 = calc2 + calculatedPremium * 0.2;
                            break;
                        case int n when (n >= 2 && n <= 5):
                            calc2 = calc2 + calculatedPremium * 0.1;
                            break;
                    }

                }

                // calculatedPremium += calc;
            };
            totalPremium = basePremium + calc + calc1 + calc2;

            return totalPremium.ToString();
           
      
        }

        private int ageInYears(string dob)
        {
            CultureInfo ci = new CultureInfo("en-GB");
            DateTime today = DateTime.Now.Date;
            int ageInYears = DateTime.Now.Year - DateTime.Parse(dob).Year;
            return ageInYears;

        }

        private bool CheckDeclineRuleStartDateOfPolicy(Policy policy)
        {

            CultureInfo ci = new CultureInfo("en-GB");
            DateTime startDate = DateTime.Parse(policy.startDateOfPolicy, ci);

            if (startDate >= DateTime.Now.Date)
            {
               return false;


            } else
            {
                return true;
            };

  
          
        }

     
    }

   

}

    