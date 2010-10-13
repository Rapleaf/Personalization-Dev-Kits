<?php
namespace Rapleaf;

include 'rapleaf_client.php';

set_response($_GET["response"]);
// JavaScript callback 'onRapleafResponse'
print "if(typeof onRapleafResponse == 'function'){onRapleafResponse();}";
?>