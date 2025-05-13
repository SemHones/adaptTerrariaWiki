from urllib.request import urlopen
from bs4 import BeautifulSoup
import base64
import requests

class ScrapeSummon:
    def __init__(self, url):
        self.url = url
        self.soup = None