<?php
    namespace Rapleaf;

    // Set these values, the RAPLEAF_LOCATION should end with a '/' e.g. http://www.yoursite.com/rapleaf/
    // and the file rapleaf_response.php should be hosted here i.e. http://www.yoursite.com/rapleaf/rapleaf_response.php

    define('RAPLEAF_API_KEY', "SET_ME");
    define('RAPLEAF_ACCOUNT_NUMBER', "SET_ME");
    define('RAPLEAF_LOCATION', "http://SET_ME/");
    
    function get_api_key()
    {
        return RAPLEAF_API_KEY;
    }

    function get_account_number()
    {
        return RAPLEAF_ACCOUNT_NUMBER; 
    }

    function get_location()
    {
        return RAPLEAF_LOCATION; 
    }
?>