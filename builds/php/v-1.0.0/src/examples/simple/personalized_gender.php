<?php
    include "rapleaf_client.php";

    $personalization = Rapleaf\get_personalization();
    $gender = $personalization->{'gender'};
    $color = $gender == "Male" ? "#5186E8" : "#E64963";
    print "<div style=\"margin: 20px; padding: 10px; border: 1px solid black; width: 100px; height: 100px; background-color:" . $color . ";\">We picked this color for you, hope you like it!</div>";
?>