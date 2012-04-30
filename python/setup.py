from distutils.core import setup

setup(
    name='RapLeafApi',
    author='RapLeaf',
    author_email='developer@rapleaf.com',
    version='0.1.0',
    packages=['rapleafApi'],
    url='http://www.rapleaf.com',
    description='A library for interacting with RapLeaf\'s Personalization API',
    keywords='rapleaf api',
    long_description=open('README.txt').read(),
    requires=['urllib3'],
)
