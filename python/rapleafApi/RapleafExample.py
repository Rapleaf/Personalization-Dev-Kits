import RapleafApi
import sys

""" This example script takes an e-mail as a command line argument 
and queries Rapleaf's database for any data associated with
the provided e-mail (unknown fields are left blank) 
The hash returned from query_by_email is iterated through
and each k/v pair is sent to std out """
  
api_key = 'SET_ME'        # Set your API key here
email = sys.argv[1]         # Command line argument
api = RapleafApi.RapleafApi(api_key)   # Instance of the API class
try:
    response = api.query_by_email(email)
    for k, v in response.iteritems():
        print('%s = %s' % (k, v))
except Exception as inst:       # HTTP code returned other than 200
    code, body = inst.args
    print('Error Code = %d' % code)
    print('Error Body = %s' % body)
