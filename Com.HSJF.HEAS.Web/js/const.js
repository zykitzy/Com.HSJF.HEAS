var ACTION = {
    "Add": "add",
    "Edit": "edit",
    "Remove": "remove",
    "View": "view"
};

var FIELD_DISPLAY = {
	"Introducer":{
	    "Name": {
	        "Visible": true,
	        "Index": 1
	    },
	    "Contract": {
	        "Visible": false,
	        "Index": 2
	    },
	    "RebateAmmount": {
	        "Visible": true,
	        "Index": 3
	    },
	    "RebateRate": {
	        "Visible": true,
	        "Index": 4
	    },
	    "Account": {
	        "Visible": false,
	        "Index": 5
	    },
	    "AccountBank": {
	        "Visible": false,
	        "Index": 6
	    }
	},
    "RelationPerson": {
        "Name": {
            "Visible": true,
            "Index": 1
        },
        "RelationTypeText": {
            "Visible": true,
            "Index": 2
        },
        "IdentificationTypeText": {
            "Visible": true,
            "Index": 3
        },
        "IdentificationNumber": {
            "Visible": true,
            "Index": 4
        },
        "MaritalStatusText": {
            "Visible": true,
            "Index": 5
        }
    },
    "Contacts": {
        "ContactTypeText": {
            "Visible": true,
            "Index": 1
        },
        "ContactNumber": {
            "Visible": true,
            "Index": 2
        },
        "IsDefault": {
            "Visible": true,
            "Index": 3
        }
    },
    "EmergencyContacts": {
        "ContactTypeText": {
            "Visible": true,
            "Index": 1
        },
        "Name": {
            "Visible": true,
            "Index": 2
        },
        "ContactNumber": {
            "Visible": true,
            "Index": 3
        }
    },
    "RelationEnterprise": {
        "EnterpriseName": {
            "Visible": true,
            "Index": 1
        },
        "LegalPerson": {
            "Visible": true,
            "Index": 2
        },
        "RegisteredCapital": {
            "Visible": true,
            "Index": 3
        }
    },
    "Addresses": {
        "AddressTypeText": {
            "Visible": true,
            "Index": 1
        },
        "AddressInfo": {
            "Visible": true,
            "Index": 2
        },
        "IsDefault": {
            "Visible": true,
            "Index": 3
        }
    },
    "Collateral": {
        "CollateralTypeText": {
            "Visible": true,
            "Index": 1
        },
        "BuildingName": {
            "Visible": true,
            "Index": 2
        },
        "Address": {
            "Visible": true,
            "Index": 3
        },
        "RightOwner": {
            "Visible": true,
            "Index": 4
        },
        "HouseSize": {
            "Visible": true,
            "Index": 5
        }
    },
    "RelationPersonAudits": {
        "Name": {
            "Visible": true,
            "Index": 1
        },
        "RelationTypeText": {
            "Visible": true,
            "Index": 2
        },
        "IdentificationTypeText": {
            "Visible": true,
            "Index": 3
        },
        "IdentificationNumber": {
            "Visible": true,
            "Index": 4
        },
        "MaritalStatusText": {
            "Visible": true,
            "Index": 5
        }
    },
    "ContactAudits": {
        "ContactTypeText": {
            "Visible": true,
            "Index": 1
        },
        "ContactNumber": {
            "Visible": true,
            "Index": 2
        },
        "IsDefault": {
            "Visible": true,
            "Index": 3
        }
    },
    "EmergencyContactAudits": {
        "ContactTypeText": {
            "Visible": true,
            "Index": 1
        },
        "Name": {
            "Visible": true,
            "Index": 2
        },
        "ContactNumber": {
            "Visible": true,
            "Index": 3
        }
    },
    "RelationEnterpriseAudits": {
        "EnterpriseName": {
            "Visible": true,
            "Index": 1
        },
        "LegalPerson": {
            "Visible": true,
            "Index": 2
        },
        "RegisteredCapital": {
            "Visible": true,
            "Index": 3
        }
    },
    "AddressAudits": {
        "AddressTypeText": {
            "Visible": true,
            "Index": 1
        },
        "AddressInfo": {
            "Visible": true,
            "Index": 2
        },
        "IsDefault": {
            "Visible": true,
            "Index": 3
        }
    },
    "CollateralAudits": {
        "CollateralTypeText": {
            "Visible": true,
            "Index": 1
        },
        "BuildingName": {
            "Visible": true,
            "Index": 2
        },
        "Address": {
            "Visible": true,
            "Index": 3
        },
        "RightOwner": {
            "Visible": true,
            "Index": 4
        },
        "HouseSize": {
            "Visible": true,
            "Index": 5
        }
    },
    "IndividualCredits": {
        "PersonIDText": {
            "Visible": true,
            "Index": 1
        },
        "CreditCard": {
            "Visible": true,
            "Index": 2
        },
        "CreditInfo": {
            "Visible": true,
            "Index": 3
        },
        "OtherCredit": {
            "Visible": true,
            "Index": 4
        },
        "OverdueInfo": {
            "Visible": true,
            "Index": 5
        }
    },
    "EnterpriseCredits": {
        "EnterpriseIDText": {
            "Visible": true,
            "Index": 1
        },
        "CreditCard": {
            "Visible": true,
            "Index": 2
        },
        "CreditInfo": {
            "Visible": true,
            "Index": 3
        }
    },
    "EnforcementPersons": {
        "PersonIDText": {
            "Visible": true,
            "Index": 1
        },
        "EnforcementWeb": {
            "Visible": true,
            "Index": 2
        },
        "TrialRecord": {
            "Visible": true,
            "Index": 3
        },
        "LawXP": {
            "Visible": true,
            "Index": 4
        },
        "ShiXin": {
            "Visible": true,
            "Index": 5
        },
        "BadNews": {
            "Visible": true,
            "Index": 6
        },
    },
    "IndustryCommerceTaxs": {
        "EnterpriseIDText": {
            "Visible": true,
            "Index": 1
        },
        "AnnualInspection": {
            "Visible": true,
            "Index": 2
        },
        "ActualManagement": {
            "Visible": true,
            "Index": 3
        }
    },
    "HouseDetails": {
        "CollateralIDText": {
            "Visible": true,
            "Index": 1
        },
        "AssessedValue": {
            "Visible": true,
            "Index": 2
        },
        "HouseType": {
            "Visible": true,
            "Index": 3
        },
        "Collateral": {
            "Visible": true,
            "Index": 4
        }
    },
    "EstimateSources": {
        "EstimateInstitutions": {
            "Visible": true,
            "Index": 1
        },
        "RushEstimate": {
            "Visible": true,
            "Index": 2
        }
    },
    "Guarantors": {
        "Name": {
            "Visible": true,
            "Index": 0
        },
        "ContactNumber": {
            "Visible": true,
            "Index": 1
        },
        "RelationTypeText": {
            "Visible": true,
            "Index": 2
        },
        "GuarantType": {
            "Visible": true,
            "Index": 3
        }
    }
};

var maxfileSize = 30 * 1024 * 1024;

//请求超时设置（ms）
var timeout = 60000;


//var loca = "http://localhost:6963/";

var getProvince =  "/Api/Estimate/GetProvince";

var getCitybyId =  "/Api/Estimate/GetCity?ProvinceID=";

var getAreaById =  "/Api/Estimate/GetArea?cityID=";

var getConstructionById =  "/Api/Estimate/GetConstruction";

var getBuildingById = "/Api/Estimate/GetBuilding";

var getHouseById = "/Api/Estimate/GetHouse";

var getAutoPriceByInfo = "/Api/Estimate/GetAutoPrice";
