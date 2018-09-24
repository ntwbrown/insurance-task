// Javascript Code+ 


var Claim = function (data) {
    var self = this;

    self.claimdate = ko.observable('').extend({ required: true });

    if (typeof data !== 'undefined') {
        self.claimdate(data.claimdate);

        //self.size(data.size);
    }
}
var Driver = function (name, dob, occupation, claims) {
    var self = this;
    self.name = ko.observable().extend({ required: true });
    self.dob = ko.observable().extend({ required: true });
    self.occupation = ko.observable().extend({ required: true });
    self.claims = ko.observableArray([]);




    if (typeof cliams !== 'undefined') {
        $.each(claims, function (i, el) {
            self.claims.push(new Claim({ claimdate: el.claimdate, msg: el.msg }));
        });
    }
    self.removeClaim = function (claim) {
        // alert("ll");
        self.claims.remove(claim);
    };
    self.addClaim = function () {
        if (self.claims().length < 5) {
            self.claims.push(new Claim({ claimdate: '' }));
        } else {
            alert("Sorry, the maximum number of claims per driver is 5");

        }

    };
};

var PolicyModel = function (policy) {
    var self = this;
    var currentDate;
    var today = new Date();
    var dd = today.getDate();
    if (dd < 10) {
        dd = "0" + dd;
    }
    var mm = today.getMonth() + 1;
    if (mm < 10) {
        mm = "0" + mm;
    }
    var yyyy = today.getFullYear();

    currentDate = yyyy + '-' + mm + '-' + dd;
   
    self.startDateOfPolicy = ko.observable(currentDate).extend({ required: true });

    self.generalMessage = ko.observable('');
    self.policyResult = ko.observable('');
    self.drivers = ko.observableArray([]);

    self.occupations = ko.observableArray([
        {
            name: 'Other',
            id: '0'
        },
        {
            name: 'Chaueffeur',
            id: '1'
        }, {
            name: 'Accountant',
            id: '2'
        }
    ]),

        self.addDriver = function (name, dob, occupation) {

            if (self.drivers().length < 5) {
                self.drivers.push(new Driver(name, dob, occupation));
            } else {
                self.generalMessage("Sorry, you are not able to add any more drivers, the maximum number of drivers on a policy is 5.");
            }

        };

    self.removeDriver = function (driver) {
        self.drivers.remove(driver);
    };

    self.save = function (policy) {
        console.log("PA " + policy.startDateOfPolicy());
        ProcessApplication(policy);
  
    };



    function ProcessApplication(policy) {
        var self = policy;
        var jsonData = ko.toJSON(policy);
        console.log("JSON DATA " + jsonData);
     
        console.log("PA > " + policy.startDateOfPolicy());
  
        $.ajax({
            type: 'POST',
            url: uri + 'getquote',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: jsonData,
            crossDomain: true,
            success: function (data, status, xhr) {

                console.log('Data Received');
                console.log("Data " + data.startDateOfPolicy);
                self.policyResult(data.policyResult + ' ' + data.generalMessage);

            },
            error: function (data, status, xhr) {
                console.log('Error ');
            }
        });
    }
};


function ProcessApplication(policy) {
    var self = this;
  
    postData(policy);
};



const uri = 'http://localhost:55902/api/PolicyProcessing/';
let todos = null;


ko.applyBindings(new PolicyModel());