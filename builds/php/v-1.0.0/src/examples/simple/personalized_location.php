<?php
    include "rapleaf_client.php";

    $personalization = Rapleaf\get_personalization();
    $location = $personalization->{'location'};

    $news_url = "http://news.google.com/news/search?" .
                "pz=1&cf=all&ned=us&hl=en&as_q=&as_e" .
                "pq=&as_oq=&as_eq=&as_scoring=r&btnG" .
                "=Search&as_drrb=q&as_qdr=a&as_minm=" .
                "9&as_mind=12&as_maxm=10&as_maxd=12&" .
                "as_nsrc=&as_nloc=&geo=" . $location  .
                "&as_author=&as_occt=any";

    print "<a href=\"" . $news_url . "\">View news for your location</a>";
?>