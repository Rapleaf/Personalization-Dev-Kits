#!/bin/bash

API_KEY="SET ME!"

HOST="personalize.rapleaf.com"
BASEPATH="/v4/dr"
TIMEOUT=2

function queryByEmails
{
  emails=$1                   # Array of string emails
  showAvailable=${2-false}    # Boolean specifying whether or not to report that RapLeaf has data for additional fields
  
  urlConcat=""
  for ((i=0; i<${#emails[@]}; i++)) {
    urlConcat=$urlConcat`buildUrl ${emails[i]} $showAvailable`" "
  }
  
  response=`curl -s -A 'RapleafApi/bash/1.0' -m $TIMEOUT $urlConcat`
  echo $response
}

function buildUrl
{
  email=$1                    # String email
  showAvailable=${2-false}    # Boolean specifying whether or not to report that RapLeaf has data for additional fields
  
  url="https://"$HOST$BASEPATH"?api_key="$API_KEY"&email="$email
  if $showAvailable ; then
    url=$url"&show_available=1"
  fi
  
  echo $url
}
