<?php
namespace Rapleaf;

include 'rapleaf_config.php';

// Use this method with an (optional) email address to create the html tag which will call out to Rapleaf.
// e.g. print Rapleaf\request("someone@example.com");
function request($email = NULL)
{
    return "<script type=\"text/javascript\" src=\"" . build_url($email) . "\"></script>";
}

function build_url($email = NULL){
    $api_key = get_api_key();
    $acct = get_account_number();
    $redirect_url = get_location() . "rapleaf_response.php";
    $url = "http://personalize.rlcdn.com/v4/wv?url=" . urlencode($redirect_url) . "&acct=" . $acct;
    if($email != NULL){
        $encrypted_data = encrypt_args($api_key, "email=" . $email);
        $url = $url . "&a=" . urlencode($encrypted_data);
    }
    return $url;
}

/* Utility method used by the above.
 */
function encrypt_args($api_key, $args){
    $method = "BF-ECB";
    $encrypted_data = openssl_encrypt($args, $method, substr($api_key, 0, 16));
    return $encrypted_data;
}

?>