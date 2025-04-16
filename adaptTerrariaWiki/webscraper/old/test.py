from urllib.request import urlopen
from bs4 import BeautifulSoup
import re
import json

class TestClass:
    # doesn't work
    def rangedWeaponsTerrariaScrapingNew(self):
        url = "https://terraria.fandom.com/wiki/Ranged_weapons"
        page = urlopen(url)
        html = page.read().decode("utf-8")
        soup = BeautifulSoup(html, "html.parser")
        # Find the table with the specified class
        # table = soup.find_all("table", class_="sortable terraria lined jquery-tablesorter")
        table = soup.find("table")

        if table:
            # Extract text from table
            table = (table.encode('utf-8'))
            soup = BeautifulSoup(table, "html.parser")
            table = soup.find('table')

            data = []
            for row in table.find_all('tr')[1:]:  # Skip the first row (header row)
                # Initialize an empty dictionary to store row data
                row_data = {}

                # Iterate over each cell in the row
                
                link_tag = row.find('a')
                if link_tag:
                    first_image = link_tag.find('img')
                    if first_image:
                        row_data['Name'] = first_image['alt']
                        if first_image['src']:
                            row_data['Image'] = first_image['src']
                        elif first_image['data-src']:
                            row_data['Image'] = first_image['data-src']
                    row_data['Href'] = link_tag['href']
                cells = row.find_all(['th', 'td'])
                row_data['Damage'] = cells[3].get_text(strip=True)
                row_data['Source'] = cells[4].get_text(strip=True)
                row_data['Notes'] = cells[5].get_text(strip=True)

                # Append the row data to the list
                data.append(row_data)
                
            # print(json.dumps(data, indent=4))
        else:
            print("Table not found.")

    # doesn't work
    def rangedWeaponsTerrariaScraping(self):
        url = "https://terraria.fandom.com/wiki/Ranged_weapons"
        page = urlopen(url)
        html = page.read().decode("utf-8")

        pattern = r'<table class="sortable terraria lined jquery-tablesorter".*?>.*?</table>'
        match_results = re.search(pattern, html, re.IGNORECASE | re.DOTALL)
        if match_results:
            text = match_results.group()
            text = re.sub("<.*?>", "", text) # Remove HTML tags
            print(text)
        else:
            print("Table not found.")

    def scrapInfo(self):
        url = "http://olympus.realpython.org/profiles/dionysus"
        # url = "https://terraria.fandom.com/wiki/Ranged_weapons"
        page = urlopen(url)
        html = page.read().decode("utf-8")

        for string in ["Name: ", "Favorite Color:"]:
            string_start_idx = html.find(string)
            text_start_idx = string_start_idx + len(string)

            next_html_tag_offset = html[text_start_idx:].find("<")
            text_end_idx = text_start_idx + next_html_tag_offset

            raw_text = html[text_start_idx : text_end_idx]
            clean_text = raw_text.strip(" \r\n\t")
            print(clean_text)

    def scrapTitle(self):
        # url = "http://olympus.realpython.org/profiles/dionysus"
        url = "https://terraria.fandom.com/wiki/Ranged_weapons"
        page = urlopen(url)
        html = page.read().decode("utf-8")

        pattern = "<title.*?>.*?</title.*?>"
        match_results = re.search(pattern, html, re.IGNORECASE)
        title = match_results.group()
        title = re.sub("<.*?>", "", title) # Remove HTML tags

        print("title")
        print(title)

    def replaceString(self):
        string = "Everything is <replaced> if it's in <tags>."
        string = re.sub("<.*>", "ELEPHANTS", string)
        print(string) # 'Everything is ELEPHANTS.'

        string = "Everything is <replaced> if it's in <tags>."
        string = re.sub("<.*?>", "ELEPHANTS", string)
        print(string) # 'Everything is ELEPHANTS if it's in ELEPHANTS.'

    def findString(self):
        # Find all the occurrences of a substring in a string
        re.findall("ab*c", "ac") # ['ac']
        re.findall("ab*c", "ABC", re.IGNORECASE) # ['ABC']

        # Find the first occurrence of a substring in a string
        match_results = re.search("ab*c", "ABC", re.IGNORECASE)
        match_results.group() # 'ABC'


    def scrapePage(self):

        url1 = "http://olympus.realpython.org/profiles/aphrodite"
        url2 = "http://olympus.realpython.org/profiles/poseidon"

        page = urlopen(url2)
        html_bytes = page.read()
        html = html_bytes.decode("utf-8")

        title_index = html.find("<title>")
        start_index = title_index + len("<title>")
        end_index = html.find("</title>")
        title = html[start_index:end_index]

        print(title)