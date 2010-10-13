<script type="text/javascript">
    function onRapleafResponse(){
        /* AJAX - for near instant personalization of a homepage */

        var req = new XMLHttpRequest();
        req.open("GET", "http://localhost/personalized_gender.php", true);
        req.onreadystatechange = function() {
            if(req.readyState === 4) {
                document.getElementById('personalized_gender').innerHTML = req.responseText;
            }
        }
        req.send();
    }
</script>

<?php
  print "Welcome " . "<b>" . $_GET['email'] . "!</b>";
?>

<div id="personalized_gender"></div>

<?php
  // Rapleaf code to request personalization data.
  include 'rapleaf_request.php';
  print Rapleaf\request($_GET['email']);
?>

<a href="personalized_location.php">View personalized location</a>

