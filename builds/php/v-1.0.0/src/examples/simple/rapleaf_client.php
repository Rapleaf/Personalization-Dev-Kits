<?php
    namespace Rapleaf;

    include "rapleaf_config.php";

    function get_personalization()
    {
        $response = get_response();
        $json_string = openssl_decrypt($response, "BF-ECB", get_api_key());
        $personalization = json_decode($json_string);
        return $personalization;
    }

    /*

    Advanced users may wish to alter the behaviour of set_response and get_response methods to store the response on the session
    rather than a cookie.

    */

    function set_response($response)
    {
        setcookie("rapleaf-response", $response, time() + (86400 * 7));
    }

    function get_response()
    {
        return $_COOKIE["rapleaf-response"];
    }
?>